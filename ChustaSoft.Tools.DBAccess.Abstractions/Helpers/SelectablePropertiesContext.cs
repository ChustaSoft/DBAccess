using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.DBAccess
{
    public class SelectablePropertiesContext
    {

        public string Property { get; private set; }
        public Type Type { get; private set; }
        public ICollection<SelectablePropertiesContext> Nested { get; set; }


        public SelectablePropertiesContext(Type type, string property)
        {
            Nested = new List<SelectablePropertiesContext>();
            Type = type;
            Property = property;
        }


        internal void Add(Type type, string selected)
        {
            Nested.Add(new SelectablePropertiesContext(type, selected));
        }

    }
}
