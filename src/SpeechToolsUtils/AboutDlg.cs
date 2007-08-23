// --------------------------------------------------------------------------------------------
#region // Copyright © 2002-2004, SIL International. All Rights Reserved.   
// <copyright from='2002' to='2004' company='SIL International'>
//		Copyright © 2002-2004, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: AboutDlg.cs
// Responsibility: TE Team
// 
// <remarks>
// Help About dialog
// </remarks>
// --------------------------------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SIL.SpeechTools.Utils
{
	#region IAboutDlg interface
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Public interface (exported with COM wrapper) for the FW Help About dialog box
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[ComVisible(true)]
	[GuidAttribute("8E90485A-1047-4270-A4D2-8BFA9C1D5ED7")]
	public interface IAboutDlg
	{
		/// -----------------------------------------------------------------------------------
		/// <summary>
		/// Shows the form as a modal dialog with the currently active form as its owner
		/// </summary>
		/// ------------------------------------------------------------------------------------
		int ShowDialog();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The build description which appears in the Build label on the about dialog box
		/// </summary>
		/// <remarks>
		/// .Net clients should not set this. It will be ignored.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		string Build
		{
			set;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets a flag indicating whether or not the text "Beta" should follow the version no.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		bool IsBetaVersion
		{
			set;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The product name which appears in the Name label on the about dialog box
		/// </summary>
		/// <remarks>
		/// .Net clients should not set this. It will be ignored.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		string ProdName
		{
			set;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The product version which appears in the App Version label on the about dialog box
		/// </summary>
		/// <remarks>
		/// .Net clients should not set this. It will be ignored.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		string ProdVersion
		{
			set;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The copyright info which appears in the Copyright label on the splash screen
		/// </summary>
		/// <remarks>
		/// .Net clients should not set this. It will be ignored.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		string Copyright
		{
			set;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The drive letter whose free space will be reported in the About box.
		/// </summary>
		/// <remarks>
		/// .Net clients should not set this. It will be ignored.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		string DriveLetter
		{
			set;
		}
	}

	#endregion

	#region AboutDlg implementation
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// FW Help about dialog (previously HelpAboutDlg in AfDialog.cpp)
	/// </summary>
	/// <remarks>
	/// This dialog shows the registration key from HKLM\Software\SIL\FieldWorks\FwUserReg.
	/// If a DropDeadDate is to something different then 1/1/3000 it also displays the date 
	/// after which the program is no longer working. 
	/// </remarks>
	/// ----------------------------------------------------------------------------------------
	[ProgId("SpeechToolsInterfaces.AboutDlg")]
	// Key attribute to hide the "clutter" from System.Windows.Forms.Form
	[ClassInterface(ClassInterfaceType.None)]
	[GuidAttribute("7ACF99EB-7FB6-482a-B1C2-11E2E14C02A0")]
	[ComVisible(true)]
	public class AboutDlg : Form, IAboutDlg
	{
		#region Data members

		private IContainer components;

		private string m_sAvailableMemoryFmt;
		private string m_sAppVersionFmt;
		private string m_sTitleFmt;
		private string m_sAvailableDiskSpaceFmt;
		private string m_buildFmt;
		private bool m_showBuildNum = false;
		private bool m_isBetaVersion = false;
		private Panel panel1;
		private Label lblBuild;
		private Label lblAppVersion;
		private Label lblAvailableMemoryValue;
		private Label lblAvailableDiskSpaceValue;
		private Label lblCopyright;
		private Label lblName;
		private string m_sDriveLetter;
		private string m_tmpProdVersion;
		#endregion

		#region Construction and Disposal
		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Constructor
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		public AboutDlg()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Cache the format strings. The control labels will be overwritten when the
			// dialog is shown.
			m_sAvailableMemoryFmt = lblAvailableMemoryValue.Text;
			m_sAppVersionFmt = lblAppVersion.Text;
			m_sTitleFmt = Text;
			m_sAvailableDiskSpaceFmt = lblAvailableDiskSpaceValue.Text;
			m_buildFmt = lblBuild.Text;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public AboutDlg(bool showBuildNum, bool isBetaVersion) : this()
		{
			m_showBuildNum = showBuildNum;
			m_isBetaVersion = isBetaVersion;
		}

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		protected override void Dispose( bool disposing )
		{
			// Must not be run more than once.
			if (IsDisposed)
				return;

			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.ToolTip m_toolTip;
			System.Windows.Forms.Button buttonOk;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDlg));
			System.Windows.Forms.Label lblAvailableMemory;
			System.Windows.Forms.Label lblAvailableDiskSpace;
			System.Windows.Forms.PictureBox fieldWorksIcon;
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblBuild = new System.Windows.Forms.Label();
			this.lblAppVersion = new System.Windows.Forms.Label();
			this.lblAvailableMemoryValue = new System.Windows.Forms.Label();
			this.lblAvailableDiskSpaceValue = new System.Windows.Forms.Label();
			this.lblCopyright = new System.Windows.Forms.Label();
			this.lblName = new System.Windows.Forms.Label();
			m_toolTip = new System.Windows.Forms.ToolTip(this.components);
			buttonOk = new System.Windows.Forms.Button();
			lblAvailableMemory = new System.Windows.Forms.Label();
			lblAvailableDiskSpace = new System.Windows.Forms.Label();
			fieldWorksIcon = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(fieldWorksIcon)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_toolTip
			// 
			m_toolTip.AutomaticDelay = 100;
			m_toolTip.AutoPopDelay = 1000;
			m_toolTip.InitialDelay = 100;
			m_toolTip.ReshowDelay = 100;
			// 
			// buttonOk
			// 
			resources.ApplyResources(buttonOk, "buttonOk");
			buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			buttonOk.Name = "buttonOk";
			m_toolTip.SetToolTip(buttonOk, resources.GetString("buttonOk.ToolTip"));
			buttonOk.UseVisualStyleBackColor = true;
			// 
			// lblAvailableMemory
			// 
			resources.ApplyResources(lblAvailableMemory, "lblAvailableMemory");
			lblAvailableMemory.Name = "lblAvailableMemory";
			// 
			// lblAvailableDiskSpace
			// 
			resources.ApplyResources(lblAvailableDiskSpace, "lblAvailableDiskSpace");
			lblAvailableDiskSpace.Name = "lblAvailableDiskSpace";
			// 
			// fieldWorksIcon
			// 
			fieldWorksIcon.ErrorImage = null;
			fieldWorksIcon.Image = global::SIL.SpeechTools.Utils.Properties.Resources.kimidSilLogo;
			resources.ApplyResources(fieldWorksIcon, "fieldWorksIcon");
			fieldWorksIcon.InitialImage = null;
			fieldWorksIcon.Name = "fieldWorksIcon";
			fieldWorksIcon.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.lblBuild);
			this.panel1.Controls.Add(this.lblAppVersion);
			this.panel1.Controls.Add(this.lblAvailableMemoryValue);
			this.panel1.Controls.Add(this.lblAvailableDiskSpaceValue);
			this.panel1.Controls.Add(lblAvailableMemory);
			this.panel1.Controls.Add(lblAvailableDiskSpace);
			this.panel1.Controls.Add(this.lblCopyright);
			this.panel1.Controls.Add(fieldWorksIcon);
			this.panel1.Controls.Add(this.lblName);
			this.panel1.Controls.Add(buttonOk);
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			// 
			// lblBuild
			// 
			resources.ApplyResources(this.lblBuild, "lblBuild");
			this.lblBuild.Name = "lblBuild";
			// 
			// lblAppVersion
			// 
			resources.ApplyResources(this.lblAppVersion, "lblAppVersion");
			this.lblAppVersion.Name = "lblAppVersion";
			// 
			// lblAvailableMemoryValue
			// 
			resources.ApplyResources(this.lblAvailableMemoryValue, "lblAvailableMemoryValue");
			this.lblAvailableMemoryValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblAvailableMemoryValue.Name = "lblAvailableMemoryValue";
			// 
			// lblAvailableDiskSpaceValue
			// 
			resources.ApplyResources(this.lblAvailableDiskSpaceValue, "lblAvailableDiskSpaceValue");
			this.lblAvailableDiskSpaceValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblAvailableDiskSpaceValue.Name = "lblAvailableDiskSpaceValue";
			// 
			// lblCopyright
			// 
			resources.ApplyResources(this.lblCopyright, "lblCopyright");
			this.lblCopyright.Name = "lblCopyright";
			// 
			// lblName
			// 
			resources.ApplyResources(this.lblName, "lblName");
			this.lblName.Name = "lblName";
			// 
			// AboutDlg
			// 
			this.AcceptButton = buttonOk;
			resources.ApplyResources(this, "$this");
			this.BackColor = System.Drawing.Color.White;
			this.ControlBox = false;
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			((System.ComponentModel.ISupportInitialize)(fieldWorksIcon)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		#region Initialization Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the window handle gets created we want to initialize the controls
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			try
			{
				// Set the Application label to the name of the app
				Assembly assembly = Assembly.GetEntryAssembly();
				string strRoot;
				if (assembly == null)
				{	
					// Must be called from COM client
					strRoot = m_sDriveLetter + ":\\";
					lblBuild.Visible = m_showBuildNum;
				}
				else
				{
					SetVersionInformation(assembly);

					// Set the title bar text
					Text = string.Format(m_sTitleFmt, Application.ProductName);
					strRoot = Application.ExecutablePath.Substring(0, 2) + "\\";
				}

				SetCopyrightInformation();
				SetMemoryAndDiskInformation(strRoot);
			}
			catch
			{
				// ignore errors
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the application's version information.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetVersionInformation(Assembly assembly)
		{
			object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
			
			string productName = Application.ProductName;
			
			if (string.IsNullOrEmpty(productName) && attributes != null && attributes.Length > 0)
				productName = ((AssemblyTitleAttribute)attributes[0]).Title;

			lblName.Text = productName;

			lblBuild.Visible = m_showBuildNum;
			if (m_showBuildNum)
			{
				// The build number is just the number of days since 01/01/2000
				int bldNum = assembly.GetName().Version.Build;
				DateTime bldDate = new DateTime(2000, 1, 1).Add(new TimeSpan(bldNum, 0, 0, 0));
				lblBuild.Text = string.Format(m_buildFmt, bldDate.ToString("dd-MMM-yyyy"));
			}

			string appVersion = assembly.GetName().Version.ToString(2);
#if DEBUG
			lblAppVersion.Text = string.Format(m_sAppVersionFmt, appVersion, "(Debug version)",
				(m_isBetaVersion ? "Beta" : string.Empty));
#else
			lblAppVersion.Text = string.Format(m_sAppVersionFmt, appVersion, string.Empty,
				(m_isBetaVersion ? "Beta" : string.Empty));
#endif
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the copyright information.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetCopyrightInformation()
		{
			// Get copyright information from assembly info. By doing this we don't have
			// to update the about dialog each year.
			string copyright;
			object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(
				typeof(AssemblyCopyrightAttribute), false);

			// Try to get the copyright from the executing assembly.
			// If that fails, use a generic one.
			copyright = (attributes != null && attributes.Length > 0 ?
				((AssemblyCopyrightAttribute)attributes[0]).Copyright :
				"(C) 2002-2007 SIL International");

			lblCopyright.Text = string.Format(lblCopyright.Text, copyright.Replace("(C)", "©"));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Fills in the memory usage and disk space information.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetMemoryAndDiskInformation(string strRoot)
		{
			// Set the memory information in MB.
			STUtils.MemoryStatus ms = new STUtils.MemoryStatus();
			STUtils.GlobalMemoryStatus(ref ms);
			double available = (double)ms.dwAvailPhys / Math.Pow(1024, 2);
			double total = (double)ms.dwTotalPhys / Math.Pow(1024, 2);
			lblAvailableMemoryValue.Text = string.Format(m_sAvailableMemoryFmt,
				available.ToString("###,###,###,###,###.##"),
				total.ToString("###,###,###,###,###"));

			// Set the disk space information in KB and GB.
			ulong freeDiskSpace = STUtils.GetFreeDiskSpace(strRoot);
			ulong kbFree = freeDiskSpace / 1024;
			double gbFree = (double)freeDiskSpace / Math.Pow(1024, 3);
			lblAvailableDiskSpaceValue.Text = string.Format(m_sAvailableDiskSpaceFmt,
				kbFree.ToString("###,###,###,###,###,###,###"),
				gbFree.ToString("##,###.#"), strRoot);
		}

		#endregion

		#region IAboutDlg interface implementation
		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the form as a modal dialog with the currently active form as its owner
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		int IAboutDlg.ShowDialog()
		{
			return (int)base.ShowDialog();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The build description which appears in the Build label on the about dialog box
		/// </summary>
		/// <remarks>
		/// .Net clients should not set this. It will be ignored.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		string IAboutDlg.Build
		{
			set 
			{
				lblBuild.Text = value;
				m_showBuildNum = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Indicates whether or not the term "Beta" should follow the version number.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		bool IAboutDlg.IsBetaVersion
		{
			set
			{
				m_isBetaVersion = value;
				if (m_tmpProdVersion != null)
				{
#if DEBUG
					lblAppVersion.Text = string.Format(m_sAppVersionFmt, m_tmpProdVersion,
						"(Debug version)", (m_isBetaVersion ? "Beta" : string.Empty));
#else
					lblAppVersion.Text = string.Format(m_sAppVersionFmt, m_tmpProdVersion,
						string.Empty, (m_isBetaVersion ? "Beta" : string.Empty));
#endif
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The product name which appears in the Name label on the about dialog box
		/// </summary>
		/// <remarks>
		/// .Net clients should not set this. It will be ignored.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		string IAboutDlg.ProdName
		{
			set
			{
				lblName.Text = value;
				Text = string.Format(m_sTitleFmt, value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The product version which appears in the App Version label on the about dialog box
		/// </summary>
		/// <remarks>
		/// .Net clients should not set this. It will be ignored.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		string IAboutDlg.ProdVersion
		{
			set 
			{
				m_tmpProdVersion = value;
#if DEBUG
				lblAppVersion.Text = string.Format(m_sAppVersionFmt, value, "(Debug version)",
					(m_isBetaVersion ? "Beta" : string.Empty));
#else
				lblAppVersion.Text = string.Format(m_sAppVersionFmt, value, string.Empty,
					(m_isBetaVersion ? "Beta" : string.Empty));
#endif
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The copyright info which appears in the Copyright label on the about dialog box
		/// </summary>
		/// <remarks>
		/// .Net clients should not set this. It will be ignored.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		string IAboutDlg.Copyright
		{
			set
			{
				value = value.Replace("\n", " ");
				lblCopyright.Text = value.Replace("(C)", "©");
			}
		}
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The drive letter whose free space will be reported in the About box.
		/// </summary>
		/// <remarks>
		/// .Net clients should not set this. It will be ignored.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		string IAboutDlg.DriveLetter
		{
			set { m_sDriveLetter = value; }
		}

		#endregion
	}

	#endregion
}
