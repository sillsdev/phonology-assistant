using System.Windows.Forms;
using SIL.Pa.Model;

namespace SIL.Pa.UI.Views
{
	public class ViewBase : UserControl
	{
		protected PaProject _project;

		/// ------------------------------------------------------------------------------------
		protected bool OnProjectLoaded(object args)
		{
			if (args is PaProject)
				_project = args as PaProject;

			return false;
		}
	}
}
