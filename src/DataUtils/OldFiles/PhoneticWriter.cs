using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;

namespace SIL.SpeechTools.Database
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Values in this enumeration correspond to the RelativeLocation field in the CharIndex table.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public enum RelativePhoneLocation : int
	{
		Alone = 0,
		Initial = 1,
		Medial = 2,
		Final = 3
	}

	#region PhoneticWordInfo Class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Stores misc. information that will be written for a single PhoneticList record.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PhoneticWordInfo
	{
		public int PhoneticListId;
		public string HexPhonetic;
		public string BaseHexPhonetic;
		public string CVPattern;
		public string MOASortKey;
		public string POASortKey;
		public string UDSortKey;
		public PhoneInfo[] Phones;
	}

	#endregion

	#region PhoneInfo Class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PhoneInfo
	{
		public string Phone;
		public int CharListId;
		public int IPACharacterId;
		public IPACharacterType BaseCharType;
		public int Order;
		public int Offset;
		public RelativePhoneLocation Location;
		public string MOArticulation;
		public string POArticulation;
		public string UDSortOrder;
		public ulong Mask0;
		public ulong Mask1;
		public ulong BinaryMask;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constucts a new PhoneInfo object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhoneInfo()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constucts a new PhoneInfo object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhoneInfo(string phone)
		{
			Phone = phone;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the Phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Phone;
		}
	}

	#endregion

	#region PhoneticWriter Class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Class for writing phonetic words (and all related info. including phones) information
	/// to the database.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PhoneticWriter
	{
		private const char kConsonant = 'C';
		private const char kVowel = 'V';
		private PaDataTable m_phoneticListTable;
		private PaDataTable m_charListTable;
		private PaDataTable m_charIndexTable;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new instance of a phonetic word writer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhoneticWriter()
		{
			m_phoneticListTable = new PaDataTable("PhoneticList");
			m_charListTable = new PaDataTable("CharList");
			m_charIndexTable = new PaDataTable("CharIndex");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Writes the specified phonetic word and it's phone information, to the database.
		/// If the specified phonetic word already exists in the phonetic list table then
		/// there's no need to add another copy of the word and the id of the found phonetic
		/// record is returned. If the phonetic word cannot be found in the phonetic list
		/// file then a record for it is added and the new record's id is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Write(string phoneticWord)
		{
			return Write(phoneticWord, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Writes the specified phonetic word and it's phone information, to the database.
		/// If the specified phonetic word already exists in the phonetic list table then
		/// there's no need to add another copy of the word and the id of the found phonetic
		/// record is returned. If the phonetic word cannot be found in the phonetic list
		/// file then a record for it is added and the new record's id is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal int Write(string phoneticWord, PhoneticWordInfo eticWordInfo)
		{
			if (phoneticWord == null || phoneticWord.Trim() == string.Empty)
				return 0;

			phoneticWord = phoneticWord.Trim();
			string hexPhoneticWord = DBUtils.StrToHex(phoneticWord);

			string sql = string.Format("{0}='{1}'",
				DBFields.HexFieldFromMainField[DBFields.Phonetic], hexPhoneticWord);

			// Check if the word already exists in the phonetic list table.
			DataRow[] rows = m_phoneticListTable.Select(sql);
			if (rows.Length > 0)
				return (int)rows[0][DBFields.PhoneticListId];

			// Parse the word into phones if necessary.
			if (eticWordInfo == null)
				eticWordInfo = ParseWord(phoneticWord);

			// Save all the phones for the word in the CharList table.
			CommitPhones(eticWordInfo);

			eticWordInfo.HexPhonetic = hexPhoneticWord;

			// Add the word to the phonetic list table.
			eticWordInfo.PhoneticListId = CommitWord(phoneticWord, eticWordInfo);
			if (eticWordInfo.PhoneticListId == 0)
				return 0;

			LinkWordsToPhones(eticWordInfo);

			if (!DBUtils.TransactionPending)
				DBUtils.UpdateTotalCounts(eticWordInfo.PhoneticListId);

			return eticWordInfo.PhoneticListId;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new records in the CharIndex table linking a phonetic word to each of it's
		/// phones in the CharList table.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LinkWordsToPhones(PhoneticWordInfo eticWordInfo)
		{
			foreach (PhoneInfo phoneInfo in eticWordInfo.Phones)
			{
				DataRow row = m_charIndexTable.NewRow();
				row[DBFields.PhoneticListId] = eticWordInfo.PhoneticListId;
				row[DBFields.CharListId] = phoneInfo.CharListId;
				row["CharOrder"] = phoneInfo.Order;
				row["CharOffset"] = phoneInfo.Offset;
				row["RelativeLocation"] = (int)phoneInfo.Location;
				m_charIndexTable.Rows.Add(row);
				m_charIndexTable.Commit();
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses a phonetic word and returns a collection of phones that make up the word.
		/// At this point, it is very important that the phoneticWord already be normalized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static PhoneticWordInfo ParseWord(string phoneticWord)
		{
			if (phoneticWord == null || phoneticWord.Trim() == string.Empty)
				return null;

			ArrayList phones = new ArrayList();
			StringBuilder singlePhone = new StringBuilder(2);
			StringBuilder cvPattern = new StringBuilder();
			StringBuilder baseHexPhonetic = new StringBuilder();
			StringBuilder moaSortKey = new StringBuilder();
			StringBuilder poaSortKey = new StringBuilder();

			RelativePhoneLocation location = RelativePhoneLocation.Initial;
			PhoneInfo phoneInfo = null;
			int offset = 0;
			int order = 0;

			// Loop through the characters (i.e. codepoints) that make up the word.
			foreach (char c in phoneticWord.ToCharArray())
			{
				IPACharInfo ipaCharInfo = DBUtils.IPACharCache[Convert.ToInt32(c)];
				if (ipaCharInfo == null)
				{
					// TODO: Generate some sort of logging report on invalid character found.
					offset++;
					continue;
				}

				// TODO: add more checks for things like tie bars etc.

				// Check if the current character starts a new phone or not.
				if (ipaCharInfo.IsBaseChar || phoneInfo == null)
				{
					// Check if we need to save the accumulated information for a
					// phone before starting the accumulation of a new phone.
					if (singlePhone.Length > 0)
					{
						phoneInfo.Phone = singlePhone.ToString();
						phoneInfo.POArticulation = poaSortKey.ToString();
						phoneInfo.MOArticulation = moaSortKey.ToString();
						phones.Add(phoneInfo);
						offset += singlePhone.Length;
						location = RelativePhoneLocation.Medial;
						singlePhone.Length = moaSortKey.Length = poaSortKey.Length = 0;
					}

					// Prepare to start accumulating information for a new phone.
					phoneInfo = new PhoneInfo();
//					phoneInfo.IPACharacterId = ipaCharInfo.IPACharacterId;
					phoneInfo.BaseCharType = ipaCharInfo.CharType;
					phoneInfo.Location = location;
					phoneInfo.Offset = offset;
					phoneInfo.Order = order++;

					// For the word, accumulate the base character codepoints as hex numbers.
					baseHexPhonetic.Append(ipaCharInfo.HexIPAChar);
					baseHexPhonetic.Append(" ");

					// For the word, accumulate the CV pattern.
					if (ipaCharInfo.CharType == IPACharacterType.Consonant)
						cvPattern.Append(kConsonant);
					else if (ipaCharInfo.CharType == IPACharacterType.Vowel)
						cvPattern.Append(kVowel);
				}

				singlePhone.Append(c);
				moaSortKey.Append(ipaCharInfo.MOArticulation.ToString("X3"));
				poaSortKey.Append(ipaCharInfo.POArticulation.ToString("X3"));
				phoneInfo.Mask0 |= ipaCharInfo.Mask0;
				phoneInfo.Mask1 |= ipaCharInfo.Mask1;
				phoneInfo.BinaryMask |= ipaCharInfo.BinaryMask;
			}

			// Make sure the last phone gets added.
			if (singlePhone.Length > 0)
			{
				// Make sure the relative location is set correctly for the last
				// phone in the word (or the only phone in the word in some cases).
				phoneInfo.Location = (phones.Count == 0 ?
					RelativePhoneLocation.Alone : RelativePhoneLocation.Final);

				phoneInfo.Phone = singlePhone.ToString();
				phoneInfo.POArticulation = poaSortKey.ToString();
				phoneInfo.MOArticulation = moaSortKey.ToString();
				phones.Add(phoneInfo);
			}

			// Now save all the information for the word.
			PhoneticWordInfo eticWordInfo = new PhoneticWordInfo();
			eticWordInfo.BaseHexPhonetic = baseHexPhonetic.ToString().Trim();
			eticWordInfo.CVPattern = cvPattern.ToString().Trim();
			eticWordInfo.Phones = (PhoneInfo[])phones.ToArray(typeof(PhoneInfo));

			return eticWordInfo;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method is used when the phone boundaries of a phonetic word are already known
		/// (i.e. when they come from a wave file document transcribed via SA). This method
		/// only cycles through the phones in a word and accumlate the MOA and POA sort
		/// keys for the phones as well as determining the CV pattern and hex representation
		/// of base characters for the word.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static void ProcessPhoneInfo(PhoneticWordInfo eticWordInfo)
		{
			if (eticWordInfo == null || eticWordInfo.Phones.Length == 0)
				return;

			StringBuilder cvPattern = new StringBuilder();
			StringBuilder baseHexPhonetic = new StringBuilder();
			StringBuilder moaSortKey = new StringBuilder();
			StringBuilder poaSortKey = new StringBuilder();

			// Loop through the phones in the word.
			foreach (PhoneInfo phoneInfo in eticWordInfo.Phones)
			{
				phoneInfo.Mask0 = phoneInfo.Mask1 = phoneInfo.BinaryMask = 0;
				moaSortKey.Length = poaSortKey.Length = 0;
				bool firstCodePoint = true;

				// Loop through the characters (i.e. codepoints) that make up the phone.
				foreach (char c in phoneInfo.Phone.ToCharArray())
				{
					IPACharInfo ipaCharInfo = DBUtils.IPACharCache[Convert.ToInt32(c)];
					if (ipaCharInfo == null)
					{
						// TODO: Generate some sort of logging report on invalid character found.
						continue;
					}

					moaSortKey.Append(ipaCharInfo.MOArticulation.ToString("X3"));
					poaSortKey.Append(ipaCharInfo.POArticulation.ToString("X3"));
					phoneInfo.Mask0 |= ipaCharInfo.Mask0;
					phoneInfo.Mask1 |= ipaCharInfo.Mask1;
					phoneInfo.BinaryMask |= ipaCharInfo.BinaryMask;

					// Treat the base character in the phone a little differently.
					if (firstCodePoint)
					{
						firstCodePoint = false;
						//phoneInfo.IPACharacterId = ipaCharInfo.IPACharacterId;
						phoneInfo.BaseCharType = ipaCharInfo.CharType;

						// For the word, accumulate the base character codepoints as hex numbers.
						baseHexPhonetic.Append(ipaCharInfo.HexIPAChar);
						baseHexPhonetic.Append(" ");

						// For the word, accumulate the CV pattern.
						if (ipaCharInfo.CharType == IPACharacterType.Consonant)
							cvPattern.Append(kConsonant);
						else if (ipaCharInfo.CharType == IPACharacterType.Vowel)
							cvPattern.Append(kVowel);
					}
				}

				// We've now finished accumulating the MOA and POA sort keys for a
				// single phone, so save them for the phone.
				phoneInfo.POArticulation = poaSortKey.ToString();
				phoneInfo.MOArticulation = moaSortKey.ToString();
			}

			// Now save all the information for the word.
			eticWordInfo.BaseHexPhonetic = baseHexPhonetic.ToString().Trim();
			eticWordInfo.CVPattern = cvPattern.ToString().Trim();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int CommitWord(string phoneticWord, PhoneticWordInfo eticWordInfo)
		{
			DataRow row = m_phoneticListTable.NewRow();
			row[DBFields.Phonetic] = phoneticWord;
			row[DBFields.HexFieldFromMainField[DBFields.Phonetic]] = eticWordInfo.HexPhonetic;
			row["BaseHexPhonetic"] = eticWordInfo.BaseHexPhonetic;
			row[DBFields.CVPattern] = eticWordInfo.CVPattern;
			row["MOASortKey"] = eticWordInfo.MOASortKey;
			row["POASortKey"] = eticWordInfo.POASortKey;
			m_phoneticListTable.Rows.Add(row);
			m_phoneticListTable.Commit();

			return m_phoneticListTable.NewRecordId;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Commits the phones to the CharList table.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CommitPhones(PhoneticWordInfo eticWordInfo)
		{
			string sqlFmt = "HexCharStr='{0}'";
            DataRow row;

			StringBuilder moaSortKey = new StringBuilder();
			StringBuilder poaSortKey = new StringBuilder();
			StringBuilder udSortKey = new StringBuilder();

			foreach (PhoneInfo phoneInfo in eticWordInfo.Phones)
			{
				int charListId = 0;
				
				// Look for the phone in the char list table.
				string hexCharStr = DBUtils.StrToHex(phoneInfo.Phone);
				DataRow[] rows = m_charListTable.Select(string.Format(sqlFmt, hexCharStr));

				// Was the phone found in the char. list table?
				if (rows.Length > 0)
				{
					// The phone is already in the CharList table so update sort keys
					// from the record that already exists.
					charListId = (int)rows[0][DBFields.CharListId];
					moaSortKey.Append(rows[0]["MOArticulation"] as string);
					poaSortKey.Append(rows[0]["POArticulation"] as string);
					udSortKey.Append(((int)rows[0]["UDSortOrder"]).ToString("X3"));
				}
				else
				{
					// The phone wasn't found in the char. list table so add a new record for it.
					row = m_charListTable.NewRow();
					row["CharStr"] = phoneInfo.Phone;
					row["HexCharStr"] = hexCharStr;
					row["IPACharacterID"] = phoneInfo.IPACharacterId;
					row["BaseCharType"] = (int)phoneInfo.BaseCharType;
					row["MOArticulation"] = phoneInfo.MOArticulation;
					row["POArticulation"] = phoneInfo.POArticulation;
					row["UDSortOrder"] = 0;
					row["Mask0"] = phoneInfo.Mask0;
					row["Mask1"] = phoneInfo.Mask1;
					row["BinaryMask"] = phoneInfo.BinaryMask;

					m_charListTable.Rows.Add(row);
					m_charListTable.Commit();
					charListId = m_charListTable.NewRecordId;

					moaSortKey.Append(phoneInfo.MOArticulation);
					poaSortKey.Append(phoneInfo.POArticulation);
					udSortKey.Append("000");
				}

				// Save this so we can write a record in the CharIndex table.
				phoneInfo.CharListId = charListId;
			}

			if (moaSortKey.Length > 0)
				eticWordInfo.MOASortKey = moaSortKey.ToString();
			
			if (poaSortKey.Length > 0)
				eticWordInfo.POASortKey = poaSortKey.ToString();
			
			if (udSortKey.Length > 0)
				eticWordInfo.UDSortKey = udSortKey.ToString();
		}
	}

	#endregion
}
