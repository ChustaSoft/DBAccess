using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.DBAccess
{

    public abstract class SelectablePropertiesBuilderBase : ISelectablePropertiesBuilder
    {

        public SelectablePropertiesContext Context { get; protected set; }

        public IEnumerable<SelectablePropertiesNode> Build() => Context.GetAll();

        public void AddFlush(Type type, string propertyName, bool rootSelection)
        {
            Context.AddFlush(type, propertyName, rootSelection);
        }

        public void AddDeepen(Type type, string propertyName)
        {
            Context.AddDeepen(type, propertyName);
        }

        
        
    }



    public class SelectablePropertiesBuilder<TOrigin, TSelection> : SelectablePropertiesBuilderBase
    {

        public SelectablePropertiesBuilder(string selection)
        {
            Context = new SelectablePropertiesContext(typeof(TSelection), selection);
        }

        public SelectablePropertiesBuilder(ISelectablePropertiesBuilder currentBuilder, string currentProperty)
        {
            Context = currentBuilder.Context;
            Context.AddSibling(typeof(TSelection), currentProperty);
        }

    }


    public class SelectablePropertiesBuilder<TOrigin, TParent, TSelection> : SelectablePropertiesBuilderBase
    {

        public SelectablePropertiesBuilder(ISelectablePropertiesBuilder parentBuilder)
        {
            Context = parentBuilder.Context;
        }

        public SelectablePropertiesBuilder(ISelectablePropertiesBuilder parentBuilder, string currentProperty)
        {
            parentBuilder.AddFlush(typeof(TSelection), currentProperty, true);
            Context = parentBuilder.Context;
        }

    }
}
