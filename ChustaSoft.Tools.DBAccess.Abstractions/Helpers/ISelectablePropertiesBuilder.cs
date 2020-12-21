using System;

namespace ChustaSoft.Tools.DBAccess
{

    public interface ISelectablePropertiesBuilder
    {

        SelectablePropertiesContext Build();

        void AddFlush(Type type, string propertyName, bool rootSelection);

        void AddDeepen(Type type, string propertyName);

    }

}
