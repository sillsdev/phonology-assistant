// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace SIL.Pa.Resources
{
	/// ----------------------------------------------------------------------------------------
	public partial class ResourceHelper : Form
	{
		private static ResourceManager s_helpResources;

		/// ------------------------------------------------------------------------------------
		public ResourceHelper()
		{
			InitializeComponent();
		}

		/// -----------------------------------------------------------------------------------
		/// <summary>
		/// Return a help topic or help file path.
		/// </summary>
		/// -----------------------------------------------------------------------------------
		 public static string GetHelpString(string hid)
		{
			if (s_helpResources == null)
			{
				s_helpResources = new ResourceManager(
					"SIL.Pa.ResourceStuff.HelpTopicPaths", Assembly.GetExecutingAssembly());
			}

			if (string.IsNullOrEmpty(hid))
				hid = "hidTopicDoesNotExist";

			return s_helpResources.GetString(hid);
		}
	}
}