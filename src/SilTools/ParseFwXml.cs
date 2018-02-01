using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SilTools
{
	public class ParseFwXml : XmlParser
	{
		private bool _inCurVernWss;
		private bool _curVernWssUni;
		public string Code { get; private set; }

		public ParseFwXml(string xmlFullName) : base(xmlFullName)
		{
			Declare(XmlNodeType.Element, CheckElement);
			Declare(XmlNodeType.Text, CaptureText);
			Parse();
		}

		private void CheckElement(XmlReader r)
		{
			_curVernWssUni = (_inCurVernWss && r.Name == "Uni");
			_inCurVernWss = (r.Name == "CurVernWss");
		}

		private void CaptureText(XmlReader r)
		{
			if (!_curVernWssUni) return;
			Code = r.Value.Split(' ')[0];
			Finished();
		}
	}
}
