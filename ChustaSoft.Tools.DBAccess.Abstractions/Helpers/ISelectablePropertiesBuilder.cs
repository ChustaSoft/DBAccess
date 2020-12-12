using System;

namespace ChustaSoft.Tools.DBAccess
{

    public interface ISelectablePropertiesBuilder<TOrigin, TSelection>
    {

        SelectablePropertiesContext Build();

        void Add(Type type, string propertyName);
    }

}
