using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using SIL.SpeechTools.Utils.Properties;

namespace SIL.SpeechTools.Utils
{
	public class MusicXML
	{
		#region Public Methods

		public MusicXML() : base()
		{
			//Create an XML declaration. 
			XmlDeclaration xmldecl;
			xmldecl = m_doc.CreateXmlDeclaration("1.0", "UTF-8", "no");
			m_doc.AppendChild(xmldecl);

			// Create a document type node.
			CreateMusicXMLDocType();

			//Create the root element and 
			//add it to the document.
			XmlElement root = m_doc.CreateElement("score-partwise");
			m_doc.AppendChild(root);

			CreateIdNode();

			CreatePartListNode();

			CreatePartNode();
		}

		public MusicXML(string samaString) : this()
		{
			FromSAMA_String(samaString);
		}

		public bool FromSAMA_String(string samaString)
		{
			if ((samaString == null) || (samaString == string.Empty))
				samaString = "(CLEF1)(VX041)(Q=120)";

			// Parse initial codes
			string samaCodes = samaString.Substring(0, 21);
			ParseInitialCodes(samaCodes);

			// Parse notes
			for (int offset = 21; offset < samaString.Length; offset += 7)
			{
				bool foundNote = true;
				string samaCode = samaString.Substring(offset, 7);
				string nextSamaNote = "A=3*qn ";
				int nextNoteOffset = offset + 7;
				if (nextNoteOffset < samaString.Length)
				{
					while ((samaString[nextNoteOffset] == '(') && (nextNoteOffset < samaString.Length))
					{
						nextNoteOffset += 7;
						if (nextNoteOffset >= samaString.Length)
						{
							foundNote = false;
							break;
						}
					}
					if (foundNote)
					{
						nextSamaNote = samaString.Substring(nextNoteOffset, 7);
					}
				}
				if (samaCode[0] == '(')
				{
					AddSymbol(samaCode);
				}
				else
				{
					AddNote(samaCode, nextSamaNote);
				}
			} // next offset
			AddLastBarLine();
			return true;
		}

		public string ToSAMA_String(ref bool bPerfect)
		{
			bool bLastMeasure = false;
			string samaString = "(CLEF1)(VX001)(Q=120)";
			int iLocation = 0;
			//
			// check for MusicXML

			
			// parse part-list element
			string xpath = "score-partwise/part-list/score-part/midi-instrument/midi-program";
			int vxNum;
			if (int.TryParse(m_doc.SelectSingleNode(xpath).InnerText, out vxNum))
			{
				vxNum--;
				string vxCode = "000" + vxNum.ToString();
				samaString = samaString.Remove(10, 3);
				vxCode = vxCode.Substring(vxCode.Length - 3);
				samaString = samaString.Insert(10, vxCode);
			}

			// parse part element
			xpath = "score-partwise/part";
			try
			{
				XmlNode nodePart = m_doc.SelectSingleNode(xpath);
				int divisions = 1;
				foreach (XmlNode nodeMeasure in nodePart.ChildNodes) 
				{
					string name = nodeMeasure.Name;
					bool bExplicitBarLineGiven = false;
					//
					bLastMeasure = (nodeMeasure == nodePart.LastChild);
					if (name == "measure")
					{
						m_currMeasure = int.Parse(nodeMeasure.Attributes[0].Value);
						foreach (XmlNode nodeMeasureItem in nodeMeasure.ChildNodes)
						{
							string itemName = nodeMeasureItem.Name;
							iLocation++;
							switch (itemName)
							{
								case "attributes":
									if (m_currMeasure == 1)
									{
										GetClef(nodeMeasureItem, ref samaString);
										divisions = GetDivisions(nodeMeasureItem);
									}
									break;
								case "direction":
									GetTempo(nodeMeasureItem, ref samaString);
									break;
								case "note":
									samaString += ProcessNoteNode(nodeMeasureItem);
									break;
								case "grouping":
									samaString += ProcessGroupingNode(nodeMeasureItem, iLocation);
									break;
								case "barline":
									bExplicitBarLineGiven = true;
									if (!bLastMeasure) // then
									{
										samaString += ProcessBarlineNode(nodeMeasureItem);
									} // end if
									break;
								default:
									// Here we ignore unhandled items.
									break;
							} // end switch
						} // end foreach
					} // end if
					if ((!bLastMeasure) && (!bExplicitBarLineGiven)) // then
					{
						samaString += "(-BAR-)";
					}
				} // end foreach
			}
			catch
			{
				bPerfect = false;
				return samaString;
			}
			bPerfect = true;
			return samaString;
		}

		public bool Save(string filePath)
		{
			m_filePath = filePath;
			CreateMusicXMLDocType();

			try
			{
				m_doc.Save(filePath);
			}
			catch
			{
				return false;
			}

			GetDTDSafeXML(filePath);
			ValidateXML();

			return true;
		}

		//public string asSAMA();

