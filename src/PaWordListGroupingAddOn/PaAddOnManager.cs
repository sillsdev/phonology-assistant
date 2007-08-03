using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using SIL.Pa;
using SIL.Pa.Controls;
using SIL.SpeechTools.Utils;
using SIL.FieldWorks.Common.UIAdapters;
using XCore;

namespace SIL.Pa.AddOn
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaAddOnManager : IxCoreColleague
	{
		private CustomDropDown m_dropDown;
		private NumberOfPhonesToMatchCtrl m_numPhonesCtrl;
		private List<PaWordListGrid> m_grids = new List<PaWordListGrid>();
		
		private Dictionary<PaWordListGrid, int> m_numPhonesBefore =
			new Dictionary<PaWordListGrid, int>();

		private Dictionary<PaWordListGrid, int> m_numPhonesAfter =
			new Dictionary<PaWordListGrid, int>();
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaAddOnManager()
		{
			PaApp.AddMediatorColleague(this);
			m_dropDown = new CustomDropDown();
			m_numPhonesCtrl = new NumberOfPhonesToMatchCtrl();
			m_numPhonesCtrl.lnkApply.Click += lnkApply_Click;
			m_dropDown.AddControl(m_numPhonesCtrl);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAfterWordListGrouped(object args)
		{
			try
			{
				PaWordListGrid grid = args as PaWordListGrid;

				if (grid != null && !m_grids.Contains(grid) &&
					grid.Cache != null && grid.Cache.IsForSearchResults)
				{
					m_grids.Add(grid);
					m_numPhonesBefore[grid] = WordListGroupingBuilder.NumberOfAfterPhonesToMatch;
					m_numPhonesAfter[grid] = WordListGroupingBuilder.NumberOfAfterPhonesToMatch;
					grid.HandleDestroyed += grid_HandleDestroyed;
					grid.ColumnHeaderMouseClick += grid_ColumnHeaderMouseClick;
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
		void grid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				PaWordListGrid grid = sender as PaWordListGrid;

				if (grid == null || e.ColumnIndex != 0 ||
					!(grid.Columns[0] is SilHierarchicalGridColumn) || grid.Cache == null ||
					!grid.Cache.IsForSearchResults || grid.SortOptions.AdvSortOrder[1] == 0 ||
					!grid.SortOptions.SortInformationList[0].FieldInfo.IsPhonetic)
				{
					return;
				}

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
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAfterWordListUnGrouped(object args)
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
