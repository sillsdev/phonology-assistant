using System;
using System.Drawing;
using System.Windows.Forms;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.UI.Controls;
using SilUtils;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class ClassesDlg : OKCancelDlgBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ClassesDlg()
		{
			InitializeComponent();
			App.SettingsHandler.LoadFormProperties(this);
			lvClasses.Load();
			lvClasses.LoadSettings(Name);
			lvClasses_SelectedIndexChanged(null, null);
			lvClasses.Font = FontHelper.UIFont;
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Save the form's state.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			base.SaveSettings();
			App.SettingsHandler.SaveFormProperties(this);
			lvClasses.SaveSettings(Name);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save changes made to any of the classes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			foreach (ListViewItem item in lvClasses.Items)
			{
				// Ensure any renamed classes don't have empty class names
				if (item.Text == string.Empty)
				{
					Utils.MsgBox(Properties.Resources.kstidDefineClassEmptyClassName,
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}
			}

			lvClasses.SaveChanges();
			App.MsgMediator.SendMessage("SearchClassesChanged", null);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get	{return (lvClasses.IsDirty || base.IsDirty);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
			ClassListViewItem item = (lvClasses.SelectedItems.Count > 0 ?
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
			ClassListViewItem item = (lvClasses.SelectedItems.Count > 0 ?
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

			ClassListViewItem item = lvClasses.SelectedItems[0] as ClassListViewItem;

			if (item == null)
				return;

			using (DefineClassDlg dlg = new DefineClassDlg(item, this))
			{
				//dlg.TxtClassName.Enabled = false;
				DialogResult result = dlg.ShowDialog(this);
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void cmnuAddCharClass_Click(object sender, EventArgs e)
		{
			AddClass(SearchClassType.Phones);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void cmnuAddArtFeatureClass_Click(object sender, EventArgs e)
		{
			AddClass(SearchClassType.Articulatory);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void cmnuAddBinFeatureClass_Click(object sender, EventArgs e)
		{
			AddClass(SearchClassType.Binary);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddClass(SearchClassType type)
		{
			using (DefineClassDlg dlg = new DefineClassDlg(type, this))
			{
				DialogResult result = dlg.ShowDialog(this);
				if (result == DialogResult.Yes || result == DialogResult.OK)
				{
					ClassListViewItem item = dlg.ClassInfo;
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
			if (lvClasses.SelectedItems.Count > 0)
			{
				ClassListViewItem item = lvClasses.SelectedItems[0] as ClassListViewItem;
				if (item != null)
				{
					string baseName =
						string.Format(Properties.Resources.kstidClassCopyOfPrefix, item.Text);

					string newName = baseName;

					int i = 1;
					while (lvClasses.DoesClassNameExist(newName, null, false))
					{
						newName = string.Format(Properties.Resources.kstidClassCopyNameFormat,
							baseName, i++);
					}

					item = CopyAndInsertItem(item, newName);
					lvClasses.LabelEdit = true;
					item.BeginEdit();
				}
			}
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

			ClassListViewItem newItem = new ClassListViewItem(item);
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