		public bool Load(string filePath)
		{
			m_filePath = filePath;

			GetDTDSafeXML(filePath);
			bool isValid = ValidateXML();

			try
			{
				m_doc.Load(new StringReader(m_dtdSafeXML));
			}
			catch
			{
				if (!isValid)
				{
					string path = STUtils.PrepFilePathForSTMsgBox(filePath);
					STUtils.STMsgBox(
						string.Format(Resources.kstidInvalidMusicXMLFile, path),
						MessageBoxButtons.OK);
				}
				return false;
			}

			return true;
		}

		#endregion

		#region Properties

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the score-partwise node.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public XmlElement ScorePartwiseNode
		{
			get
			{
				XmlElement scorePartwise = (XmlElement)m_doc.SelectSingleNode("score-partwise");
				return scorePartwise;
			}
			set
			{
				if (value == null) return;

				XmlElement scorePartwise = (XmlElement)m_doc.SelectSingleNode("score-partwise");
				XmlNode importedNode = m_doc.ImportNode(value, true);
				try
				{
					m_doc.ReplaceChild(importedNode, scorePartwise);
				}
				catch (Exception e)
				{
					MessageBox.Show(string.Format("{0}", e.Message));
				}
				//scorePartwise = value;
			}
		}

		public string EncodingDate
		{
			get
			{
				string nodePath = "score-partwise/identification/encoding/encoding-date";
				return m_doc.SelectSingleNode(nodePath).InnerText;
			}
			set
			{
				string nodePath = "score-partwise/identification/encoding/encoding-date";
				m_doc.SelectSingleNode(nodePath).InnerText = value;
			}
		}

		public string Encoder
		{
			get
			{
				string nodePath = "score-partwise/identification/encoding/encoder";
				return m_doc.SelectSingleNode(nodePath).InnerText;
			}
			set
			{
				string nodePath = "score-partwise/identification/encoding/encoder";
				m_doc.SelectSingleNode(nodePath).InnerText = value;
			}
		}

		public string Software
		{
			get
			{
				string nodePath = "score-partwise/identification/encoding/software";
				return m_doc.SelectSingleNode(nodePath).InnerText;
			}
			set
			{
				string nodePath = "score-partwise/identification/encoding/software";
				m_doc.SelectSingleNode(nodePath).InnerText = value;
			}
		}

		public string EncodingDescription
		{
			get
			{
				string nodePath = "score-partwise/identification/encoding/encoding-description";
				return m_doc.SelectSingleNode(nodePath).InnerText;
			}
			set
			{
				string nodePath = "score-partwise/identification/encoding/encoding-description";
				m_doc.SelectSingleNode(nodePath).InnerText = value;
			}
		}

		public string Source
		{
			get
			{
				string nodePath = "score-partwise/identification/source";
				return m_doc.SelectSingleNode(nodePath).InnerText;
			}
			set
			{
				string nodePath = "score-partwise/identification/source";
				m_doc.SelectSingleNode(nodePath).InnerText = value;
			}
		}

		public string PartName
		{
			get
			{
				string nodePath = "score-partwise/part-list/score-part/part-name";
				return m_doc.SelectSingleNode(nodePath).InnerText;
			}
			set
			{
				string nodePath = "score-partwise/part-list/score-part/part-name";
				m_doc.SelectSingleNode(nodePath).InnerText = value;
			}
		}

		//public string InstrumentName
		//{
		//    get
		//    {
		//        return partList.SelectSingleNode("score-part/instrument-name").InnerText;
		//    }
		//    set
		//    {
		//        partList.SelectSingleNode("score-part/instrument-name").InnerText = value;
		//    }
		//}

		#endregion

		#region Helper Functions

		private XmlDocumentType CreateMusicXMLDocType()
		{
			XmlDocumentType doctype = m_doc.DocumentType;
			XmlNode decl = m_doc.FirstChild;

			// Remove any existing document type node
			if (doctype != null)
				m_doc.RemoveChild(doctype);

			// Create a new document type node
			string inetDTDPath = "http://www.musicxml.org/dtds/partwise.dtd";

			try
			{
				doctype = m_doc.CreateDocumentType("score-partwise", "-//Recordare//DTD MusicXML 1.1 Partwise//EN", inetDTDPath, null);
			}
			catch
			{
				try // the second attempt usually succeeds for some reason
				{
					doctype = m_doc.CreateDocumentType("score-partwise", "-//Recordare//DTD MusicXML 1.1 Partwise//EN", inetDTDPath, null);
				}
				catch
				{
					STUtils.STMsgBox("Unable to create document type element.", MessageBoxButtons.OK);
				}
			}

			m_doc.InsertAfter(doctype, decl);

			return doctype;
		}

		private bool CreateIdNode()
		{
			XmlNode nodeIdentification = AddNode(m_doc.DocumentElement, "identification", "");
			if (nodeIdentification == null) { return false; }

			// Create child nodes of <identification>
			XmlNode nodeEncoding = AddNode(nodeIdentification, "encoding", "");

			// Create child nodes of <encoding>
			AddNode(nodeEncoding, "encoding-date", DateTime.Today.ToShortDateString());
			AddNode(nodeEncoding, "software", "Speech Analyzer");
			return true;
		}

