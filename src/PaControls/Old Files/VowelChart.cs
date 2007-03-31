using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;
using SIL.SpeechTools.Database;
using SIL.Pa.FFSearchEngine;

namespace SIL.Pa.Controls
{
	public partial class VowelChart : CharChartBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public VowelChart()
		{
			InitializeComponent();

			Initialize(IPACharacterType.Vowel);
			
			// Add some dots that aren't end points.
			linFront.DotPoints.Add(new Point(chr9.Right + 1, linCloseMid1.StartPoint.Y));
			linFront.DotPoints.Add(new Point(chr16.Right + 1, linOpenMid1.StartPoint.Y));
			linBack.DotPoints.Add(new Point(linBack.StartPoint.X, linCloseMid2.StartPoint.Y));
			linBack.DotPoints.Add(new Point(linBack.StartPoint.X, linOpenMid2.StartPoint.Y));
			linCentral.DotPoints.Add(new Point(linCentral.StartPoint.X, linCentral.StartPoint.Y));
			linCentral.DotPoints.Add(new Point(chr11.Right + 2, linCloseMid1.StartPoint.Y));
			linCentral.DotPoints.Add(new Point(chr18.Right + 2, linOpenMid1.StartPoint.Y));

			// Create a gap in the Central line to accomodate the schwa
			Point[] pts = new Point[] {
				new Point(chr15.Left + 7, chr15.Top),
				new Point(chr15.Left + 15, chr15.Bottom + 2)};

			linCentral.Gaps.Add(pts);

			// Create a gap in the Central line to accomodate the turned 'a'
			pts = new Point[] {
				new Point(chr23.Left + 7, chr23.Top),
				new Point(chr23.Left + 16, chr23.Bottom + 2)};

			linCentral.Gaps.Add(pts);
		}
	}
}
