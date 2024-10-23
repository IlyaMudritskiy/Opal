using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Linq;
using Opal.Forms;
using Opal.Model.AppConfiguration;
using Opal.src.CommonClasses.Containers;
using Opal.src.CommonClasses.SreenProvider;
using Opal.src.Utils;
using ProcessDashboard.src.CommonClasses.SreenProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Opal.src.CommonClasses.DataProvider
{
    public class HubDataProvider : IDataProvider
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private Config _config = Config.Instance;
        private static MainForm _form;
        private IScreen _screen;
        private string _machineName;

        public HubDataProvider(MainForm form)
        {
            _form = form;
            _machineName = Environment.MachineName;
        }

        public void Start()
        {
            if (_screen == null)
            {
                _screen = ScreenFactory.Create(_config.LineID);
                _screen.Show(_form.MainFormPanel);
            }

            Task.Run(() => StartSignalRConnectionAsync());
        }

        private async Task StartSignalRConnectionAsync()
        {
            var connection = new HubConnectionBuilder()
                .WithUrl($"{_config.DataProvider.HubUrl}?clientId={_machineName}&lineId={_config.LineID}&typeId={_config.ProductID}")
                .WithAutomaticReconnect()
                .Build();

            InvokeOnUiThread(() => _form.SetMessage($"Hub info: Machine: {_machineName}, Line: {_config.LineID}, Product: {_config.ProductID}", Colors.Green));
            Log.Info($"[CONNECTION INFO] Hub info: Machine: {_machineName}, Line: {_config.LineID}, Product: {_config.ProductID}");
            Log.Info("[OK] Conection created");

            connection.On<string>("ReceiveMessage", message =>
            {
                Log.Info($"[OK] On ReceiveMessage: {message}");
                InvokeOnUiThread(() => _form.SetMessage(message, Colors.Green));
            });

            connection.On<string>("NewDataAvailable", message =>
            {
                try
                {
                    var parsedMessage = JObject.Parse(message);
                    Log.Info($"[PARSED] On NewDataAvailable: {parsedMessage["DUT"]["serial_nr"]}");
                    InvokeOnUiThread(() => _form.SetMessage($"{parsedMessage["DUT"]["serial_nr"]}", Colors.Green));
                    InvokeOnUiThread(() => _screen.Update(parsedMessage, _form));
                }
                catch (Exception ex)
                {
                    Log.Error($"On NewDataAvailable exception: {ex.Message}");
                }
            });

            connection.On<string>("CloseConnection", message =>
            {
                Log.Info(message);
                try
                {
                    Log.Info($"[OK] Connection Closed: {message}");
                    connection.StopAsync().Wait();
                    InvokeOnUiThread(() => _form.SetMessage($"Connection closed", Colors.Green));
                }
                catch (Exception ex)
                {
                    Log.Error($"Error stopping connection: {ex.Message}");
                }
            });

            connection.On<string>("AddedToGroup", message =>
            {
                Log.Info($"[OK] {message}");
                InvokeOnUiThread(() => _form.SetMessage(message, Colors.Green));
            });

            try
            {
                await connection.StartAsync();
                Log.Info($"[OK] Connection started");
            }
            catch (Exception ex)
            {
                Log.Error($"Exception: {ex.Message}");
            }
        }

        private void InvokeOnUiThread(Action action)
        {
            if (_form.InvokeRequired)
            {
                _form.BeginInvoke(action);
            }
            else
            {
                action.Invoke();
            }
        }

        public Func<Dictionary<string, TableDataContainer>> GetDVCallback()
        {
            return null;
        }
    }
}
