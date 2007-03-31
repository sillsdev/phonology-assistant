using System;
using System.Collections.Generic;
using System.Text;

namespace SIL.SpeechTools.Database
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Class, a collection of which, is used for the speaker combo. box on the content window.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SpeakerInfo
	{
		private int m_id;
		private string m_name;
		private Gender m_gender;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Id
		{
			get {return m_id;}
			internal set {m_id = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Name
		{
			get
			{
				return (m_name == DBUtilsStrings.kstidDropDownNoneEntry ?
					string.Empty : m_name);
			}
			set { m_name = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Gender Gender
		{
			get { return m_gender; }
			set { m_gender = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return m_name;
		}
	}
}
