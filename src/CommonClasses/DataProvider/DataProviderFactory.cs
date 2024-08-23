using Opal.Forms;
using Opal.Model.AppConfiguration;

namespace Opal.src.CommonClasses.DataProvider
{
    public static class DataProviderFactory
    {
        public static IDataProvider Get(string dataProvider, MainForm form)
        {
            if (dataProvider.ToLower() == DataProviderType.File)
                return new FileDataProvider(form);

            if (dataProvider.ToLower() == DataProviderType.API)
                return new APIDataProvider(form);

            if (dataProvider.ToLower() == DataProviderType.Hub)
                return null;

            return null;
        }
    }
}