		private bool CreatePartListNode()
		{
			XmlNode nodePartList = AddNode(m_doc.DocumentElement, "part-list", "");

			if (nodePartList == null) { return false; }

			// Create child nodes of <part-list>
			XmlNode nodeScorePart = AddNode(nodePartList, "score-part", "");
			AddAttribute(nodeScorePart, "id", "P1");

			// Create child nodes of <score-part>
			AddNode(nodeScorePart, "part-name", "");
			//currentNode.AppendChild(m_doc.CreateElement("instrument-name"));

			return true;
		}

		private bool CreatePartNode()
		{
			XmlNode nodePart = AddNode(m_doc.DocumentElement, "part", "");
			AddAttribute(nodePart, "id", "P1");

			if (nodePart == null) { return false; }
			AddMeasure();
			return true;
		}

		private XmlNode AddNode(XmlNode nodeParent, string element, string value)
		{
			XmlNode nodeChild = m_doc.CreateElement(element);
			nodeParent.AppendChild(nodeChild);
			if (value != "")
			{
				nodeChild.InnerText = value;
			}
			return nodeChild;
		}

		private XmlNode AddAttribute(XmlNode node, string attribute, string value)
		{
			XmlNode nodeAttr = m_doc.CreateNode(XmlNodeType.Attribute, attribute, null);
			nodeAttr.Value = value;
			node.Attributes.SetNamedItem(nodeAttr);
			return nodeAttr;
		}

		private string FindAlter(char samaAccidental)
		{
			string accidentals = "!@-=+#$";
			double alter = (accidentals.IndexOf(samaAccidental) - 3.0) / 2.0;

			return alter.ToString();
		}

		private string FindDuration(string samaDuration)
		{
			Dictionary<string, double> duration = new Dictionary<string, double>();
			duration.Add("w", 480.0);
			duration.Add("h", 240.0);
			duration.Add("q", 120.0);
			duration.Add("i", 60.0);
			duration.Add("s", 30.0);
			duration.Add("z", 15.0);

			Dictionary<string, double> modDuration = new Dictionary<string, double>();
			modDuration.Add("n", 1.0);
			modDuration.Add(".", 1.5);
			modDuration.Add("t", (2.0 / 3.0));
			modDuration.Add("v", 0.8);
			double dur = duration[samaDuration[0].ToString()];
			double modDur = modDuration[samaDuration[1].ToString()];
			string result = string.Format("{0:D}", (int)(dur * modDur));

			return result;
		}

		private string FindType(string samaDuration)
		{
			Dictionary<string, string> type = new Dictionary<string, string>();
			type.Add("w", "whole");
			type.Add("h", "half");
			type.Add("q", "quarter");
			type.Add("i", "eighth");
			type.Add("s", "16th");
			type.Add("z", "eighth");
			//
			string result = type[samaDuration];
			return result;
		}

		private string FindAccidental(string samaAccidental)
		{
			Dictionary<string, string> accidental = new Dictionary<string, string>();
			accidental.Add("$", "three-quarters-sharp");
			accidental.Add("#", "sharp");
			accidental.Add("+", "quarter-sharp");
			accidental.Add("=", "");
			accidental.Add("-", "quarter-flat");
			accidental.Add("@", "flat");
			accidental.Add("!", "three-quarters-flat");

			return accidental[samaAccidental];
		}

		private void AddMeasure()
		{
			string xpath = "score-partwise/part";
			XmlNode part = m_doc.SelectSingleNode(xpath);

			m_currMeasure++;
			XmlNode nodeMeasure = AddNode(part, "measure", "");
			AddAttribute(nodeMeasure, "number", m_currMeasure.ToString());
		}

		private void SetMeasureAttributes(int divisions, string clefSign, int clefLine, int clefOctave)
		{
			string xpath = string.Format("score-partwise/part[@id='P1']/measure[@number='{0:D}']", m_currMeasure);
			XmlNode nodeMeasure = m_doc.SelectSingleNode(xpath);

			XmlNode nodeAttributes = AddNode(nodeMeasure, "attributes", "");
			AddNode(nodeAttributes, "divisions", divisions.ToString());
			XmlNode nodeTime = AddNode(nodeAttributes, "time", "");
			AddNode(nodeTime, "senza-misura", "");
			XmlNode nodeClef = AddNode(nodeAttributes, "clef", "");
			AddNode(nodeClef, "sign", clefSign);
			AddNode(nodeClef, "line", clefLine.ToString());
			if (clefOctave != 0) 
			{
				XmlNode nodeOctave = AddNode(nodeClef, "clef-octave-change", clefOctave.ToString());
			}
		}

		private void SetTempo(int tempo)
		{
			string xpath = string.Format("score-partwise/part[@id='P1']/measure[@number='{0:D}']", m_currMeasure);
			XmlNode nodeMeasure = m_doc.SelectSingleNode(xpath);

			XmlNode nodeDirection = AddNode(nodeMeasure, "direction", "");
			XmlNode nodeDirectionType = AddNode(nodeDirection, "direction-type", "");
			AddNode(nodeDirectionType, "words", "");
			XmlNode nodeSound = AddNode(nodeDirection, "sound", "");
			AddAttribute(nodeSound, "tempo", tempo.ToString());
		}

