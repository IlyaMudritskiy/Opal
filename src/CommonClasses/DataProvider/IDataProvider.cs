using Opal.src.CommonClasses.Containers;
using System;
using System.Collections.Generic;

namespace Opal.src.CommonClasses.DataProvider
{
    public interface IDataProvider
    {
        void Start();
        Func<Dictionary<string, TableDataContainer>> GetDVCallback();
    }
}
