// Copyright (c) 2010-2018 SIL International
// License: MIT
using System;
using System.IO;
using System.Windows.Forms;

namespace SIL.PaToFdoInterfaces
{
    public partial class ChooseProject : Form
    {
        public string SelectedProject { get; set; }

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
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            SelectedProject = (string)listBox1.SelectedItem;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