		private void AddNote(string samaNote, string nextSamaNote)
		{
			string xpath = string.Format("score-partwise/part[@id='P1']/measure[@number='{0:D}']", m_currMeasure);
			XmlNode nodeMeasure = m_doc.SelectSingleNode(xpath);
			XmlNode nodeNote = AddNode(nodeMeasure, "note", "");
			if (samaNote[0] == 'R')
			{
				AddNode(nodeNote, "rest", "");
			}
			else
			{
				AddGraceElement(nodeNote, samaNote);
				AddPitchElement(nodeNote, samaNote);
			}
			AddDurationElement(nodeNote, samaNote);
			AddTieElement(nodeNote, samaNote, nextSamaNote);
			AddNode(nodeNote, "type", FindType(samaNote.Substring(4, 1)));
			AddDotElement(nodeNote, samaNote);
			AddAccidentalElement(nodeNote, samaNote);
			AddTimeModificationElement(nodeNote, samaNote);
			AddStemElement(nodeNote, samaNote);
			AddNotationsElement(nodeNote, samaNote, nextSamaNote);
		}

		private void AddGraceElement(XmlNode nodeNote, string samaNote)
		{
			if (samaNote[4] == 'z')
			{
				XmlNode nodeGrace = AddNode(nodeNote, "grace", "");
				AddAttribute(nodeGrace, "slash", "yes");
			}
		}

		private void AddPitchElement(XmlNode nodeNote, string samaNote)
		{
			XmlNode nodePitch = AddNode(nodeNote, "pitch", "");
			AddNode(nodePitch, "step", samaNote[0].ToString());
			if (samaNote[1] != '=')
			{
				AddNode(nodePitch, "alter", FindAlter(samaNote[1]));
			}
			AddNode(nodePitch, "octave", samaNote[2].ToString());
		}

		private void AddDurationElement(XmlNode nodeNote, string samaNote)
		{
			if (samaNote[4] != 'z')
			{
				AddNode(nodeNote, "duration", FindDuration(samaNote.Substring(4, 2)));
			}
		}

		private void AddTieElement(XmlNode nodeNote, string samaNote, string nextSamaNote)
		{
			int nextTieState = CheckNextTieState(samaNote, nextSamaNote);

			if ((m_tieState == kTie)) // tie stop
			{
				XmlNode nodeTie = AddNode(nodeNote, "tie", "");
				AddAttribute(nodeTie, "type", "stop");
			}
			if ((nextTieState == kTie)) // tie start
			{
				XmlNode nodeTie = AddNode(nodeNote, "tie", "");
				AddAttribute(nodeTie, "type", "start");
			}
		}

		private void AddTimeModificationElement(XmlNode nodeNote, string samaNote)
		{
			Dictionary<string, int> mod = new Dictionary<string, int>();
			mod.Add("t", 3);
			mod.Add("v", 5);
			if (mod.ContainsKey(samaNote[5].ToString()))
			{
				int actualNotes = mod[samaNote[5].ToString()];
				int normalNotes = mod[samaNote[5].ToString()] - 1;

				XmlNode nodeTimeMod = AddNode(nodeNote, "time-modification", "");
				AddNode(nodeTimeMod, "actual-notes", actualNotes.ToString());
				AddNode(nodeTimeMod, "normal-notes", normalNotes.ToString());
			}
		}

		private void AddStemElement(XmlNode nodeNote, string samaNote)
		{
			if (samaNote[4] == 'z')
			{
				AddNode(nodeNote, "stem", "up");
			}
		}

		private void AddDotElement(XmlNode nodeNote, string samaNote)
		{
			if (samaNote[5] == '.')
			{
				AddNode(nodeNote, "dot", "");
			}
		}

		private void AddAccidentalElement(XmlNode nodeNote, string samaNote)
		{
			if (samaNote[1] != '=')
			{
				AddNode(nodeNote, "accidental", FindAccidental(samaNote[1].ToString()));
			}
		}

