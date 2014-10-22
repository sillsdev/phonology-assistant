using System;
using System.ComponentModel;
using System.Windows.Forms;
using Localization;
using Localization.UI;
using SIL.Pa.UI.Controls;
using SilTools;

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
			SilTools.Utils.WaitCursors(true);
			InitializeComponent();

			_labelClassType.Font = FontHelper.UIFont;
			_labelClassTypeValue.Font = FontHelper.UIFont;
			_labelClassName.Font = FontHelper.UIFont;
			_textBoxClassName.Font = FontHelper.UIFont;
			_textBoxMembers.Font = FontHelper.UIFont;
			_labelMembers.Font = FontHelper.UIFont;

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

			_textBoxClassName.Text = m_classInfo.Text;
			_textBoxMembers.Text = m_classInfo.FormattedMembersString;
			_textBoxMembers.SelectionStart = _textBoxMembers.Text.Length + 1;
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
			get { return _textBoxClassName; }
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
			SilTools.Utils.WaitCursors(false);
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
			_textBoxClassName.Text = _textBoxClassName.Text.Trim();

			// Ensure the new class doesn't have an empty class name
			if (_textBoxClassName.Text == string.Empty)
			{
				SilTools.Utils.MsgBox(LocalizationManager.GetString("DialogBoxes.DefineClassDlg.EmptyClassNameMsg", "Class name must not be empty."));
				return false;
			}

			if (m_classesDlg == null)
				return true;

			bool exists = m_classesDlg.ClassListView.DoesClassNameExist(
				_textBoxClassName.Text, m_origClassInfo, true);
			
			if (exists)
			{
				_textBoxClassName.Focus();
				_textBoxClassName.SelectAll();
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
			m_classInfo.Text = _textBoxClassName.Text.Trim();
			m_classInfo.IsDirty = true;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleMembersTextBoxKeyDown(object sender, KeyEventArgs e)
		{
		}

		#endregion
	}
}