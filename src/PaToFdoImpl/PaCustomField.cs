using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIL.PaToFdoInterfaces;

namespace SIL.FieldWorks.PaObjects
{
    public class PaCustomField : IPaCustomField
    {
        private string className;
        private string fieldName;
        private string fieldType;
        private string[] writingSystems;

        internal static List<PaCustomField> GetCustomFields(dynamic fdoCache)
        {
            List<PaCustomField> fields = new List<PaCustomField>();
            dynamic metaDataCache = SilTools.ReflectionHelper.GetProperty(fdoCache, "MetaDataCacheAccessor");
            Type writingSystemServices = ((object)fdoCache).GetType().Assembly.GetType("SIL.FieldWorks.FDO.DomainServices.WritingSystemServices");
            dynamic fieldIds = SilTools.ReflectionHelper.GetResult(metaDataCache, "GetFieldIds", null);
            foreach (int flid in fieldIds)
            {
                if (SilTools.ReflectionHelper.GetResult(metaDataCache, "IsCustom", flid))
                {
                    var field = new PaCustomField();
                    field.FieldId = flid;
                    field.className = SilTools.ReflectionHelper.GetResult(metaDataCache, "GetOwnClsName", flid);
                    field.fieldName = SilTools.ReflectionHelper.GetResult(metaDataCache, "GetFieldName", flid);
                    field.fieldType = SilTools.ReflectionHelper.GetResult(metaDataCache, "GetFieldType", flid).ToString();
                    int magicWs = SilTools.ReflectionHelper.GetResult(metaDataCache, "GetFieldWs", flid);
                    var wss = (IEnumerable<object>) SilTools.ReflectionHelper.GetResult(writingSystemServices, "GetWritingSystemList", new object[] { fdoCache, magicWs, false });
                    List<string> tempWs = new List<string>();
                    foreach (var ws in wss)
                    {
                        tempWs.Add(SilTools.ReflectionHelper.GetProperty(ws, "Id").ToString());
                    }
                    field.writingSystems = tempWs.ToArray();
                    fields.Add(field);
                }
            }

            return fields;
        }

        #region Extra fields required for processing
        internal int FieldId;
        #endregion

        #region Implementation of IPaCustomField
        public string ClassName
        {
            get { return className; }
        }

        public string FieldName
        {
            get { return fieldName; }
        }

        public string FieldType
        {
            get { return fieldType; }
        }

        public string[] WritingSystems
        {
            get { return writingSystems; }
        }
        #endregion

    }
}