		private void AddNotationsElement(XmlNode nodeNote, string samaNote, string nextSamaNote)
		{
			int newTieState = CheckNextTieState(samaNote, nextSamaNote);
			bool needTieNotation = (m_tieState != newTieState) || (m_tieState == kTie);
			bool needTupletNotation = (samaNote[5] == 't') || (samaNote[5] == 'v');
			bool glissStarting = (((samaNote[6] != ' ') && (samaNote[6] != '_')) || (nextSamaNote[3] != '*'));
			bool needGlissNotation = (m_glissStarted || glissStarting);

			if (!needTieNotation && !needTupletNotation && !needGlissNotation)
			{
				// notations node is not needed
				m_tieState = newTieState;
				return;
			}
			XmlNode nodeNotations = AddNode(nodeNote, "notations", "");
			if (m_tieState == kTie) // tie stop
			{
				XmlNode nodeTied = AddNode(nodeNotations, "tied", "");
				AddAttribute(nodeTied, "type", "stop");
			}
			if ((m_tieState == kSlur) && (newTieState != kSlur)) // slur stop
			{
				XmlNode nodeSlur = AddNode(nodeNotations, "slur", "");
				AddAttribute(nodeSlur, "type", "stop");
			}
			if (newTieState == kTie) // tie start
			{
				XmlNode nodeTied = AddNode(nodeNotations, "tied", "");
				AddAttribute(nodeTied, "type", "start");
			}
			if ((m_tieState != kSlur) && (newTieState == kSlur)) // slur start
			{
				XmlNode nodeSlur = AddNode(nodeNotations, "slur", "");
				AddAttribute(nodeSlur, "type", "start");
			}
			m_tieState = newTieState;

			if (needTupletNotation) // tuplet
			{
				XmlNode nodeTupletStart = AddNode(nodeNotations, "tuplet", "");
				AddAttribute(nodeTupletStart, "type", "start");
				XmlNode nodeTupletStop = AddNode(nodeNotations, "tuplet", "");
				AddAttribute(nodeTupletStop, "type", "stop");
			}

			if (m_glissStarted)
			{
				XmlNode nodeGliss = AddNode(nodeNotations, "glissando", "");
				AddAttribute(nodeGliss, "type", "stop");
				m_glissStarted = false;
			}

			if (((samaNote[6] != ' ') && (samaNote[6] != '_')) || (nextSamaNote[3] != '*'))
			{
				XmlNode nodeGliss = AddNode(nodeNotations, "glissando", "");
				AddAttribute(nodeGliss, "type", "start");
				m_glissStarted = true;
			}
		}

		private int CheckNextTieState(string samaNote, string nextSamaNote)
		{
			string lookUp1 = samaNote[6].ToString();
			if (lookUp1 != "_")
			{
				lookUp1 = " ";
			}
			string lookUp2 = "!";
			if (samaNote.Substring(0, 3) == nextSamaNote.Substring(0, 3))
			{
				lookUp2 = "=";
			}
			string lookUp = lookUp1 + lookUp2;
			Dictionary<string, int> a = new Dictionary<string, int>();
			a.Add("_=", kTie);
			a.Add("_!", kSlur);
			a.Add(" =", kNormal);
			a.Add(" !", kNormal);

			return a[lookUp];
		}

		private void AddSymbol(string samaCode)
		{
			string xpath = string.Format("score-partwise/part[@id='P1']/measure[@number='{0:D}']", m_currMeasure);
			XmlNode nodeMeasure = m_doc.SelectSingleNode(xpath);

			if (samaCode.Contains("BAR"))
			{
				if (samaCode[1] == '=')
				{
					XmlNode nodeBarline = AddNode(nodeMeasure, "barline", "");
					AddNode(nodeBarline, "bar-style", "heavy");
				}
				AddMeasure();
			}

			if (samaCode.Contains("PHR"))
			{
				const char sq = '\'';
				const char dq = '"';
				Dictionary<char, int[]> InputFSM_Dictionary_phrase = new Dictionary<char, int[]>();
				//                      
				// Here I create a Finite State Machine (FSM).
				//                                     State: { 1, 2, 3 }
				InputFSM_Dictionary_phrase.Add(sq, new int[3] { 2, 1, 1 });
				InputFSM_Dictionary_phrase.Add(dq, new int[3] { 3, 3, 2 });   
				//
				int oldPhraseState;
				string transition;

				char letter = samaCode[1];
				oldPhraseState = m_phraseState;
				m_phraseState = InputFSM_Dictionary_phrase[letter][m_phraseState - 1]; // zero-based
				transition = oldPhraseState.ToString() + m_phraseState.ToString();
				switch (transition)
				{
					case "12":
						AddPhraseElement("start","phrase", "0");
						break;
					case "13":
						AddPhraseElement("start", "phrase", "-10");
						AddPhraseElement("start", "subphrase", "10");
						break;
					case "21":
						AddPhraseElement("stop", "phrase", "0");
						break;
					case "23":
						AddPhraseElement("start", "subphrase", "0");
						break;
					case "31":
						AddPhraseElement("stop", "subphrase", "-15");
						AddPhraseElement("stop", "phrase", "20");
						break;
					case "32":
						AddPhraseElement("stop", "subphrase", "0");
						break;
					default:
// 2
						MessageBox.Show("Error in FSM.");
						break;
				} // end switch
			}
		}

