using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Palaso.Reporting;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class ClassesDlg : OKCancelDlgBase
	{
		public PaProject Project { get; private set; }
		public ClassListView ClassListView { get; private set; }

		/// ------------------------------------------------------------------------------------
		public ClassesDlg()
		{
			InitializeComponent();

			if (App.DesignMode)
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

			cmnuAddCharClass.Click += delegate { AddClass(SearchClassType.Phones); };
			cmnuAddArtFeatureClass.Click += delegate { AddClass(SearchClassType.Articulatory); };
			cmnuAddBinFeatureClass.Click += delegate { AddClass(SearchClassType.Binary); };
		}
	
		/// ------------------------------------------------------------------------------------
		public ClassesDlg(PaProject project) : this()
		{
			Project = project;
			ClassListView.Load();
			HandleClassesListViewSelectedIndexChanged(null, null);
			ClassListView.Font = FontHelper.UIFont;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			
			if (!App.DesignMode)
				ClassListView.LoadSettings(Name);
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Save the form's state.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			ClassListView.SaveSettings(Name);
			base.SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save changes made to any of the classes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			if (ClassListView.Items.Cast<ListViewItem>().Any(item => item.Text == string.Empty))
			{
				var msg = App.GetString("ClassesDlg.EmptyClassNameMsg", "Class name must not be empty.");
				ErrorReport.NotifyUserOfProblem(msg);
				return false;
			}

			ClassListView.SaveChanges();
			App.MsgMediator.SendMessage("SearchClassesChanged", null);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get	{return (ClassListView.IsDirty || base.IsDirty);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the state of the buttons according to the item selected.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleClassesListViewSelectedIndexChanged(object sender, EventArgs e)
		{
			var item = (ClassListView.SelectedItems.Count > 0 ?
				ClassListView.SelectedItems[0] as ClassListViewItem : null);

			ClassListView.LabelEdit = (item != null && item.AllowEdit);
			btnModify.Enabled = (item != null && item.AllowEdit);
			btnDelete.Enabled = (item != null && item.AllowEdit);
			btnCopy.Enabled = (item != null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Treat a double-click like clicking the modify button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleClassesListViewDoubleClick(object sender, EventArgs e)
		{
			var item = (ClassListView.SelectedItems.Count > 0 ?
				ClassListView.SelectedItems[0] as ClassListViewItem : null);

			if (item != null && item.AllowEdit)
				btnModify.PerformClick();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Treat pressing the enter key like clicking the modify button and pressing the
		/// delete key like clicking on the delete button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleClassesListViewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return && btnModify.Enabled)
				btnModify.PerformClick();
			else if (e.KeyCode == Keys.Delete && btnDelete.Enabled)
				btnDelete.PerformClick();
			else if (e.KeyCode == Keys.F2 && btnModify.Enabled && ClassListView.SelectedItems.Count > 0)
				ClassListView.SelectedItems[0].BeginEdit();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Modify the currently selected class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleModifyButtonClick(object sender, EventArgs e)
		{
			if (ClassListView.SelectedItems.Count == 0)
				return;

			var item = ClassListView.SelectedItems[0] as ClassListViewItem;
			if (item == null)
				return;

			using (var dlg = GetDefineClassDialogForItem(item.ClassType, item))
			{
				//dlg.TxtClassName.Enabled = false;
				var result = dlg.ShowDialog(this);
				if (result == DialogResult.Yes || result == DialogResult.OK)
				{
					item.Copy(dlg.ClassInfo);
					item.IsDirty = true;
					ClassListView.Focus();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private DefineClassBaseDlg GetDefineClassDialogForItem(SearchClassType type, ClassListViewItem item)
		{
			switch (type)
			{
				case SearchClassType.Phones: return new DefinePhoneClassDlg(item, this);
				case SearchClassType.Articulatory: return new DefineDescriptiveFeatureClassDlg(item, this);
				case SearchClassType.Binary: return new DefineDistinctiveFeatureClassDlgBase(item, this);
				default: return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new (user-defined) class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleAddButtonClick(object sender, EventArgs e)
		{
			Point pt = btnAdd.PointToScreen(new Point(0, btnAdd.Height));
			cmnuAdd.Show(pt);
		}

		/// ------------------------------------------------------------------------------------
		private void AddClass(SearchClassType type)
		{
			using (var dlg = GetDefineClassDialogForItem(type, null))
			{
				var result = dlg.ShowDialog(this);
				if (result == DialogResult.Yes || result == DialogResult.OK)
				{
					var item = dlg.ClassInfo;
					item.SubItems.Add(new ListViewItem.ListViewSubItem());
					item.SubItems.Add(new ListViewItem.ListViewSubItem());
					CopyAndInsertItem(item, null);
					ClassListView.Focus();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make a copy of the currently selected class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleCopyButtonClick(object sender, EventArgs e)
		{
			if (ClassListView.SelectedItems.Count <= 0)
				return;

			var item = ClassListView.SelectedItems[0] as ClassListViewItem;
			if (item == null)
				return;

			var fmt = App.GetString("ClassesDlg.CopyClassPrefix", "Copy of {0}",
				"Prefix for names of copied items");

			string baseName = string.Format(fmt, item.Text);
			string newName = baseName;

			fmt = App.GetString("ClassesDlg.CopyClassNameFormat", "{0} ({1:D2})",
				"Format for name of copied class. First parameter is the copied class name and second is a two digit number to make the name unique.");

			int i = 1;
			while (ClassListView.DoesClassNameExist(newName, null, false))
				newName = string.Format(fmt, baseName, i++);

			item = CopyAndInsertItem(item, newName);
			ClassListView.LabelEdit = true;
			item.BeginEdit();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Delete the currently selected class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleDeleteButtonClick(object sender, EventArgs e)
		{
			if (ClassListView.SelectedItems.Count > 0)
				ClassListView.DeleteItem(ClassListView.SelectedItems[0] as ClassListViewItem);

			ClassListView.Focus();
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
			
			if (className != null)
				newItem.Text = className;

			ClassListView.Items.Add(newItem);
			newItem.Selected = true;
			newItem.IsDirty = true;
			return newItem;
		}
	}
}