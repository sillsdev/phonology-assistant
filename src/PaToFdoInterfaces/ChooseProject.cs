// Copyright (c) 2010-2018 SIL International
// License: MIT

using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using L10NSharp;

namespace SIL.PaToFdoInterfaces
{
    public partial class ChooseProject : Form
    {
        public string SelectedProject { get; private set; }

        public ChooseProject()
        {
            InitializeComponent();
        }

        private void ChooseProject_Load(object sender, EventArgs e)
        {
            foreach (var project in Directory.GetDirectories(new PaFieldWorksHelper().ProjectsDirectory))
            {
                if (Directory.GetFiles(project, "*.fwdata").Length > 0)
                {
                    listBox1.Items.Add(Path.GetFileName(project));
                }
            }
	        Ok.Enabled = false;
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            SelectedProject = (string)listBox1.SelectedItem;
            DialogResult = DialogResult.OK;
            Close();
        }

		private void AnotherLocation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var filter = LocalizationManager.GetString("PaToFdoInterfaces.Filters.ChooseProject",
				"Data (*.fwdata)|*.fwdata");
			var dlg = new OpenFileDialog
			{
				Filter = filter,
				FilterIndex = 1,
				CheckFileExists = true,
				InitialDirectory = Properties.Settings.Default.FieldWorksProjectsFolder
			};
			if (dlg.ShowDialog() != DialogResult.OK) return;
			SelectedProject = dlg.FileName;
			DialogResult = DialogResult.OK;
			Close();
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBox1.SelectedIndex > 0 && listBox1.SelectedIndex < listBox1.Items.Count) Ok.Enabled = true;
		}

		private void Help_Click(object sender, EventArgs e)
		{
			System.Windows.Forms.Help.ShowHelp(new Label(), HelpFilePath, "User_Interface/Menus/File/Add_data_sources.htm");
		}

		#region Help related properties and methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When running normally, this should be the same as Application.StartupPath. When
		/// running tests, this will be the folder that contains the assembly in which this
		/// class is found.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string AssemblyPath
		{
			get
			{
				// CodeBase prepends "file:/" (Win) or "file:" (Linux), which must be removed.
				int prefixLen = (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX) ? 5 : 6;
				return Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Substring(prefixLen);
			}
		}

		/// ------------------------------------------------------------------------------------
		private static string s_helpFilePath;
		public const string kHelpFileName = "Phonology_Assistant_Help.chm";
		public static string HelpFilePath
		{
			get
			{
				if (string.IsNullOrEmpty(s_helpFilePath))
				{
					s_helpFilePath = AssemblyPath;
					s_helpFilePath = Path.Combine(s_helpFilePath, kHelpFileName);
				}

				return s_helpFilePath;
			}
		}
		#endregion Help Properties
	}
}