		private void AddPhraseElement(string type, string feature, string offset)
		{
			string xpath = string.Format("score-partwise/part[@id='P1']/measure[@number='{0:D}']", m_currMeasure);
			XmlNode nodeMeasure = m_doc.SelectSingleNode(xpath);

			XmlNode nodeGrouping = AddNode(nodeMeasure, "grouping", "");
			AddAttribute(nodeGrouping, "type", type);
			AddNode(nodeGrouping, "feature", feature);

			Dictionary<string, string> phraseTypeDict = new Dictionary<string, string>();
			phraseTypeDict.Add("phrase", "'");
			phraseTypeDict.Add("subphrase", "\"");

			XmlNode nodeDirection = AddNode(nodeMeasure, "direction", "");
			AddAttribute(nodeDirection, "placement", "above");
			XmlNode nodeDirectionType = AddNode(nodeDirection, "direction-type", "");
			XmlNode nodeWords = AddNode(nodeDirectionType, "words", phraseTypeDict[feature]);
			AddAttribute(nodeWords, "font-size", "20");
			AddAttribute(nodeWords, "relative-x", offset);
		}

		private void ParseInitialCodes(string samaCodes)
		{
			int instrument = int.Parse(samaCodes.Substring(10, 3));
			instrument++; // SAMA MIDI numbers differ by one from standard
			SetMidiInstrument(instrument);
			int clef = int.Parse(samaCodes[5].ToString());
			string[] clefSign = { "G", "G", "G", "F", "F" };
			int[] clefLine = { 2, 2, 2, 4, 4 };
			int[] clefOctave = { 1, 0, -1, 0, -1 };
			SetMeasureAttributes(120, clefSign[clef], clefLine[clef], clefOctave[clef]);
			int tempo = int.Parse(samaCodes.Substring(17, 3));
			SetTempo(tempo);
		}

		private void SetMidiInstrument(int instrument)
		{
			string xpath = "score-partwise/part-list/score-part[@id='P1']";
			XmlNode nodeScorePart = m_doc.SelectSingleNode(xpath);

			XmlNode nodeScoreInstrument = AddNode(nodeScorePart, "score-instrument", "");
			AddAttribute(nodeScoreInstrument, "id", "P1-I1");
			AddNode(nodeScoreInstrument, "instrument-name", "");
			XmlNode nodeMidiInstrument = AddNode(nodeScorePart, "midi-instrument", "");
			AddAttribute(nodeMidiInstrument, "id", "P1-I1");
			AddNode(nodeMidiInstrument, "midi-channel", "1");
			AddNode(nodeMidiInstrument, "midi-program", instrument.ToString());
		}

		private bool ValidateXML()
		{
			// Set the validation settings.
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.ProhibitDtd = false;
			settings.ValidationType = ValidationType.DTD;
			settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

			// Create the XmlReader
			XmlReader xr = XmlReader.Create(new StringReader(m_dtdSafeXML), settings);

			// Parse the file.
			try
			{
				while (xr.Read()) ;
			}
			catch
			{
				return false;
			}

			if (m_validationErrors != string.Empty)
			{
				string filePath = STUtils.PrepFilePathForSTMsgBox(m_filePath);
				string msg = string.Format(Resources.kstidMusicXMLValidationErrMsg,
					filePath, m_validationErrors);
				
				STUtils.STMsgBox(msg, MessageBoxButtons.OK);
				m_validationErrors = string.Empty;
				return false;
			}
			else
				return true;
		}

		// Display any validation errors.
		private static void ValidationCallBack(object sender, ValidationEventArgs e)
		{
			if (m_validationErrors != string.Empty)
				m_validationErrors += "\n\n";
			string msg;
			string senderClass = sender.ToString();
			if (senderClass.Contains("XmlValidatingReaderImpl"))
			{
				msg = "<{0}> element, line {1}: {2}";
				XmlReader reader = (XmlReader)sender;
				string name = reader.Name;
				int line = e.Exception.LineNumber;
				m_validationErrors += string.Format(msg, name, line, e.Message);
			}
			else
				m_validationErrors += e.Message;
		}

		private void GetDTDSafeXML(string path)
		{
			// Get the document type element as a string
			string xml = File.ReadAllText(path);
			int start = xml.IndexOf("<!DOCTYPE");
			int length;
			string name = string.Empty;
			for (length = 1; xml[start + length] != '>'; length++)
			{
				if ((name == string.Empty) && (xml[start + length] == ' ') && (length > 9))
					name = xml.Substring(start + 10, length - 10);
			}
			length++; // need to include the > at the end

			// Check the document type name
			if (name != "score-partwise")
			{
				STUtils.STMsgBox("XML document type is not recognized.", MessageBoxButtons.OK);
				return;
			}

			string doctype = xml.Substring(start, length);

			// Insert a "safe" document type element.
			// (This avoids problems with internet access and proxies)
			string systemIdNew = Assembly.GetExecutingAssembly().Location;
			systemIdNew = Directory.GetParent(systemIdNew) + "\\dtds\\partwise.dtd";
			string doctypeNew = "<!DOCTYPE score-partwise PUBLIC \"-//Recordare//DTD MusicXML 1.1 Partwise//EN\" \"";
			doctypeNew += systemIdNew + "\">";

			m_dtdSafeXML = xml.Replace(doctype, doctypeNew);
		}

		private void AddLastBarLine()
		{
			string xpath = string.Format("score-partwise/part[@id='P1']/measure[@number='{0:D}']", m_currMeasure);
			XmlNode nodeMeasure = m_doc.SelectSingleNode(xpath);

			XmlNode nodeBarline = AddNode(nodeMeasure, "barline", "");
			AddNode(nodeBarline, "bar-style", "light-heavy");
		}

