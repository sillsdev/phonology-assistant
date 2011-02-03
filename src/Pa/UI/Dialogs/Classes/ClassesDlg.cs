using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class ClassesDlg : OKCancelDlgBase
	{
		/// ------------------------------------------------------------------------------------
		public ClassesDlg()
		{
			InitializeComponent();

			if (DesignMode)
				return;

			btnAdd.Margin = new Padding(0, btnOK.Margin.Top, btnOK.Margin.Left, btnOK.Margin.Bottom);
			btnModify.Margin = btnAdd.Margin;
			btnCopy.Margin = btnAdd.Margin;
			btnDelete.Margin = btnAdd.Margin;

			tblLayoutButtons.ColumnCount += 4;
			tblLayoutButtons.ColumnStyles.Insert(0, new ColumnStyle());
			tblLayoutButtons.ColumnStyles.Insert(0, new ColumnStyle());
			tblLayoutButtons.ColumnStyles.Insert(0, new ColumnStyle());
			tblLayoutButtons.ColumnStyles.Insert(0, new ColumnStyle());

			tblLayoutButtons.Controls.Add(btnAdd, 0, 0);
			tblLayoutButtons.Controls.Add(btnModify, 1, 0);
			tblLayoutButtons.Controls.Add(btnCopy, 2, 0);
			tblLayoutButtons.Controls.Add(btnDelete, 3, 0);
			ReAddButtons(5);

			lvClasses.Load();
			lvClasses_SelectedIndexChanged(null, null);
			lvClasses.Font = FontHelper.UIFont;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			lvClasses.LoadSettings(Name);
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Save the form's state.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			lvClasses.SaveSettings(Name);
			base.SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save changes made to any of the classes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			if (lvClasses.Items.Cast<ListViewItem>().Any(item => item.Text == string.Empty))
			{
				var msg = App.LocalizeString("EmptyClassNameMsg",
					"Class name must not be empty.", App.kLocalizationGroupMisc);
				
				Utils.MsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}

			lvClasses.SaveChanges();
			App.MsgMediator.SendMessage("SearchClassesChanged", null);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get	{return (lvClasses.IsDirty || base.IsDirty);}
		}

		/// ------------------------------------------------------------------------------------
		public ClassListView ClassListView
		{
			get { return lvClasses; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the state of the buttons according to the item selected.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lvClasses_SelectedIndexChanged(object sender, EventArgs e)
		{
			var item = (lvClasses.SelectedItems.Count > 0 ?
				lvClasses.SelectedItems[0] as ClassListViewItem : null);

			lvClasses.LabelEdit = (item != null && item.AllowEdit);
			btnModify.Enabled = (item != null && item.AllowEdit);
			btnDelete.Enabled = (item != null && item.AllowEdit);
			btnCopy.Enabled = (item != null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Treat a double-click like clicking the modify button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lvClasses_DoubleClick(object sender, EventArgs e)
		{
			var item = (lvClasses.SelectedItems.Count > 0 ?
				lvClasses.SelectedItems[0] as ClassListViewItem : null);

			if (item != null && item.AllowEdit)
				btnModify.PerformClick();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Treat pressing the enter key like clicking the modify button and pressing the
		/// delete key like clicking on the delete button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lvClasses_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return && btnModify.Enabled)
				btnModify.PerformClick();
			else if (e.KeyCode == Keys.Delete && btnDelete.Enabled)
				btnDelete.PerformClick();
			else if (e.KeyCode == Keys.F2 && btnModify.Enabled && lvClasses.SelectedItems.Count > 0)
				lvClasses.SelectedItems[0].BeginEdit();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Modify the currently selected class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnModify_Click(object sender, EventArgs e)
		{
			if (lvClasses.SelectedItems.Count == 0)
				return;

			var item = lvClasses.SelectedItems[0] as ClassListViewItem;

			if (item == null)
				return;

			using (var dlg = new DefineClassDlg(item, this))
			{
				//dlg.TxtClassName.Enabled = false;
				var result = dlg.ShowDialog(this);
				if (result == DialogResult.Yes || result == DialogResult.OK)
				{
					item.Copy(dlg.ClassInfo);
					item.IsDirty = true;
					lvClasses.Focus();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new (user-defined) class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnAdd_Click(object sender, EventArgs e)
		{
			Point pt = btnAdd.PointToScreen(new Point(0, btnAdd.Height));
			cmnuAdd.Show(pt);
		}

		/// ------------------------------------------------------------------------------------
		private void cmnuAddCharClass_Click(object sender, EventArgs e)
		{
			AddClass(SearchClassType.Phones);
		}

		/// ------------------------------------------------------------------------------------
		private void cmnuAddArtFeatureClass_Click(object sender, EventArgs e)
		{
			AddClass(SearchClassType.Articulatory);
		}

		/// ------------------------------------------------------------------------------------
		private void cmnuAddBinFeatureClass_Click(object sender, EventArgs e)
		{
			AddClass(SearchClassType.Binary);
		}
		
		/// ------------------------------------------------------------------------------------
		private void AddClass(SearchClassType type)
		{
			using (var dlg = new DefineClassDlg(type, this))
			{
				var result = dlg.ShowDialog(this);
				if (result == DialogResult.Yes || result == DialogResult.OK)
				{
					var item = dlg.ClassInfo;
					item.SubItems.Add(new ListViewItem.ListViewSubItem());
					item.SubItems.Add(new ListViewItem.ListViewSubItem());
					CopyAndInsertItem(item, null);
					lvClasses.Focus();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make a copy of the currently selected class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnCopy_Click(object sender, EventArgs e)
		{
			if (lvClasses.SelectedItems.Count <= 0)
				return;

			var item = lvClasses.SelectedItems[0] as ClassListViewItem;
			if (item == null)
				return;

			var fmt = App.LocalizeString("ClassesDlg.CopyClassPrefix", "Copy of {0}",
				"Prefix for names of copied items", App.kLocalizationGroupDialogs);

			string baseName = string.Format(fmt, item.Text);
			string newName = baseName;

			fmt = App.LocalizeString("ClassesDlg.CopyClassNameFormat", "{0} ({1:D2})",
				"Format for name of copied class. First parameter is the copied class name and second is a two digit number to make the name unique.",
				App.kLocalizationGroupDialogs);

			int i = 1;
			while (lvClasses.DoesClassNameExist(newName, null, false))
				newName = string.Format(fmt, baseName, i++);

			item = CopyAndInsertItem(item, newName);
			lvClasses.LabelEdit = true;
			item.BeginEdit();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Delete the currently selected class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (lvClasses.SelectedItems.Count > 0)
				lvClasses.DeleteItem(lvClasses.SelectedItems[0] as ClassListViewItem);

			lvClasses.Focus();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a copy of the specified class and inserts it into the class list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private ClassListViewItem CopyAndInsertItem(ClassListViewItem item, string className)
		{
			if (item == null)
				return null;

			var newItem = new ClassListViewItem(item);
			//newItem.Id = 0;
			
			if (className != null)
				newItem.Text = className;

			//newItem.Group = lvClasses.Groups[(int)ClassGroup.UserDefined];
			lvClasses.Items.Add(newItem);
			newItem.Selected = true;
			newItem.IsDirty = true;
			return newItem;
		}
	}
}