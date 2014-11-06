using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIL.PaToFdoInterfaces
{
    public interface IPaCustomFieldValue
    {
        string FieldName { get; }
        string WritingSystem { get; }
        string Value { get; }
    }
}