		private void GetClef(XmlNode nodeMeasureItem, ref string samaString)
		{
			if (!m_haveClef)
			{
				string sign = nodeMeasureItem.SelectSingleNode("clef/sign").InnerText;
				string line = nodeMeasureItem.SelectSingleNode("clef/line").InnerText;
				string change = "0";
				try
				{
					change = nodeMeasureItem.SelectSingleNode("clef/clef-octave-change").InnerText;
				}
				catch
				{
					change = "0";
				}
				Dictionary<string, string> clefLookup = new Dictionary<string, string>();
				clefLookup.Add("G,2,1", "0");
				clefLookup.Add("G,2,0", "1");
				clefLookup.Add("G,2,-1", "2");
				clefLookup.Add("F,4,0", "3");
				clefLookup.Add("F,4,-1", "4");

				string clef = sign + "," + line + "," + change;

				samaString = samaString.Remove(5, 1);
				samaString = samaString.Insert(5, clefLookup[clef]);
				m_haveClef = true;
			}
		}

		private int GetDivisions(XmlNode nodeMeasureItem)
		{
			string divisions = "";

			try
			{
				divisions = nodeMeasureItem.SelectSingleNode("divisions").InnerText;
			}
			catch
			{
				divisions = "1";
			}

			return int.Parse(divisions);
		}

		private void GetTempo(XmlNode nodeMeasureItem, ref string samaString)
		{
			if (!m_haveTempo)
			{
				XmlNode nodeSound = nodeMeasureItem.SelectSingleNode("sound");
				string tempo = "000" + nodeSound.Attributes["tempo"].Value;
				samaString = samaString.Remove(17, 3);
				tempo = tempo.Substring(tempo.Length - 3);
				samaString = samaString.Insert(17, tempo);
				m_haveTempo = true;
			}
		}

		private string ProcessNoteNode(XmlNode nodeMeasureItem)
		{
			string samaCode = "C=4*qn ";
			bool isGraceNote = false;
			//
			foreach (XmlNode childNode in nodeMeasureItem.ChildNodes)
			{
				switch (childNode.Name)
				{
					case "dot":
						samaCode = samaCode.Substring(0, 5) + "." + samaCode.Substring(6, 1);
						break;
					case "duration":
						// We are not handeling length at this time.
						break;
					case "grace":
						isGraceNote = true;
						samaCode = samaCode.Substring(0, 4) + "z" + samaCode.Substring(5, 2);
						break;
					case "notations":
						{
							ProcessNotationsNode_Export2SAMA(childNode);
						}
						break;
					case "pitch":
						samaCode = samaCode.Remove(0, 3);
						samaCode = samaCode.Insert(0, PitchToSAMA(childNode));
						break;
					case "rest":
						samaCode = "R" + samaCode.Substring(1, 6);
						break;
					case "stem":
					case "tie":
						break;
					case "accidental":
						{
							samaCode = samaCode.Remove(1, 1);
							samaCode = samaCode.Insert(1, ProcessAccidentalNode_Export2SAMA(childNode));
						}
						break;
					case "time-modification":
						{
							string c = childNode.SelectSingleNode("actual-notes").InnerText;
							if (c == "3")
							{
								samaCode = samaCode.Substring(0, 5) + "t" + samaCode.Substring(6, 1);
							}
							else if (c == "5")
							{
								samaCode = samaCode.Substring(0, 5) + "v" + samaCode.Substring(6, 1);
							}
						}
						break;
					case "type":
						if (!isGraceNote) // then
						{
							string duration = ProcessType_Export2SAMA(childNode);
							//
							samaCode = samaCode.Remove(4, 1);
							samaCode = samaCode.Insert(4, duration);
						}
						break;
					default:
						// Here we ignore unhandled items.
						break;
				} // end switch
			}

			// add tie if needed
			if (m_tieLevel >= 1) // then
			{
				samaCode = samaCode.Substring(0, samaCode.Length - 1) + "_";
			}

			return samaCode;
		}

		private string ProcessAccidentalNode_Export2SAMA(XmlNode childNode)
		{
			Dictionary<string, string> accidentalLookup = new Dictionary<string, string>();
			accidentalLookup.Add("three-quarters-sharp", "$");
			accidentalLookup.Add("sharp", "#");
			accidentalLookup.Add("quarter-sharp", "+");
			accidentalLookup.Add("natural", "=");
			accidentalLookup.Add("", "=");
			accidentalLookup.Add("quarter-flat", "-");
			accidentalLookup.Add("flat", "@");
			accidentalLookup.Add("three-quarters-flat", "!");
			//
			return accidentalLookup[childNode.InnerText];
		}

		private string ProcessType_Export2SAMA(XmlNode childNode)
		{
			Dictionary<string, string> typeLookup = new Dictionary<string, string>();
			typeLookup.Add("whole", "w");
			typeLookup.Add("half", "h");
			typeLookup.Add("quarter", "q");
			typeLookup.Add("eighth", "i");
			typeLookup.Add("16th", "s");
			//
			return typeLookup[childNode.InnerText];
		}

