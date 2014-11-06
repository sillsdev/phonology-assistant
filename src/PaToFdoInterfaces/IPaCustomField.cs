using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIL.PaToFdoInterfaces
{
    public interface IPaCustomField
    {
        string ClassName { get; }
        string FieldName { get; }
        string FieldType { get; }
        string[] WritingSystems { get; }
    }
}
