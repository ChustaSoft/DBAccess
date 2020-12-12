using System;

namespace ChustaSoft.Tools.DBAccess
{

    public class SelectablePropertiesBuilder<TOrigin, TSelection> : ISelectablePropertiesBuilder<TOrigin, TSelection>
    {

        protected SelectablePropertiesContext _context;


        public SelectablePropertiesBuilder(string selection)
        {
            _context = new SelectablePropertiesContext(typeof(TSelection), selection);
        }

        protected SelectablePropertiesBuilder() { }


        public SelectablePropertiesContext Build() => _context;


        public void Add(Type type, string propertyName)
        {
            _context.Add(type, propertyName);
        }

    }


    public class SelectablePropertiesBuilder<TOrigin, TParent, TSelection> : SelectablePropertiesBuilder<TOrigin, TParent>
    {

        private ISelectablePropertiesBuilder<TOrigin, TParent> _parentBuilder;

       
        public SelectablePropertiesBuilder(string currentProperty, ISelectablePropertiesBuilder<TOrigin, TParent> parentBuilder)
        {

            parentBuilder.Add(typeof(TSelection), currentProperty);

            _parentBuilder = parentBuilder;
        }

        public new SelectablePropertiesContext Build() => _parentBuilder.Build();

    }
}
