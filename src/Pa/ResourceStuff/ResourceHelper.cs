using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace SIL.Pa.Resources
{
	/// ----------------------------------------------------------------------------------------
	public partial class ResourceHelper : Form
	{
		private static ResourceManager s_stringResources;
		private static ResourceManager s_helpResources;

		/// ------------------------------------------------------------------------------------
		public ResourceHelper()
		{
			InitializeComponent();

			if (s_stringResources == null)
			{
				s_stringResources = new ResourceManager(
					"SIL.Pa.Resources.PaStrings", Assembly.GetExecutingAssembly());
			}

			if (s_helpResources == null)
			{
				s_helpResources = new ResourceManager(
					"SIL.Pa.Resources.HelpTopicPaths", Assembly.GetExecutingAssembly());
			}
		}

		/// -----------------------------------------------------------------------------------
		/// <summary>
		/// Returns a string from the resource file using the specified resource ID.
		/// </summary>
		/// <param name="stid">String resource id</param>
		/// <param name="resMngr">Resource manager from which to get string specified by
		/// stid</param>
		/// -----------------------------------------------------------------------------------
		private static string GetResourceString(string stid, ResourceManager resMngr)
		{
			string str = (stid == null || resMngr == null ? null : resMngr.GetString(stid));
			return (string.IsNullOrEmpty(str) ? stid : str);
		}

		/// -----------------------------------------------------------------------------------
		/// <summary>
		/// Returns a string from the resource file using the specified resource ID.
		/// </summary>
		/// <param name="stid">String resource id</param>
		/// <returns>String</returns>
		/// -----------------------------------------------------------------------------------
		public static string GetResourceString(string stid)
		{
			if (s_stringResources == null)
			{
				s_stringResources = new ResourceManager(
					"SIL.Pa.Resources.PaStrings", Assembly.GetExecutingAssembly());
			}

			return GetResourceString(stid, s_stringResources);
		}

		/// -----------------------------------------------------------------------------------
		/// <summary>
		/// Return a help topic or help file path.
		/// </summary>
		/// <param name="hid">String resource id</param>
		/// <returns>String</returns>
		/// -----------------------------------------------------------------------------------
		 public static string GetHelpString(string hid)
		{
			if (s_helpResources == null)
			{
				s_helpResources = new ResourceManager(
					"SIL.Pa.Resources.HelpTopicPaths", Assembly.GetExecutingAssembly());
			}

			if (string.IsNullOrEmpty(hid))
				hid = "hidTopicDoesNotExist";

			return s_helpResources.GetString(hid);
		}
	}
}