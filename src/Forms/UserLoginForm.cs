using Newtonsoft.Json;
using Opal.Model.AppConfiguration;
using System.Net.Http;
using System.Net;
using System.Text;
using System;
using System.Windows.Forms;

namespace Opal.src.Forms
{
    public partial class UserLoginForm : Form
    {
        private static Config _config = Config.Instance;

        public UserLoginForm()
        {
            InitializeComponent();
            this.Login_btn.Click += (sender, e) => LogIn();
        }

        public void LogIn()
        {
            SetElementsEnabled(false);

            var url = $"{_config.DataProvider.ApiUrl}/Auth/Login";
            var user = Username_txb.Text;
            var passwd = Password_txb.Text;

            using (var client = new HttpClient())
            {
                var payload = new
                {
                    username = user,
                    password = passwd
                };
                var jsonPayload = JsonConvert.SerializeObject(payload);
                var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                try
                {
                    var response = client.PostAsync(url, httpContent).Result; // Force synchronous execution
                    var content = response.Content.ReadAsStringAsync().Result; // Force synchronous execution

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        _config.Auth.Token = content;
                        ResultMessage_lbl.Text = "Login OK.";
                        return;
                    }
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        ResultMessage_lbl.Text = content;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            SetElementsEnabled(true);
        }

        private void SetElementsEnabled(bool enabled)
        {
            Username_lbl.Enabled = enabled;
            Password_lbl.Enabled = enabled;

            Username_txb.Enabled = enabled;
            Password_txb.Enabled = enabled;

            Login_btn.Enabled = enabled;
        }
    }
}
