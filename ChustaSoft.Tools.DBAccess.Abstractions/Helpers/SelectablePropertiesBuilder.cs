using System;

namespace ChustaSoft.Tools.DBAccess
{

    public abstract class SelectablePropertiesBuilderBase : ISelectablePropertiesBuilder
    {

        protected SelectablePropertiesContext _context;


        public SelectablePropertiesContext Build() => _context;

        public void AddFlush(Type type, string propertyName, bool rootSelection)
        {
            _context.AddFlush(type, propertyName, rootSelection);
        }

        public void AddDeepen(Type type, string propertyName)
        {
            _context.AddDeepen(type, propertyName);
        }

    }



    public class SelectablePropertiesBuilder<TOrigin, TSelection> : SelectablePropertiesBuilderBase
    {

        public SelectablePropertiesBuilder(string selection)
        {
            _context = new SelectablePropertiesContext(typeof(TSelection), selection);
        }

    }


    public class SelectablePropertiesBuilder<TOrigin, TParent, TSelection> : SelectablePropertiesBuilderBase
    {

        public SelectablePropertiesBuilder(ISelectablePropertiesBuilder parentBuilder)
        {
            _context = parentBuilder.Build();
        }

        public SelectablePropertiesBuilder(ISelectablePropertiesBuilder parentBuilder, string currentProperty)
        {
            parentBuilder.AddFlush(typeof(TSelection), currentProperty, true);
            _context = parentBuilder.Build();
        }

    }
}
