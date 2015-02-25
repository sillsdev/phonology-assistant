// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.IO;
using SIL.Pa;
using SIL.Pa.Controls;
using SIL.SpeechTools.Utils;
using SIL.FieldWorks.Common.UIAdapters;
using XCore;

// I don't want to use a custom attribute, so I'm
// kludging what I want by using this attribute.
[assembly: System.Reflection.AssemblyDefaultAlias("CanBeDisabled")]

namespace SIL.Pa.SearchResultAddOn
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaAddOnManager : IxCoreColleague
	{
		private const string ksilhgcolName = "silhgc";

		private readonly Font m_fntGlyph;
		private readonly CustomDropDown m_dropDown;
		private readonly NumberOfPhonesToMatchCtrl m_numPhonesCtrl;
		private readonly List<PaWordListGrid> m_grids = new List<PaWordListGrid>();
		
		private readonly Dictionary<PaWordListGrid, int> m_numPhonesBefore =
			new Dictionary<PaWordListGrid, int>();

		private readonly Dictionary<PaWordListGrid, int> m_numPhonesAfter =
			new Dictionary<PaWordListGrid, int>();

		private SearchVw m_view;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaAddOnManager()
		{
			try
			{
				Assembly assembly = Assembly.GetExecutingAssembly();
				string settingName = Path.GetFileNameWithoutExtension(assembly.CodeBase);
				if (!PaApp.SettingsHandler.GetBoolSettingsValue(settingName, "Enabled", true))
					return;

				PaApp.AddMediatorColleague(this);
				m_dropDown = new CustomDropDown();
				m_numPhonesCtrl = new NumberOfPhonesToMatchCtrl();
				m_numPhonesCtrl.lnkApply.Click += lnkApply_Click;
				m_dropDown.AddControl(m_numPhonesCtrl);

				m_fntGlyph = new Font("Marlett", 9);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewOpened(object args)
		{
			try
			{
				if (args is SearchVw)
				{
					m_view = args as SearchVw;
					SetupAddOnToolbarButtons();
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
		private void SetupAddOnToolbarButtons()
		{
			try
			{
				string tooltip = "CAE Sort";
				m_view.TMAdapter.AddCommandItem("CmdCAESort", "CAESort",
					"CAE", null, null, tooltip, null, null, Keys.None, null, null);

				string xml = "name=\"tbbCAESort\" commandid=\"CmdCAESort\" " +
					"type=\"0\" displaytype=\"1\" begingroup=\"true\"";

				m_view.TMAdapter.AddToolBarItem("tbFFWnd", xml,	"tbbPhoneticSort", true);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnCAESort(object args)
		{
			if (m_view.ResultViewManger == null || m_view.ResultViewManger.CurrentViewsGrid == null)
				return true;

			PaWordListGrid grid = m_view.ResultViewManger.CurrentViewsGrid;
			if (grid.IsGroupedByField)
				grid.GroupByField = null;

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateCAESort(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps != null)
			{
				itemProps.Update = true;
				itemProps.Visible = true;
				itemProps.Enabled = (m_view.ResultViewManger != null &&
					m_view.ResultViewManger.CurrentViewsGrid != null);
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAfterWordListGroupedByField(object args)
		{
			try
			{
				PaWordListGrid grid = args as PaWordListGrid;

				if (grid != null && !m_grids.Contains(grid) &&
					grid.Cache != null && grid.Cache.IsForSearchResults)
				{
					m_grids.Add(grid);
					m_numPhonesBefore[grid] = WordListGroupingBuilder.NumberOfBeforePhonesToMatch;
					m_numPhonesAfter[grid] = WordListGroupingBuilder.NumberOfAfterPhonesToMatch;
					grid.HandleDestroyed += grid_HandleDestroyed;
					grid.ColumnHeaderMouseClick += grid_ColumnHeaderMouseClick;
					OnWordListGridSorted(grid);
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
		protected bool OnWordListGridSorted(object args)
		{
			try
			{
				PaWordListGrid grid = args as PaWordListGrid;
				if (grid != null && m_grids.Contains(grid))
				{
					grid.Columns[0].HeaderCell.Style.Font = m_fntGlyph;
					grid.Columns[0].HeaderCell.Style.ForeColor = Color.CadetBlue;
					grid.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

					if (DoesGridQualifyForFeature(grid))
					{
						grid.Columns[0].Name = ksilhgcolName;
						grid.Columns[0].HeaderText = "6";
						grid.Columns[0].SortMode = DataGridViewColumnSortMode.Automatic;
					}
					else
					{
						grid.Columns[0].Name = string.Empty;
						grid.Columns[0].HeaderText = string.Empty;
						grid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
					}
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
		protected bool OnBeforeWordListSorted(object args)
		{
			try
			{
				string colName = ((object[])args)[0] as string;
				if (colName == ksilhgcolName)
					return true;
			}
			catch { }

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void grid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				PaWordListGrid grid = sender as PaWordListGrid;

				if (e.ColumnIndex != 0 || !DoesGridQualifyForFeature(grid))
					return;

				if (!grid.Focused)
				{
					grid.Focus();
					Application.DoEvents();
				}

				Rectangle rc = grid.GetCellDisplayRectangle(0, -1, false);
				Point pt = new Point(rc.X, rc.Bottom);
				pt = grid.PointToScreen(pt);

				m_numPhonesCtrl.NumberOfPhones = (grid.SortOptions.AdvSortOrder[0] == 0 ?
					m_numPhonesBefore[grid] : m_numPhonesAfter[grid]);

				m_numPhonesCtrl.Environment = (grid.SortOptions.AdvSortOrder[0] == 0 ?
					"before" : "after");

				m_dropDown.Tag = grid;
				m_dropDown.Show(pt);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool DoesGridQualifyForFeature(PaWordListGrid grid)
		{
			try
			{
				return (grid != null && grid.Columns[0] is SilHierarchicalGridColumn &&
					grid.Cache != null && grid.Cache.IsForSearchResults &&
					grid.SortOptions.SortInformationList[0].FieldInfo.IsPhonetic &&
					grid.SortOptions.AdvSortOrder[1] != 0);
			}
			catch { }

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void lnkApply_Click(object sender, EventArgs e)
		{
			try
			{
				m_dropDown.Close();

				PaWordListGrid grid = m_dropDown.Tag as PaWordListGrid;
				if (grid == null)
					return;

				if (grid.SortOptions.AdvSortOrder[0] == 0)
				{
					m_numPhonesBefore[grid] = WordListGroupingBuilder.NumberOfBeforePhonesToMatch =
						m_numPhonesCtrl.NumberOfPhones;
				}
				else
				{
					m_numPhonesAfter[grid] = WordListGroupingBuilder.NumberOfAfterPhonesToMatch =
						m_numPhonesCtrl.NumberOfPhones;
				}

				WordListGroupingBuilder.Group(grid);
				OnWordListGridSorted(grid);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAfterWordListUnGroupedByField(object args)
		{
			UnhookGrid(args as PaWordListGrid);
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void grid_HandleDestroyed(object sender, EventArgs e)
		{
			UnhookGrid(sender as PaWordListGrid);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UnhookGrid(PaWordListGrid grid)
		{
			try
			{
				if (grid != null && m_grids.Contains(grid))
				{
					grid.HandleDestroyed -= grid_HandleDestroyed;
					grid.ColumnHeaderMouseClick -= grid_ColumnHeaderMouseClick;
					m_grids.Remove(grid);
					m_numPhonesBefore.Remove(grid);
					m_numPhonesAfter.Remove(grid);
				}
			}
			catch { }
		}

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
