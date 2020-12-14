using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.DBAccess
{
    public class SelectablePropertiesContext
    {

        private string _lastProperty;

        public string Property { get; private set; }
        public Type Type { get; private set; }
        public ICollection<SelectablePropertiesContext> Nested { get; set; }


        public SelectablePropertiesContext(Type type, string property)
        {
            Nested = new List<SelectablePropertiesContext>();
            Type = type;
            Property = property;
            _lastProperty = property;
        }


        internal void AddFlush(Type type, string selected)
        {
            _lastProperty = selected;
            Nested.Add(new SelectablePropertiesContext(type, selected));
        }

        internal void AddDeepen(Type type, string selected)
        {
            foreach (var nestedElement in Nested)
                if (nestedElement.Property == _lastProperty)
                    nestedElement.AddFlush(type, selected);
        }

    }
}
