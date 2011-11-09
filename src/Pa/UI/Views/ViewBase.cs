using System.Windows.Forms;
using SIL.Pa.Model;

namespace SIL.Pa.UI.Views
{
	public class ViewBase : UserControl
	{
		public PaProject Project { get; private set; }

		/// ------------------------------------------------------------------------------------
		public ViewBase(PaProject project)
		{
			Project = project;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool OnProjectLoaded(object args)
		{
			if (args is PaProject)
				Project = args as PaProject;

			return false;
		}
	}
}
