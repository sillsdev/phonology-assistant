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
	/// Add-On for misc. added XY Chart functionality.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaAddOnManager : IxCoreColleague
	{
		private XYChartVw m_view;

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
			if (args is XYChartVw)
				m_view = args as XYChartVw;

			return false;
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
				XYGrid grid = args as XYGrid;
				grid.ChartLayout.SearchItems = new List<string>(PaApp.PhoneCache.Consonants);
				STUtils.SetWindowRedraw(grid, false, false);

				for (int i = grid.RowCount - 1; i > 0; i--)
				{
					if (i != grid.NewRowIndex)
						grid.Rows.RemoveAt(i);
				}

				grid.RowCount = grid.ChartLayout.SearchItems.Count + 1;

				for (int i = 0; i < grid.ChartLayout.SearchItems.Count; i++)
					grid[0, i + 1].Value = grid.ChartLayout.SearchItems[i];

				STUtils.SetWindowRedraw(grid, true, true);
			}
			catch { }

			return false;
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
