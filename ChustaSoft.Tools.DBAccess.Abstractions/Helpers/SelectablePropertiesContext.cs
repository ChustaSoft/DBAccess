using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.DBAccess
{
    public class SelectablePropertiesContext
    {

        internal ICollection<SelectablePropertiesNode> Siblings { get; private set; }
        internal SelectablePropertiesNode CurrentNode { get; private set; }


        public SelectablePropertiesContext(Type type, string property)
        {
            Siblings = new List<SelectablePropertiesNode>();
            CurrentNode = new SelectablePropertiesNode(type, property);
        }


        internal void AddFlush(Type type, string selected, bool rootSelection)
        {
            CurrentNode.Add(type, selected, rootSelection);
        }

        internal void AddDeepen(Type type, string selected)
        {
            CurrentNode.AddNested(type, selected);
        }

        internal void AddSibling(Type type, string property)
        {
            Siblings.Add(CurrentNode.Clone());
            CurrentNode = new SelectablePropertiesNode(type, property);
        }

        internal ICollection<SelectablePropertiesNode> GetAll() 
        {
            Siblings.Add(CurrentNode.Clone());

            return Siblings;
        }

    }
}
