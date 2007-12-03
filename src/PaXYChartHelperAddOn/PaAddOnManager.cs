using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using SIL.Pa;
using SIL.Pa.Data;
using SIL.Pa.Controls;
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
		private const string kAutoFillToken = "«auto-filled»";
		private const string kOrigRowToken = "«original-row»";
		private const string kOrigColToken = "«original-col»";
		private const string kAutoFillConMarker = "«[C]»";
		private const string kAutoFillVowMarker = "«[V]»";

		private XYChartVw m_view;
		private XYGrid m_xyGrid;
		private bool m_chartHasBeenAutoFilled = false;

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
		private void FillCVInSearchItem(IPACharacterType charType, string srchItem, int i)
		{
			m_chartHasBeenAutoFilled = true;

			int startIndex = i;
			string fmt = srchItem.Replace((charType == IPACharacterType.Consonant ?
				kAutoFillConMarker : kAutoFillVowMarker), "{0}");

			// Mark the original row and save the pattern with the auto. fill marker in
			// the tag property of the first cell in the row (i.e the search item cell).
			m_xyGrid.Rows[i].Tag = kOrigRowToken;
			m_xyGrid[0, i].Tag = srchItem;

			// Get a phone list sorted by MOA
			List<string> phoneList = SortedPhones(charType == IPACharacterType.Consonant ?
				IPACharacterType.Consonant : IPACharacterType.Vowel);

			foreach (string phone in phoneList)
			{
				if (i > startIndex)
				{
					m_xyGrid.Rows.InsertCopy(i - 1, i);
					m_xyGrid.Rows[i].Tag = kAutoFillToken;
				}

				m_xyGrid[0, i++].Value = string.Format(fmt, phone);
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
			bool dirtyState = m_xyGrid.IsDirty;

			// Restore the original search environments in the columns and removed
			// the columns that were auto. generated and filled by this add-on.
			for (int i = m_xyGrid.ColumnCount - 1; i > 0; i--)
			{
				try
				{
					if ((m_xyGrid.Columns[i].Tag as string) == kAutoFillToken)
						m_xyGrid.Columns.RemoveAt(i);
					else if ((m_xyGrid.Columns[i].Tag as string) == kOrigColToken)
					{
						m_xyGrid[i, 0].Value = m_xyGrid[i, 0].Tag as string;
						m_xyGrid.Columns[i].Tag = null;
					}
				}
				catch { }
			}

			// Restore the original search items in the rows and removed the
			// rows that were auto. generated and filled by this add-on.
			for (int i = m_xyGrid.RowCount - 1; i > 0; i--)
			{
				try
				{
					if ((m_xyGrid.Rows[i].Tag as string) == kAutoFillToken)
						m_xyGrid.Rows.RemoveAt(i);
					else if ((m_xyGrid.Rows[i].Tag as string) == kOrigRowToken)
					{
						m_xyGrid[0, i].Value = m_xyGrid[0, i].Tag as string;
						m_xyGrid.Rows[i].Tag = null;
					}
				}
				catch { }
			}

			// Now clear out all the result cells since their values probably don't make
			// sense now that the grid has been restored to its state before it was filled.
			foreach (DataGridViewRow row in m_xyGrid.Rows)
			{
				if (row.Index == 0 || row.Index == m_xyGrid.NewRowIndex)
					continue;

				foreach (DataGridViewColumn col in m_xyGrid.Columns)
				{
					if (col.Index > 0)
						row.Cells[col.Index].Value = null;
				}
			}

			m_chartHasBeenAutoFilled = false;
			m_xyGrid.IsDirty = dirtyState;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private List<string> SortedPhones(IPACharacterType charType)
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
