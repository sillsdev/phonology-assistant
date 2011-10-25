using System;
using System.ComponentModel;
using System.Windows.Forms;
using Localization.UI;
using SIL.Pa.UI.Controls;
using SilTools;
using Utils=SilTools.Utils;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class DefineClassBaseDlg : OKCancelDlgBase
	{
		protected readonly ClassesDlg m_classesDlg;
		protected readonly ClassListViewItem m_classInfo;
		protected readonly ClassListViewItem m_origClassInfo;

		#region Construction and setup
		/// ------------------------------------------------------------------------------------
		public DefineClassBaseDlg()
		{
			Utils.WaitCursors(true);
			InitializeComponent();

			lblClassType.Font = FontHelper.UIFont;
			lblClassTypeValue.Font = FontHelper.UIFont;
			lblClassName.Font = FontHelper.UIFont;
			txtClassName.Font = FontHelper.UIFont;
			txtMembers.Font = FontHelper.UIFont;
			lblMembers.Font = FontHelper.UIFont;
			rbMatchAll.Font = FontHelper.UIFont;
			rbMatchAny.Font = FontHelper.UIFont;

			LocalizeItemDlg.StringsLocalized += SetLocalizedTexts;
			SetLocalizedTexts();
		}

		/// ------------------------------------------------------------------------------------
		public DefineClassBaseDlg(ClassListViewItem classInfo, ClassesDlg classDlg) : this()
		{
			m_classesDlg = classDlg;
			m_origClassInfo = classInfo;
			m_classInfo = new ClassListViewItem(classInfo);
			m_classInfo.IsDirty = false;

			txtClassName.Text = m_classInfo.Text;
			txtMembers.Text = m_classInfo.FormattedMembersString;
			txtMembers.SelectionStart = txtMembers.Text.Length + 1;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void SetLocalizedTexts()
		{
			if (!DesignMode)
				throw new NotImplementedException();
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ClassListViewItem ClassInfo
		{
			get {return m_classInfo;}
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TextBox TxtClassName
		{
			get { return txtClassName; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the pattern that would be built from the contents of the members text box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected virtual string CurrentPattern
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

		#region Overridden methods
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			Utils.WaitCursors(false);
			base.OnShown(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get	
			{
				if (m_origClassInfo == null)
					return true;

				return (CurrentPattern != m_origClassInfo.Pattern ||
					m_classInfo.Text != m_origClassInfo.Text);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Return true if data is OK.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool Verify()
		{
			txtClassName.Text = txtClassName.Text.Trim();

			// Ensure the new class doesn't have an empty class name
			if (txtClassName.Text == string.Empty)
			{
				Utils.MsgBox(App.GetString("DefineClassDlg.EmptyClassNameMsg", "Class name must not be empty."));
				return false;
			}

			if (m_classesDlg == null)
				return true;

			bool exists = m_classesDlg.ClassListView.DoesClassNameExist(
				txtClassName.Text, m_origClassInfo, true);
			
			if (exists)
			{
				txtClassName.Focus();
				txtClassName.SelectAll();
			}

			return !exists;
		}

		#endregion
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			m_classInfo.Pattern = CurrentPattern;
			return true;
		}

		#region Event handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the class name changing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtClassName_TextChanged(object sender, EventArgs e)
		{
			m_classInfo.Text = txtClassName.Text.Trim();
			m_classInfo.IsDirty = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the user choosing one of the items in the tsbWhatToInclude drop-down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void HandleScopeClick(object sender, EventArgs e)
		{
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleMembersTextBoxKeyDown(object sender, KeyEventArgs e)
		{
		}

		#endregion
	}
}