using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Opal.Forms;
using Opal.Model.AppConfiguration;
using Opal.src.CommonClasses.SreenProvider;
using Opal.src.TTL.UI.EventControllers;
using Opal.src.Utils;
using ProcessDashboard.src.CommonClasses.SreenProvider;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Opal.src.CommonClasses.DataProvider
{
    public class APIDataProvider : IDataProvider
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private Config _config = Config.Instance;
        private ApiDataSelectController _uiController;
        private readonly HttpClient _httpClient;
        private static MainForm _form;
        private IScreen _screen;

        public APIDataProvider(MainForm form)
        {
            _uiController = new ApiDataSelectController();
            _httpClient = new HttpClient();
            _form = form;
        }

        public void Start()
        {
            if (_screen == null)
            {
                _screen = ScreenFactory.Create(_config.LineID);
                _screen.Show(_form.MainFormPanel);
            }

            if (!CheckApiAccess())
                return;

            _uiController.OpenApiDataFilters();

            var data = Task.Run(() => GetData()).GetAwaiter().GetResult();

            _screen.Update(data, _form);
        }

        private bool CheckApiAccess()
        {
            if (!_config.Auth.ApiReachable)
            {
                _form.SetMessage("API is unreacheble", Colors.Red);
                return false;
            }

            if (!_config.Auth.UserAuthenticated)
            {
                _form.SetMessage("User is not authenticated", Colors.Red);
                return false;
            }

            return true;
        }

        private async Task<List<JObject>> GetData()
        {
            if (!string.IsNullOrEmpty(_config.Filter.Serial))
            {
                return await GetSingleUnit(_config.Filter.Serial);
            }

            if (_config.Filter.Start != null && _config.Filter.End != null)
            {
                return await GetByDateTime(_config.Filter.Start, _config.Filter.End);
            }

            return null;
        }

        private async Task<List<JObject>> GetByDateTime(DateTime from, DateTime to)
        {
            List<JObject> data = new List<JObject>();

            string dtFrom = from.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            string dtTo = to.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            string url = $"{_config.DataProvider.ApiUrl}/ProcessData";
            int page = 1;

            while (true)
            {
                string queryParams = $"?page={page}&fromDate={dtFrom}&toDate={dtTo}";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config.Auth.Token);

                HttpResponseMessage response = await _httpClient.GetAsync(url + queryParams);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                List<JObject> pageContent = JsonConvert.DeserializeObject<List<JObject>>(responseBody);

                bool hasMoreData = pageContent.Count > 0;

                if (!hasMoreData && response.StatusCode == System.Net.HttpStatusCode.OK)
                    break;

                data.AddRange(pageContent);

                page++;
            }

            return data;
        }

        private async Task<List<JObject>> GetSingleUnit(string serial)
        {
            List<JObject> data = new List<JObject>();

            string url = $"{_config.DataProvider.ApiUrl}/ProcessData/{serial}";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config.Auth.Token);

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            var pageContent = JsonConvert.DeserializeObject<JObject>(responseBody);


            data.Add(pageContent);

            return data;
        }
    }
}
