using System;
using System.Collections.Generic;
using System.Linq;

namespace ChustaSoft.Tools.DBAccess
{
    public class SelectablePropertiesContext
    {

        private int _currentDeepLevel = 0;
        private string _lastRootedProperty;

        public string Property { get; private set; }
        public Type Type { get; private set; }
        public ICollection<SelectablePropertiesContext> Nested { get; set; }


        public SelectablePropertiesContext(Type type, string property)
        {
            Nested = new List<SelectablePropertiesContext>();
            Type = type;
            Property = property;
            _lastRootedProperty = property;
        }


        internal void AddFlush(Type type, string selected, bool rootSelection)
        {
            var currentDeepestNestedLevel = GetDeepestNestedLevel(this, selected);

            if (rootSelection)
                _lastRootedProperty = selected;

            if (currentDeepestNestedLevel.Any(x => x.Property == selected))
                throw new ArgumentException($"Properties selection already contains {selected} property");
            else
                currentDeepestNestedLevel.Add(new SelectablePropertiesContext(type, selected));
        }

        internal void AddDeepen(Type type, string selected)
        {
            var currentDeepestNestedLevel = GetDeepestNestedLevel(this, selected);

            foreach (var nestedElement in currentDeepestNestedLevel)
                if (nestedElement.Property == _lastRootedProperty)
                    nestedElement.AddFlush(type, selected, true);

            _currentDeepLevel++;
        }


        private ICollection<SelectablePropertiesContext> GetDeepestNestedLevel(SelectablePropertiesContext context, string property, int descendedLevels = 0)
        {
            if (descendedLevels < _currentDeepLevel)
                return GetDeepestNestedLevel(context.Nested.First(x => x.Property == _lastRootedProperty), property, ++descendedLevels);
            else
                return context.Nested;
        }

    }
}
