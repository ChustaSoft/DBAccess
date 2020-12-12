using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.DBAccess
{

    public interface ISelectablePropertiesBuilder<TOrigin, TSelection>
    {

        string CurrentProperty { get; }

        (string Root, IDictionary<string, ICollection<string>> Nested) Build();
    }

}
