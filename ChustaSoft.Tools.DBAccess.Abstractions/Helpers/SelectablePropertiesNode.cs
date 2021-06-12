using System;
using System.Collections.Generic;
using System.Linq;

namespace ChustaSoft.Tools.DBAccess
{
    public class SelectablePropertiesNode
    {

        private int _currentDeepLevel = 0;
        private string _lastRootedProperty;


        public string Property { get; internal set; }
        public Type Type { get; internal set; }
        public ICollection<SelectablePropertiesNode> Nested { get; internal set; }


        internal SelectablePropertiesNode(Type type, string property)
        {
            _lastRootedProperty = property;

            Nested = new List<SelectablePropertiesNode>();
            Property = property;
            Type = type;
        }


        internal void Add(Type type, string selected, bool rootSelection)
        {
            var currentDeepestNestedLevel = GetDeepestNestedLevel(this, selected);

            if (rootSelection)
                _lastRootedProperty = selected;

            if (currentDeepestNestedLevel.Any(x => x.Property == selected))
                throw new ArgumentException($"Properties selection already contains {selected} property");
            else
                currentDeepestNestedLevel.Add(new SelectablePropertiesNode(type, selected));
        }

        internal void AddNested(Type type, string selected)
        {
            var currentDeepestNestedLevel = GetDeepestNestedLevel(this, selected);

            foreach (var nestedElement in currentDeepestNestedLevel)
                if (nestedElement.Property == _lastRootedProperty)
                    nestedElement.Add(type, selected, true);

            _currentDeepLevel++;
        }

        internal SelectablePropertiesNode Clone() => (SelectablePropertiesNode)this.MemberwiseClone();


        private ICollection<SelectablePropertiesNode> GetDeepestNestedLevel(SelectablePropertiesNode contextNode, string property, int descendedLevels = 0)
        {
            if (descendedLevels < _currentDeepLevel)
                return GetDeepestNestedLevel(contextNode.Nested.First(x => x.Property == _lastRootedProperty), property, ++descendedLevels);
            else
                return contextNode.Nested;
        }

    }
}
