using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.DBAccess
{

    public interface ISelectablePropertiesBuilder
    {

        SelectablePropertiesContext Context { get; }


        IEnumerable<SelectablePropertiesNode> Build();

        void AddFlush(Type type, string propertyName, bool rootSelection);

        void AddDeepen(Type type, string propertyName);

    }

}
