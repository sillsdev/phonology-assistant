using System;
using System.Drawing;
using System.Windows.Forms;

namespace SIL.Pa
{
	/// <summary>
	/// Summary description for DocWords.
	/// </summary>
	public class DocWords
	{
		private Label [] lblCells;
		private int m_width;
		private int m_block;
		private bool m_clickAssigned;

		public DocWords()
		{
			lblCells = new Label[7];
			for (int i = 0; i < lblCells.Length; i++)
			{
				Label lbl = new Label();
				lbl.AutoSize = true;
				lbl.BackColor = Color.FromKnownColor(KnownColor.Transparent);
				lbl.Cursor = Cursors.IBeam;
				lblCells[i] = lbl;
			}
			lblCells[0].Font = PaApp.PhoneticFont;
			lblCells[1].Font = PaApp.ToneFont;
			lblCells[2].Font = PaApp.PhonemicFont;
			lblCells[3].Font = PaApp.OrthograpicFont;
			lblCells[4].Font = PaApp.GlossFont;
			lblCells[5].Font = PaApp.PartOfSpeechFont;
			lblCells[6].Font = PaApp.ReferenceFont;
			BlockNum = 0;
			Phonetic = "Å";
			Tone = "Å";
			Phonemic = "Å";
			Ortho = ".";
			Gloss = "";
			POS = "";
			Ref = "";
		}

		public void UpdateFonts()
		{
			lblCells[0].Font = PaApp.PhoneticFont;
			lblCells[1].Font = PaApp.ToneFont;
			lblCells[2].Font = PaApp.PhonemicFont;
			lblCells[3].Font = PaApp.OrthograpicFont;
			lblCells[4].Font = PaApp.GlossFont;
			lblCells[5].Font = PaApp.PartOfSpeechFont;
			lblCells[6].Font = PaApp.ReferenceFont;
		}

		// previously used to re-create the labels without click event
		// handlers because i didn't know how to remove that, but now
		// i know, so it's not needed any more.
		//	public void RecreateLabels()
		//	{
		//		for (int i = 0; i < 7; i++)
		//		{
		//			Label lbl = new Label();
		//			lbl.AutoSize = true;
		//			lbl.BackColor = Color.FromKnownColor(KnownColor.Transparent);
		//			lbl.Cursor = Cursors.IBeam;
		//			lbl.Font = lblCells[i].Font;
		//			lbl.Text = lblCells[i].Text;
		//			lblCells[i].Dispose();
		//			lblCells[i] = lbl;
		//		}
		//		m_clickAssigned = false;
		//	}

		public bool IsEmpty()
		{
			if (Phonetic != "Å")
				return false;
			if (Tone != "Å")
				return false;
			if (Phonemic != "Å")
				return false;
			if (Ortho != ".")
				return false;
			if (Gloss != "")
				return false;
			if (POS != "")
				return false;
			if (Ref != "")
				return false;
			return true;
		}

		#region Properties
		public string Phonetic
		{
			set { lblCells[0].Text = value; }
			get { return lblCells[0].Text; }
		}

		public string Tone
		{
			set { lblCells[1].Text = value; }
			get { return lblCells[1].Text; }
		}

		public string Phonemic
		{
			set { lblCells[2].Text = value; }
			get { return lblCells[2].Text; }
		}

		public string Ortho
		{
			set { lblCells[3].Text = value; }
			get { return lblCells[3].Text; }
		}

		public string Gloss
		{
			set { lblCells[4].Text = value; }
			get { return lblCells[4].Text; }
		}

		public string POS
		{
			set { lblCells[5].Text = value; }
			get { return lblCells[5].Text; }
		}

		public string Ref
		{
			set { lblCells[6].Text = value; }
			get { return lblCells[6].Text; }
		}

		public Label[] Cells
		{
			get { return lblCells; }
		}

		public int ColWidth
		{
			set { m_width = value; }
			get { return m_width; }
		}

		public int BlockNum
		{
			set { m_block = value; }
			get { return m_block; }
		}

		public bool ClickAssigned
		{
			set { m_clickAssigned = value; }
			get { return m_clickAssigned; }
		}
		#endregion
	}
}
