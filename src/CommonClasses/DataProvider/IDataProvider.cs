using ProcessDashboard.src.CommonClasses.SreenProvider;
using System;

namespace Opal.src.CommonClasses.DataProvider
{
    public interface IDataProvider
    {
        string GetLineCode();
        void ProvideData(IScreen screen);
    }
}
