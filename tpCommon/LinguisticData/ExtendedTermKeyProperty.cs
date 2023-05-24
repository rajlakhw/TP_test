using System;
using System.Collections.Generic;

#nullable disable

namespace LinguisticData
{
    public partial class ExtendedTermKeyProperty
    {
        public int TermKeyId { get; set; }
        public int ExtendedPropertyDefinitionId { get; set; }
        public short DataType { get; set; }
        public string PropertyValue { get; set; }
    }
}