		private void ProcessNotationsNode_Export2SAMA(XmlNode childNode)
		{
			foreach (XmlNode grandChildNode in childNode.ChildNodes)
			{
				switch (grandChildNode.Name)
				{
					case "tied":
					case "slur":
						if ((grandChildNode.Attributes["type"].Value) == "start")
						{
							m_tieLevel++;
						}
						else
						{
							m_tieLevel--;
						}
						break;
					default:
						// Here we ignore unhandled items.
						break;
				} // end switch
			} // foreach 
		} // ProcessNotationsNode_Export2SAMA

		private string PitchToSAMA(XmlNode childNode)
		{
			Dictionary<double, string> alterLookup = new Dictionary<double, string>();
			alterLookup.Add(1.5, "$");
			alterLookup.Add(1.0, "#");
			alterLookup.Add(0.5, "+");
			alterLookup.Add(0.0, "=");
			alterLookup.Add(-0.5, "-");
			alterLookup.Add(-1.0, "@");
			alterLookup.Add(-1.5, "!");
			string samaCodes = childNode.SelectSingleNode("step").InnerText;
			try
			{
				string alter = childNode.SelectSingleNode("alter").InnerText;
				samaCodes += alterLookup[int.Parse(alter)];
			}
			catch
			{
				samaCodes += "=";
			}
			int octave = int.Parse(childNode.SelectSingleNode("octave").InnerText);
			samaCodes += octave.ToString();
			return samaCodes;
		}

		private string ProcessGroupingNode(XmlNode nodeMeasureItem, int iLocation)
		{
			string outputString = "";
			string test = nodeMeasureItem.Attributes[0].Value + " " + nodeMeasureItem.SelectSingleNode("feature").InnerText;
			//
			Dictionary<string, int[]> InputFSM_Dictionary_phrase = new Dictionary<string, int[]>();
			//                      
			// Here I create a Finite State Machine (FSM).
			//                                                    State: { 1, 2, 3, 4 }
			InputFSM_Dictionary_phrase.Add("start phrase", new int[4] { 0002, 2, 4, 4 });
			InputFSM_Dictionary_phrase.Add("stop phrase", new int[4] { 00004, 1, 1, 4 });
			InputFSM_Dictionary_phrase.Add("start subphrase", new int[4] { 3, 3, 4, 4 });
			InputFSM_Dictionary_phrase.Add("stop subphrase", new int[4] { 04, 4, 2, 4 });
			//
			int oldPhraseState;
			string transition;
			//
			oldPhraseState = m_phraseState;
			m_phraseState = InputFSM_Dictionary_phrase[test][m_phraseState - 1]; // zero-based
			transition = oldPhraseState.ToString() + m_phraseState.ToString();
			switch (transition)
			{
				case "12":
				case "21":
					outputString = "('PHR')";
					break;
				case "13":
					outputString = "('PHR')(\"PHR\")";
					break;
				case "23":
				case "32":
					outputString = "(\"PHR\")";
					break;
				case "31":
					outputString = "(\"PHR\")('PHR')";
					break;
				default:
					{
						string sMsg = "";
						//
						sMsg = "Error in FSM at location {0}." + iLocation.ToString();
						// 1
						MessageBox.Show(sMsg);
					}
					break;
			} // end switch
			return outputString;
		}

		private string ProcessBarlineNode(XmlNode nodeMeasureItem)
		{
			string sameCodes = "";
			string style = nodeMeasureItem.SelectSingleNode("bar-style").InnerText;
			switch (style)
			{
				case "light":
					sameCodes = "(-BAR-)";
					break;
				case "heavy":
					sameCodes = "(=BAR=)";
					break;
				case "light-light":
					sameCodes = "(-BAR-)(SPACE)(-BAR-)";
					break;
				case "light-heavy":
					sameCodes = "(-BAR-)(SPACE)(=BAR=)";
					break;
				case "heavy-heavy":
					sameCodes = "(=BAR=)(SPACE)(=BAR=)";
					break;
				case "heavy-light":
					sameCodes = "(=BAR=)(SPACE)(-BAR-)";
					break;
			}
			return sameCodes;
		}

		#endregion

		#region Members

		private XmlDocument m_doc = new XmlDocument();
		private string m_filePath = string.Empty;
		private string m_dtdSafeXML = string.Empty;
		private int m_currMeasure = 0;
		private int m_tieState = 1;
		private const int kNormal = 1;
		private const int kTie = 2;
		private const int kSlur = 3;
		private const int kNone = 1;
		private const int kPhrase = 2;
		private const int kSubphrase = 3;
		private int m_phraseState = 1;
		private bool m_glissStarted = false;
		private bool m_haveClef = false;
		private bool m_haveTempo = false;
		private int m_tieLevel = 0;
		private static string m_validationErrors = string.Empty;
		//private string errorLog = "";

		#endregion
	}
}
