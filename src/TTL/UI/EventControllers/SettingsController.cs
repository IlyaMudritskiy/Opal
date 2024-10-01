using Opal.Model.AppConfiguration;
using Opal.src.CommonClasses.DataProvider;
using Opal.src.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using System.Net;
using System;
using System.Drawing;
using Opal.src.Utils;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using System.Threading;

namespace Opal.src.TTL.UI.EventControllers
{
    public class SettingsController
    {
        private SettingsForm _sf;
        private static Config _config = Config.Instance;
        private string dataProvider = "files";
        private UserLoginForm _logInForm;

        public SettingsController()
        {
            _sf = new SettingsForm();
            _logInForm = new UserLoginForm();
            RegisterEvents();
            MapConfigToSettingsUI();
        }

        private void RegisterEvents()
        {
            _sf.FilesProvider_rbn.CheckedChanged += (sender, e) => EnableGroup((RadioButton)sender);
            _sf.ApiProvider_rbn.CheckedChanged += (sender, e) => EnableGroup((RadioButton)sender);
            _sf.HubProvider_rbn.CheckedChanged += (sender, e) => EnableGroup((RadioButton)sender);
            _sf.FilesProviderAvoustic_chb.CheckedChanged += (sender, e) => EnableAcousticCustomPath((CheckBox)sender);
            _sf.LineName_cmb.TextChanged += (sender, e) => SetProductIdFromLine((ComboBox)sender);
            _sf.Save_btn.Click += (sender, e) => SaveButtonClick();
            _sf.Close_btn.Click += (sender, e) => _sf.Close();
            _sf.Status_lbl.DoubleClick += (sender, e) => ToggleSettingsControlsEnabled();
        }

        public void OpenSettings()
        {
            _sf.ShowDialog();
            _sf.StatusText_lbl.Text = "";
        }

        private void SaveButtonClick()
        {
            // General
            SaveConfig();
            ShowStatus("Configuration saved!", Colors.Green);
            if (_sf.ApiProvider_rbn.Checked)
                CheckAvailability();
            if (_sf.HubProvider_rbn.Checked)
                CheckHub();
        }

        private void SaveConfig()
        {
            _config.ProductID = _sf.ProductId_cmb.Text;
            _config.LineID = _sf.LineName_cmb.Text;

            // Acoustic
            _config.Acoustic.Enabled = _sf.FilesProviderAvoustic_chb.Checked;
            _config.Acoustic.CustomFilesLocation = _sf.FilesLocation_txb.Text;

            // Data Provider
            _config.DataProvider.Type = dataProvider;
            _config.DataProvider.ApiUrl = _sf.ApiUrl_txb.Text;
            _config.DataProvider.HubUrl = _sf.HubUrl_txb.Text;
            _config.HubBufferSize = int.Parse(_sf.BufferSize_txb.Text); // TextChanged checks input against regex

            // Other
            _config.DataDriveLetter = _sf.DataDriveLetter_cmb.Text;
            _config.ASxReports = _sf.ASxCompliant_chk.Checked;

            _config.Save();
        }

        private void MapConfigToSettingsUI()
        {
            _sf.FilesProvider_rbn.Checked = _config.DataProvider.Type == DataProviderType.File;
            _sf.FilesProviderAvoustic_chb.Checked = _config.Acoustic.Enabled;
            _sf.FilesLocation_txb.Text = _config.Acoustic.CustomFilesLocation;

            _sf.ApiProvider_rbn.Checked = _config.DataProvider.Type == DataProviderType.API;
            _sf.ApiUrl_txb.Text = _config.DataProvider.ApiUrl;

            _sf.HubProvider_rbn.Checked = _config.DataProvider.Type == DataProviderType.Hub;
            _sf.HubUrl_txb.Text = _config.DataProvider.HubUrl;

            SetComboboxItems(_sf.LineName_cmb, _config.LineProductMap.Keys.ToList());
            _sf.LineName_cmb.Text = _config.LineID;
            _sf.ProductId_cmb.Text = _config.ProductID;

            _sf.DataDriveLetter_cmb.Text = _config.DataDriveLetter;
            _sf.ASxCompliant_chk.Checked = _config.ASxReports;

            _sf.BufferSize_txb.Text = _config.HubBufferSize.ToString();
        }

        private void EnableAcousticCustomPath(CheckBox sender)
        {
            _sf.FilesLocation_txb.Enabled = sender.Checked;
        }

        private void SetProductIdFromLine(ComboBox sender)
        {
            var items = _config.LineProductMap[sender.Text];
            SetComboboxItems(_sf.ProductId_cmb, items);
            _sf.ProductId_cmb.Text = _sf.ProductId_cmb.Items[0].ToString();
        }

        private void EnableGroup(RadioButton sender)
        {
            SetEnabledGroup(_sf.FilesProvider_grp, false);
            SetEnabledGroup(_sf.ApiProvider_grp, false);
            SetEnabledGroup(_sf.HubProvider_grp, false);
            SetEnabledGroup(_sf.LineProduct_grp, false);
            SetEnabledGroup(_sf.OtherSettings_grp, false);

            switch (sender.Name)
            {
                case "FilesProvider_rbn":
                    SetEnabledGroup(_sf.FilesProvider_grp, true);
                    SetEnabledGroup(_sf.OtherSettings_grp, true);
                    _sf.FilesLocation_txb.Enabled = _sf.FilesProviderAvoustic_chb.Checked;
                    dataProvider = "files";
                    break;
                case "ApiProvider_rbn":
                    SetEnabledGroup(_sf.ApiProvider_grp, true);
                    SetEnabledGroup(_sf.LineProduct_grp, true);
                    dataProvider = "api";
                    break;
                case "HubProvider_rbn":
                    SetEnabledGroup(_sf.HubProvider_grp, true);
                    SetEnabledGroup(_sf.LineProduct_grp, true);
                    dataProvider = "hub";
                    break;
            }
        }

