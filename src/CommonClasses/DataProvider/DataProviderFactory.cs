using Opal.Model.AppConfiguration;

namespace Opal.src.CommonClasses.DataProvider
{
    public static class DataProviderFactory
    {
        public static IDataProvider Get(string dataProvider)
        {
            if (dataProvider.ToLower() == DataProviderType.File)
                return new FileDataProvider();

            if (dataProvider.ToLower() == DataProviderType.API)
                return null;

            if (dataProvider.ToLower() == DataProviderType.Hub)
                return null;

            return null;
        }
    }
}
