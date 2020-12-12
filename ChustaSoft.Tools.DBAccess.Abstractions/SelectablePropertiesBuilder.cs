using System.Collections.Generic;

namespace ChustaSoft.Tools.DBAccess
{

    public class SelectablePropertiesBuilder<TOrigin, TSelection> : ISelectablePropertiesBuilder<TOrigin, TSelection>
    {
        
        public string CurrentProperty { get; private set; }

        protected ICollection<string> _selectedProperties { get; set; }



        public SelectablePropertiesBuilder(string currentProperty)
        {
            CurrentProperty = currentProperty;
            _selectedProperties = new List<string>();
        }
        

        public (string Root, IDictionary<string, ICollection<string>> Nested) Build()
        {
            return (CurrentProperty, new Dictionary<string, ICollection<string>>());
        }

    }


    public class SelectablePropertiesBuilder<TOrigin, TParent, TSelection> : SelectablePropertiesBuilder<TOrigin, TParent>
    {

        private ISelectablePropertiesBuilder<TOrigin, TParent> _parentBuilder;


        public SelectablePropertiesBuilder(string currentProperty)
            : base(currentProperty)
        { }

        public SelectablePropertiesBuilder(string currentProperty, ISelectablePropertiesBuilder<TOrigin, TParent> parentBuilder)
            : base(currentProperty)
        {
            _parentBuilder = parentBuilder;
        }


        public new (string Root, IDictionary<string, ICollection<string>> Nested) Build()
        {
            if (_parentBuilder != null)
            {
                var builder = _parentBuilder.Build();

                builder.Nested.Add(CurrentProperty, _selectedProperties);

                return builder;
            }
            else
            {
                return (CurrentProperty, new Dictionary<string, ICollection<string>>());
            }
        }

    }
}