        private void SetEnabledGroup(GroupBox grp, bool enabled)
        {
            grp.Enabled = enabled;
            foreach (Control control in grp.Controls)
                control.Enabled = enabled;
        }

        private void SetComboboxItems(ComboBox cb, List<string> items)
        {
            cb.Items.Clear();
            foreach (var item in items)
                cb.Items.Add(item);
        }

        private void ShowStatus(string text)
        {
            _sf.StatusText_lbl.Text = text;
        }

        private void ShowStatus(string text, Color color)
        {
            _sf.StatusText_lbl.Text = text;
            _sf.StatusText_lbl.ForeColor = color;
        }

        private void SetSettingsControlsEnabled()
        {
            SetEnabledGroup(_sf.FilesProvider_grp, _config.SettingsEnabled);
            SetEnabledGroup(_sf.ApiProvider_grp, _config.SettingsEnabled);
            SetEnabledGroup(_sf.HubProvider_grp, _config.SettingsEnabled);
            SetEnabledGroup(_sf.LineProduct_grp, _config.SettingsEnabled);
            SetEnabledGroup(_sf.OtherSettings_grp, _config.SettingsEnabled);
            _sf.Close_btn.Enabled = _config.SettingsEnabled;
            _sf.Save_btn.Enabled = _config.SettingsEnabled;
            _sf.FilesProvider_rbn.Enabled = _config.SettingsEnabled;
            _sf.ApiProvider_rbn.Enabled= _config.SettingsEnabled;
            _sf.HubProvider_rbn.Enabled=_config.SettingsEnabled;
        }

        private void ToggleSettingsControlsEnabled()
        {
            _config.SettingsEnabled = !_config.SettingsEnabled;
            SetSettingsControlsEnabled();
            SaveConfig();
        }

        private void CheckAvailability(bool login)
        {
            _sf.Close_btn.Enabled = false;
            string status = "";
            Color color = Color.Black;
            if (_sf.ApiProvider_rbn.Checked)
            {
                (status, color) = PingApi(login);
            }
            ShowStatus(status, color);
            _sf.Close_btn.Enabled = true;
        }

        private void CheckAvailability()
        {
            CheckAvailability(true);
        }

        private (string, Color) PingApi(bool login)
        {
            var url = $"{_config.DataProvider.ApiUrl}/Auth/Ping";
            var status = "";
            var color = Colors.Green;

            string token = _config.Auth.Token;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Accept", "application/json");

                    if (!string.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    }

                    var response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        status = "API is reachable.";
                        _config.Auth.UserAuthenticated = true;
                        _config.Auth.ApiReachable = true;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized && login)
                    {
                        status = "API is reachable, Log In required.";
                        color = Colors.Yellow;
                        _config.Auth.UserAuthenticated = false;
                        _config.Auth.ApiReachable = true;
                        _logInForm.Show();
                    }
                    else
                    {
                        _config.Auth.UserAuthenticated = false;
                        _config.Auth.ApiReachable = false;
                        status = "API is unreachable.";
                        color = Colors.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                _config.Auth.UserAuthenticated = false;
                _config.Auth.ApiReachable = false;
                status = $"API is unreachable.";
                color = Colors.Red;
            }
            return (status, color);
        }

        private void CheckHub()
        {
            var (message, color, available) = Task.Run(() => GetHubStatus()).GetAwaiter().GetResult();
            ShowStatus(message, color);
            _config.Auth.HubAvailable = available;
        }

        private async Task<(string Message, Color Color, bool Available)> GetHubStatus()
        {
            var machineName = Environment.MachineName;
            var connection = new HubConnectionBuilder()
                .WithUrl($"{_config.DataProvider.HubUrl}?clientId={machineName}&lineId={_config.LineID}&typeId={_config.ProductID}")
                .WithAutomaticReconnect()
                .Build();

            string message = "";
            Color color = Colors.Black;
            bool available = false;

            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(10));

            var closedTcs = new TaskCompletionSource<object>();

            connection.Closed += async (error) =>
            {
                closedTcs.TrySetResult(null);
                await connection.StartAsync();
            };

            connection.On<string>("ReceiveMessage", async m =>
            {
                message = m;
                color = Colors.Green;
                available = true;
                //await CleanupHubConnection(connection, cts);
            });

            connection.On<string>("CloseConnection", async m =>
            {
                message = m;
                color = Colors.Red;
                available = false;
                await connection.StopAsync();
                await CleanupHubConnection(connection, cts);
            });

            connection.On<string>("AddedToGroup", async m =>
            {
                message = m;
                color = Colors.Green;
                available = true;
                await connection.StopAsync();
                await CleanupHubConnection(connection, cts);
            });

            await connection.StartAsync();

            await Task.WhenAny(Task.Delay(-1, cts.Token), closedTcs.Task);

            return (message, color, available);
        }

        private async Task CleanupHubConnection(HubConnection connection, CancellationTokenSource cts)
        {
            cts.Cancel();
            await connection.DisposeAsync();
        }
    }
}
