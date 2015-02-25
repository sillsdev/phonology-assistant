// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Drawing;
using System.Windows.Forms;
using SIL.Pa.Model;

namespace SIL.Pa.UI.Dialogs
{
	public interface IOptionsDlgPage
	{
		string TabPageText { get; }
		string HelpId { get; }
		bool IsDirty { get; }
		void Save();
		void SaveSettings();
	}

	public partial class OptionsDlgPageBase : UserControl, IOptionsDlgPage
	{
		protected PaProject m_project;

		/// ------------------------------------------------------------------------------------
		public OptionsDlgPageBase()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		public OptionsDlgPageBase(PaProject project) : this()
		{
			m_project = project;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			if (!App.DesignMode)
			{
				BackColor = Color.Transparent;
				Dock = DockStyle.Fill;
			}

			AutoScroll = true;
			base.OnHandleCreated(e);
		}

		/// ------------------------------------------------------------------------------------
		public virtual string TabPageText
		{
			get { throw new NotImplementedException(); }
		}

		/// ------------------------------------------------------------------------------------
		public virtual string HelpId
		{
			get { throw new NotImplementedException(); }
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool IsDirty
		{
			get { throw new NotImplementedException(); }
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Save()
		{
			throw new NotImplementedException();
		}

		/// ------------------------------------------------------------------------------------
		public virtual void SaveSettings()
		{
		}
	}
}
