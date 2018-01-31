// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2016, SIL International. All Rights Reserved.
// <copyright from='2016' to='2016' company='SIL International'>
//		Copyright (c) 2016, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: XmlParser.cs (from SimpleCss5.cs)
// Responsibility: Greg Trihus
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace SilTools
{
    public abstract class XmlParser
    {
        protected delegate void ParserMethod(XmlReader r);
        private readonly Dictionary<XmlNodeType, List<ParserMethod>> _nodeTypeMap = new Dictionary<XmlNodeType, List<ParserMethod>>();
        private readonly XmlReader _rdr;
        private readonly StreamReader _srdr;
        private bool _finish;
        private bool _doAttributes;

        protected XmlParser(string xmlFullName)
        {
            var settings = new XmlReaderSettings(){DtdProcessing = DtdProcessing.Ignore, XmlResolver = new NullResolver()};
            _srdr = new StreamReader(xmlFullName);
            _rdr = XmlReader.Create(_srdr, settings);
        }

        protected void Close()
        {
            _rdr.Close();
            _srdr.Close();
        }

        protected void Parse()
        {
            while (_rdr.Read())
            {
                if (_nodeTypeMap.ContainsKey(_rdr.NodeType))
                {
                    ProcessMethods();
                }
                if (_doAttributes && _rdr.HasAttributes)
                {
                    var attrCount = _rdr.AttributeCount;
                    for (int attrIndex = 0; attrIndex < attrCount; attrIndex++)
                    {
                        _rdr.MoveToNextAttribute();
                        ProcessMethods();
                    }
                }
                if (_finish)
                    break;
            }
        }

        private void ProcessMethods()
        {
            foreach (ParserMethod func in _nodeTypeMap[_rdr.NodeType])
            {
                Debug.Assert(func != null, "func != null");
                func(_rdr);
                if (_finish)
                    break;
            }
        }

        protected void Finished()
        {
            _finish = true;
        }

        protected void Declare(XmlNodeType nodeType, ParserMethod parserMethod)
        {
            if (_nodeTypeMap.ContainsKey(nodeType))
            {
                _nodeTypeMap[nodeType].Add(parserMethod);
            }
            else
            {
                _nodeTypeMap[nodeType] = new List<ParserMethod>(){parserMethod};
            }
            if (nodeType == XmlNodeType.Attribute)
            {
                _doAttributes = true;
            }
        }
    }
}
