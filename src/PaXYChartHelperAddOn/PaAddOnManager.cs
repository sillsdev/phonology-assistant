using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using SIL.Pa;
using SIL.Pa.Data;
using SIL.Pa.Controls;
using SIL.Pa.FFSearchEngine;
using SIL.SpeechTools.Utils;
using SIL.FieldWorks.Common.UIAdapters;
using XCore;

namespace SIL.Pa.AddOn
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Add-On for misc. added XY Chart functionality.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaAddOnManager : IxCoreColleague
	{
		private const string kAutoFillToken = "첺uto-filled�";
		private const string kOrigRowToken = "쳍riginal-row�";
		private const string kOrigColToken = "쳍riginal-col�";
		private const string kAutoFillConMarker = "�[C]�";
		private const string kAutoFillVowMarker = "�[V]�";

		private XYChartVw m_view;
		private XYGrid m_xyGrid;
		private bool m_chartHasBeenAutoFilled = false;
		private List<DataGridViewColumn> m_addedColumns = new List<DataGridViewColumn>();
		private List<DataGridViewRow> m_addedRows = new List<DataGridViewRow>();
		private Dictionary<int, string> m_origSrchItems = new Dictionary<int, string>();
		private Dictionary<int, string> m_origEnvironments = new Dictionary<int, string>();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaAddOnManager()
		{
			PaApp.AddMediatorColleague(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewOpened(object args)
		{
			if (!(args is XYChartVw))
				return false;

			try
			{
				m_view = args as XYChartVw;
				m_xyGrid = ReflectionHelper.GetField(m_view, "m_xyGrid") as XYGrid;
				m_xyGrid.CellBeginEdit += m_xyGrid_CellBeginEdit;
				SetupAutoFillMarkerMenus();
			}
			catch { }

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupAutoFillMarkerMenus()
		{
			try
			{
				string menuText = string.Format("{0} Auto. Fill Consonant Marker", kAutoFillConMarker);
				m_view.TMAdapter.AddCommandItem("CmdInsertAutoFillConMarker", "InsertAutoFillConMarker",
					menuText, null, null, null, null, null, Keys.None, null, null);

				TMItemProperties autoFillItemProps = new TMItemProperties();
				autoFillItemProps.BeginGroup = true;
				autoFillItemProps.CommandId = "CmdInsertAutoFillConMarker";
				autoFillItemProps.Name = "mnuInsertAutoFillConMarker";
				autoFillItemProps.Text = null;
				m_view.TMAdapter.AddMenuItem(autoFillItemProps, "tbbInsertIntoChart", null);

				autoFillItemProps = new TMItemProperties();
				autoFillItemProps.BeginGroup = true;
				autoFillItemProps.CommandId = "CmdInsertAutoFillConMarker";
				autoFillItemProps.Name = "cmnuInsertAutoFillConMarker";
				autoFillItemProps.Text = null;
				m_view.TMAdapter.AddMenuItem(autoFillItemProps, "cmnuInsertIntoChart", null);

				menuText = string.Format("{0} Auto. Fill Vowel Marker", kAutoFillVowMarker);
				m_view.TMAdapter.AddCommandItem("CmdInsertAutoFillVowMarker", "InsertAutoFillVowMarker",
					menuText, null, null, null, null, null, Keys.None, null, null);

				autoFillItemProps = new TMItemProperties();
				autoFillItemProps.BeginGroup = false;
				autoFillItemProps.CommandId = "CmdInsertAutoFillVowMarker";
				autoFillItemProps.Name = "mnuInsertAutoFillVowMarker";
				autoFillItemProps.Text = null;
				m_view.TMAdapter.AddMenuItem(autoFillItemProps, "tbbInsertIntoChart", null);

				autoFillItemProps = new TMItemProperties();
				autoFillItemProps.BeginGroup = false;
				autoFillItemProps.CommandId = "CmdInsertAutoFillVowMarker";
				autoFillItemProps.Name = "cmnuInsertAutoFillVowMarker";
				autoFillItemProps.Text = null;
				m_view.TMAdapter.AddMenuItem(autoFillItemProps, "cmnuInsertIntoChart", null);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnBeforeXYChartFilled(object args)
		{
			try
			{
				STUtils.SetWindowRedraw(m_xyGrid, false, false);
				RemoveAutoFilledRowsAndColumns();
				bool dirtyState = m_xyGrid.IsDirty;

				for (int i = 1; i < m_xyGrid.ColumnCount; i++)
				{
					string environment = m_xyGrid[i, 0].Value as string;
					if (environment != null)
					{
						if (environment.Contains(kAutoFillConMarker))
							FillCVInEnvironment(IPACharacterType.Consonant, environment, i);
						else if (environment.Contains(kAutoFillVowMarker))
							FillCVInEnvironment(IPACharacterType.Vowel, environment, i);
					}
				}

				for (int i = 1; i < m_xyGrid.RowCount; i++)
				{
					string srchItem = m_xyGrid[0, i].Value as string;
					if (srchItem != null)
					{
						if (srchItem.Contains(kAutoFillConMarker))
							FillCVInSearchItem(IPACharacterType.Consonant, srchItem, i);
						else if (srchItem.Contains(kAutoFillVowMarker))
							FillCVInSearchItem(IPACharacterType.Vowel, srchItem, i);
					}
				}

				m_xyGrid.IsDirty = dirtyState;
			}
			catch {	}

			STUtils.SetWindowRedraw(m_xyGrid, true, true);
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void FillCVInEnvironment(IPACharacterType charType, string environment, int colIndex)
		{
			m_chartHasBeenAutoFilled = true;

			// Save the original environment pattern.
			m_origEnvironments[colIndex] = environment;

			int startIndex = colIndex;
			string fmt = environment.Replace((charType == IPACharacterType.Consonant ?
				kAutoFillConMarker : kAutoFillVowMarker), "{0}");

			// Get a phone list sorted by MOA
			List<string> phoneList = GetSortedPhones(charType == IPACharacterType.Consonant ?
				IPACharacterType.Consonant : IPACharacterType.Vowel);

			SearchQuery query = m_xyGrid.Columns[colIndex].Tag as SearchQuery;

			foreach (string phone in phoneList)
			{
				if (colIndex == startIndex)
					query.Pattern = string.Format(fmt, phone);
				else
				{
					m_xyGrid.Columns.Insert(colIndex, SilGrid.CreateTextBoxColumn(string.Empty));
					m_xyGrid[colIndex, 0].Value = string.Format(fmt, phone);
					SearchQuery newQuery = query.Clone();
					newQuery.Pattern = m_xyGrid[colIndex, 0].Value as string;
					m_xyGrid.Columns[colIndex].Tag = newQuery;
					m_addedColumns.Add(m_xyGrid.Columns[colIndex]);
				}

				m_xyGrid[colIndex++, 0].Value = string.Format(fmt, phone);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void FillCVInSearchItem(IPACharacterType charType, string srchItem, int rowIndex)
		{
			m_chartHasBeenAutoFilled = true;

			// Save the original search item pattern.
			m_origSrchItems[rowIndex] = srchItem;

			int startIndex = rowIndex;
			string fmt = srchItem.Replace((charType == IPACharacterType.Consonant ?
				kAutoFillConMarker : kAutoFillVowMarker), "{0}");

			// Get a phone list sorted by MOA
			List<string> phoneList = GetSortedPhones(charType == IPACharacterType.Consonant ?
				IPACharacterType.Consonant : IPACharacterType.Vowel);

			foreach (string phone in phoneList)
			{
				if (rowIndex > startIndex)
				{
					m_xyGrid.Rows.InsertCopy(rowIndex - 1, rowIndex);
					m_xyGrid[0, rowIndex].Value = string.Format(fmt, phone);
					m_addedRows.Add(m_xyGrid.Rows[rowIndex]);
				}

				m_xyGrid[0, rowIndex++].Value = string.Format(fmt, phone);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_xyGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			try
			{
				if (m_chartHasBeenAutoFilled)
				{
					RestoreMarkersMessageDlg.Show(true);
					RemoveAutoFilledRowsAndColumns();
					e.Cancel = true;
				}
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes all the rows and columns in an XY grid that were generated from a
		/// previous auto. fill procedure.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void RemoveAutoFilledRowsAndColumns()
		{
			if (!m_chartHasBeenAutoFilled)
				return;

			bool dirtyState = m_xyGrid.IsDirty;

			// Restore the original search environments in the columns.
			foreach (KeyValuePair<int, string> kvp in m_origEnvironments)
			{
				try
				{
					m_xyGrid[kvp.Key, 0].Value = kvp.Value;
					(m_xyGrid.Columns[kvp.Key].Tag as SearchQuery).Pattern = kvp.Value;
				}
				catch { }
			}

			// Restore the original search items in the rows.
			foreach (KeyValuePair<int, string> kvp in m_origSrchItems)
			{
				try
				{
					m_xyGrid[0, kvp.Key].Value = kvp.Value;
					for (int i = 1; i < m_xyGrid.Rows[kvp.Key].Cells.Count; i++)
						m_xyGrid.Rows[kvp.Key].Cells[i].Value = null;
				}
				catch { }
			}

			// Remove all the automatically generated columns.
			foreach (DataGridViewColumn col in m_addedColumns)
			{
				try
				{
					m_xyGrid.Columns.Remove(col);
				}
				catch { }
			}

			// Remove all the automatically generated rows.
			foreach (DataGridViewRow row in m_addedRows)
			{
				try
				{
					m_xyGrid.Rows.Remove(row);
				}
				catch { }
			}

			m_origEnvironments.Clear();
			m_origSrchItems.Clear();
			m_addedRows.Clear();
			m_addedColumns.Clear();
			m_chartHasBeenAutoFilled = false;
			m_xyGrid.IsDirty = dirtyState;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private List<string> GetSortedPhones(IPACharacterType charType)
		{
			SortedDictionary<string, string> sortedPhonesDict = new SortedDictionary<string, string>();

			foreach (KeyValuePair<string, IPhoneInfo> kvp in PaApp.PhoneCache)
			{
				if (kvp.Value.CharType == charType)
					sortedPhonesDict[kvp.Value.MOAKey] = kvp.Key;
			}

			return new List<string>(sortedPhonesDict.Values);
		}

		#region Message handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnSaveChart(object args)
		{
			try
			{
				if (m_chartHasBeenAutoFilled)
				{
					RestoreMarkersMessageDlg.Show(false);
					RemoveAutoFilledRowsAndColumns();
				}
			}
			catch { }

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnSaveChartAs(object args)
		{
			try
			{
				if (m_chartHasBeenAutoFilled)
				{
					RestoreMarkersMessageDlg.Show(false);
					RemoveAutoFilledRowsAndColumns();
				}
			}
			catch { }

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnInsertAutoFillConMarker(object args)
		{
			if (m_xyGrid != null)
				m_xyGrid.InsertTextInCell(kAutoFillConMarker);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateInsertAutoFillConMarker(object args)
		{
			return HandleAutoFillMenuItemUpdate(args as TMItemProperties);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnInsertAutoFillVowMarker(object args)
		{
			if (m_xyGrid != null)
				m_xyGrid.InsertTextInCell(kAutoFillVowMarker);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateInsertAutoFillVowMarker(object args)
		{
			return HandleAutoFillMenuItemUpdate(args as TMItemProperties);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool HandleAutoFillMenuItemUpdate(TMItemProperties itemProps1)
		{
			if (itemProps1 != null)
			{
				itemProps1.Visible = true;
				itemProps1.Update = true;

				if (itemProps1.Name.StartsWith("mnuInsertAutoFill"))
					itemProps1.Enabled = true;
				else
				{
					TMItemProperties itemProps2 =
						m_view.TMAdapter.GetItemProperties("cmnuInsertORGroup");

					itemProps1.Enabled = (itemProps2 != null && itemProps2.Enabled);
				}
			}

			return true;
		}

		#endregion

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Never used.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, XmlNode configurationParameters)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the message target.
		/// </summary>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			List<IxCoreColleague> targets = new List<IxCoreColleague>();
			targets.Add(this);
			return (targets.ToArray());
		}

		#endregion
	}
}
