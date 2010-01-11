using System.Windows.Forms;
using System.Xml;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.UI;
using SilUtils;

namespace SIL.Pa.AddOn
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaAddOnManager : IxCoreColleague
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaAddOnManager()
		{
			try
			{
				PaApp.AddMediatorColleague(this);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnMainViewOpened(object args)
		{
			try
			{
				if (args is PaMainWnd)
				{
					PaApp.TMAdapter.AddCommandItem("CmdAddOnInfo", "AddOnInformation",
						Properties.Resources.kstidMenuText, null, null, null, null,
						null, Keys.None, null, null);

					TMItemProperties itemProps = new TMItemProperties();
					itemProps.CommandId = "CmdAddOnInfo";
					itemProps.Name = "mnuAddOnInfo";
					itemProps.Text = null;
					PaApp.TMAdapter.AddMenuItem(itemProps, "mnuTools", "mnuOptions");
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
		protected bool OnAddOnInformation(object args)
		{
			try
			{
				using (AddOnInfoDlg dlg = new AddOnInfoDlg())
					dlg.ShowDialog(PaApp.MainForm);
			}
			catch { }

			return true;
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
			return (new IxCoreColleague[] {this});
		}

		#endregion
	}
}
