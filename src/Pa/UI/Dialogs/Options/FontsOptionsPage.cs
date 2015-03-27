// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System.IO;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	public partial class FontsOptionsPage : OptionsDlgPageBase
	{
		private readonly FieldFontsGrid _grid;

		/// ------------------------------------------------------------------------------------
		public FontsOptionsPage(PaProject project) : base(project)
		{
			InitializeComponent();
			
			var mappedFields = (m_project == null ? null : m_project.GetMappedFields());

			_grid = new FieldFontsGrid(mappedFields);
			_grid.Dock = DockStyle.Fill;
			_panelFonts.Controls.Add(_grid);

			if (Settings.Default.OptionsDlgFontGrid != null)
			{
				Settings.Default.OptionsDlgFontGrid.InitializeGrid(_grid);
				_grid.ShowFontColumn(true);
			}
			else
			{
				_grid.AutoResizeColumnHeadersHeight();
				_grid.AutoResizeColumns();
				_grid.AutoResizeRows();
				_grid.Columns["tgtfield"].Width = 150;
				_grid.Columns["font"].Width = 250;
			}
		}

		/// ------------------------------------------------------------------------------------
		public override string TabPageText
		{
			get { return LocalizationManager.GetString("DialogBoxes.OptionsDlg.FontsTab.TabText", "Fonts"); }
		}

		/// ------------------------------------------------------------------------------------
		public override string HelpId
		{
			get { return "hidFontsOptions"; }
		}
	
		// ------------------------------------------------------------------------------------
		public override bool IsDirty
		{
			get { return _grid.IsDirty; }
		}

		/// ------------------------------------------------------------------------------------
		public override void Save()
		{
			try
			{
				foreach (var kvp in _grid.Fonts)
				{
					var field = m_project.Fields.SingleOrDefault(f => f.Name == kvp.Key);
					if (field != null)
						field.Font = kvp.Value;
				}

				// Since the fonts changed, delete the project's style sheet file. This will
				// force it to be recreated the next time something is exported that needs it.
				if (File.Exists(m_project.CssFileName))
					File.Delete(m_project.CssFileName);

				App.MsgMediator.SendMessage("PaFontsChanged", null);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		public override void SaveSettings()
		{
			Settings.Default.OptionsDlgFontGrid = GridSettings.Create(_grid);
			base.SaveSettings();
		}
	}
}
