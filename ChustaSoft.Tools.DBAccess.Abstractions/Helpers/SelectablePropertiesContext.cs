using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.DBAccess
{
    public class SelectablePropertiesContext
    {

        private SelectablePropertiesNode _currentNode;


        public string Property => _currentNode.Property;
        public Type Type => _currentNode.Type;
        public ICollection<SelectablePropertiesNode> Nested => _currentNode.Nested;
        public ICollection<SelectablePropertiesNode> Siblings { get; private set; }


        public SelectablePropertiesContext(Type type, string property)
        {
            Siblings = new List<SelectablePropertiesNode>();
            _currentNode = new SelectablePropertiesNode(type, property);
        }


        internal void AddFlush(Type type, string selected, bool rootSelection)
        {
            _currentNode.Add(type, selected, rootSelection);
        }

        internal void AddDeepen(Type type, string selected)
        {
            _currentNode.AddNested(type, selected);
        }

        internal void AddSibling(Type type, string property)
        {
            Siblings.Add(_currentNode.Clone());
            _currentNode = new SelectablePropertiesNode(type, property);
        }

        internal ICollection<SelectablePropertiesNode> GetAll() 
        {
            Siblings.Add(_currentNode.Clone());

            return Siblings;
        }

    }
}
