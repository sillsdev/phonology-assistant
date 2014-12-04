// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2014, SIL International. All Rights Reserved.
// <copyright from='2014' to='2014' company='SIL International'>
//		Copyright (c) 2014, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: Fw7CustomField.cs
// Responsibility: Greg Trihus
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using SIL.Pa.DataSource;
using SIL.Pa.DataSource.FieldWorks;

namespace SIL.Pa.DataSourceClasses.FieldWorks
{
    public class Fw7CustomField : IDisposable
    {
        public class CustomField
        {
            [XmlAttribute(AttributeName = "class")]
            public string Class;
            [XmlAttribute(AttributeName = "name")]
            public string Name;
            [XmlAttribute(AttributeName = "type")]
            public string Type;
            [XmlAttribute(AttributeName = "ws")]
            public string Ws;
        }

        public List<CustomField> CustomFields;

        public class Custom
        {
            [XmlAttribute(AttributeName = "name")]
            public string Name;
            [XmlAttribute(AttributeName = "value")]
            public string Value;
            [XmlAttribute(AttributeName = "ws")]
            public string Ws;
        }

// ReSharper disable once InconsistentNaming
        public class rt  //Name is the result of the XSLT
        {
            [XmlAttribute(AttributeName = "guid")]
            public string Guid;
            public List<Custom> CustomFields;
        }

        public List<rt> CustomValues;

        public struct WsDef
        {
            public List<string> AnalysisWss;
            public List<string> VernWss;
        }

        public WsDef WritingSystems;

// ReSharper disable once FieldCanBeMadeReadOnly.Local
        private bool _serializing; // used for implicit construction during serialization
        private readonly XslCompiledTransform _extractCustomFieldsXsl = new XslCompiledTransform();
        private readonly Dictionary<string, Custom> _index = new Dictionary<string, Custom>();

// ReSharper disable once UnusedMember.Local
        private Fw7CustomField()    // used for serialization 
        {
            _serializing = true;
        }

        public Fw7CustomField(PaDataSource ds)
        {
            if (ds == null || ds.FwDataSourceInfo == null || !File.Exists(ds.FwDataSourceInfo.Name))
            {
                CustomFields = new List<CustomField>();
                CustomValues = new List<rt>();
                return;
            }
            _extractCustomFieldsXsl.Load(XmlReader.Create(Path.Combine(new []{App.AssemblyPath, "DataSourceClasses", "FieldWorks", "ExtractCustomFields.xsl"})));
            Deserialize(ds.FwDataSourceInfo.Name);
            CreateIndex();
        }

        public void Dispose()
        {
            if (!_serializing)
            {
                CustomFields.Clear();
                foreach (rt entryStandoff in CustomValues)
                {
                    entryStandoff.CustomFields.Clear();
                }
                CustomValues.Clear();
                WritingSystems.AnalysisWss.Clear();
                WritingSystems.VernWss.Clear();
                _index.Clear();
            }
        }

        public void Deserialize(string filename)
        {
            var deserializer = new XmlSerializer(typeof(Fw7CustomField));

            using (TextReader reader = new StreamReader(filename))
            {
                try
                {
                    var sb = new StringBuilder();
                    var writer = XmlWriter.Create(sb, _extractCustomFieldsXsl.OutputSettings);
                    _extractCustomFieldsXsl.Transform(XmlReader.Create(reader), null, writer, null);
                    using (var result = ((Fw7CustomField)deserializer.Deserialize(new StringReader(sb.ToString()))))
                    {
                        CustomFields = result.CustomFields;
                        CustomValues = result.CustomValues;
                        WritingSystems = result.WritingSystems;
                    }
                }
                catch (Exception e)
                {
                    Debug.Print(e.Message);
                    throw;
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        private void CreateIndex()
        {
            foreach (rt entryStandoff in CustomValues)
            {
                foreach (Custom custom in entryStandoff.CustomFields)
                {
                    _index.Add(entryStandoff.Guid + custom.Name, custom);
                }
            }
        }

        public string Value(string fieldName, string guid)
        {
            return _index[guid + fieldName].Value;
        }

        public string Ws(string fieldName, string guid)
        {
            return _index[guid + fieldName].Ws;
        }

        public List<string> FieldNames()
        {
            return CustomFields.Select(customField => customField.Name).ToList();
        }

        public string FieldWs(string fieldName)
        {
            return (from customField in CustomFields where customField.Name == fieldName select customField.Ws).FirstOrDefault();
        }

        public FwDBUtils.FwWritingSystemType FwWritingSystemType(string fieldName)
        {
            if (WritingSystems.VernWss.Contains(FieldWs(fieldName)))
                return FwDBUtils.FwWritingSystemType.Vernacular;
            if (WritingSystems.AnalysisWss.Contains(FieldWs(fieldName)))
                return FwDBUtils.FwWritingSystemType.Analysis;
            return FwDBUtils.FwWritingSystemType.None;
        }
    }
}
