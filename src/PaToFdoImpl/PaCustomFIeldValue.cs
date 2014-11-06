using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIL.PaToFdoInterfaces;

namespace SIL.FieldWorks.PaObjects
{
    public class PaCustomFieldValue : IPaCustomFieldValue
    {
        private string fieldName;
        private string wrtingSystem;
        private string value;

        #region implementation of IPaCustomFieldValue
        public string FieldName
        {
            get { return fieldName; }
        }

        public string WritingSystem
        {
            get { return wrtingSystem; }
        }

        public string Value
        {
            get { return value; }
        }
        #endregion


        internal static List<PaCustomFieldValue> GetCustomFields(dynamic lxEntry, string className, List<PaCustomField> customFields, dynamic svcloc)
        {
            var result = new List<PaCustomFieldValue>();
            dynamic cache = SilTools.ReflectionHelper.GetProperty(lxEntry, "Cache");
            dynamic dataAccess = SilTools.ReflectionHelper.GetProperty(cache, "DomainDataByFlid");
            int entryHvo = SilTools.ReflectionHelper.GetProperty(lxEntry, "Hvo");
            foreach (var field in customFields.Where(f => f.ClassName == className))
            {
                int fieldType = int.Parse(field.FieldType);
                // TODO: currently only getting multi strings, need to handle other types
                if (fieldType >= 13 && fieldType <= 16)
                {
                    dynamic multiStringProp = SilTools.ReflectionHelper.GetResult(dataAccess, "get_MultiStringProp", new object[] { entryHvo, field.FieldId });
                    PaMultiString multiString = PaMultiString.Create(multiStringProp, svcloc);
                    if (multiString != null)
                    {
                        foreach (var ws in field.WritingSystems)
                        {
                            var text = multiString.GetString(ws);
                            if (text != null)
                            {
                                var value = new PaCustomFieldValue();
                                value.fieldName = field.FieldName;
                                value.wrtingSystem = ws;
                                value.value = text;
                                result.Add(value);
                            }
                        }
                    }
                }

            }
            return result;
        }
    }
}
