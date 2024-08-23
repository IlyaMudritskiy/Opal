using Opal.Model.AppConfiguration;
using Opal.src.CommonClasses.Containers;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Opal.src.Forms
{
    public partial class APIDataSelectorForm : Form
    {
        private string queriesPath = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Queries";
        private static Config _config = Config.Instance;

        public APIDataSelectorForm()
        {
            InitializeComponent();
            RefreshQueries_btn.Click += (sender, e) => RefreshQueriesList();
            Clear_btn.Click += (sender, e) => ClearQueriesList();
            Serial_txb.TextChanged += (sender, e) => _config.Filter.Serial = Serial_txb.Text;

            RestoreState();
        }

        private void ClearQueriesList()
        {
            CustomQuery_cmb.Text = string.Empty;
        }

        private void RefreshQueriesList()
        {
            string[] files = Directory.GetFiles(queriesPath, "*.json", SearchOption.AllDirectories);
            CustomQuery_cmb.Items.Clear();

            foreach (var f in files)
            {
                CustomQuery_cmb.Items.Add(Path.GetFileName(f));
            }
        }

        public void SaveState()
        {
            var (from, to) = GetDateTimeRange();

            _config.Filter.Start = from;
            _config.Filter.End = to;
            _config.Filter.Serial = Serial_txb.Text.Trim();
            _config.Filter.SelectedRbn = GetSelectedRbn();
            _config.Filter.CustomQueryFile = CustomQuery_cmb.Text;
        }

        private void RestoreState()
        {
            if (_config.Filter == null)
            {
                _config.Filter = new SearchFilter();
                SaveState();
                return;
            }

            SetRadioButtonCheckedByName(_config.Filter.SelectedRbn);
            Serial_txb.Text = _config.Filter.Serial;
            CustomQuery_cmb.Text = _config.Filter.CustomQueryFile;

            if (Range_rbn.Checked)
            {
                DateFrom_dtp.Value = _config.Filter.Start;
                TimeFrom_dtp.Value = _config.Filter.Start;

                DateTo_dtp.Value = _config.Filter.End;
                TimeTo_dtp.Value = _config.Filter.End;
            }
        }

        private string GetSelectedRbn()
        {
            return DateTimeRange_grp.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked)?.Name;
        }

        public (DateTime from, DateTime to) GetDateTimeRange()
        {
            if (ThisHour_rbn.Checked)
                return GetCurrentHour();

            if (LastHour_rbn.Checked)
                return GetLastHour();

            if (LastMinutes_rbn.Checked)
                return GetLastMinutes();

            if (Range_rbn.Checked)
                return GetDateTimeCustomRange();

            return GetCurrentHour();
        }

        private (DateTime from, DateTime to) GetDateTimeCustomRange()
        {
            var fromDate = DateFrom_dtp.Value.Date;
            var fromTime = TimeFrom_dtp.Value.TimeOfDay;
            var toDate = DateTo_dtp.Value.Date;
            var toTime = TimeTo_dtp.Value.TimeOfDay;

            DateTime dtFrom = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, fromTime.Hours, fromTime.Minutes, 0);
            DateTime dtTo = new DateTime(toDate.Year, toDate.Month, toDate.Day, toTime.Hours, toTime.Minutes, 0);

            return (dtFrom, dtTo);
        }

        private (DateTime from, DateTime to) GetCurrentHour()
        {
            DateTime from = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
            DateTime to = DateTime.Now;

            return (from, to);
        }

        private (DateTime from, DateTime to) GetLastHour()
        {
            DateTime from = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour - 1, 0, 0);
            DateTime to = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour - 1, 59, 59);

            return (from, to);
        }

        private (DateTime from, DateTime to) GetLastMinutes()
        {
            int offset = 30;
            if (!string.IsNullOrEmpty(LastMinutes_txb.Text))
                int.TryParse(LastMinutes_txb.Text, out offset);

            DateTime to = DateTime.Now;
            DateTime from = to.AddMinutes(offset * (-1));
            
            return (from, to);
        }

        private void SetRadioButtonCheckedByName(string radioButtonName)
        {
            foreach (Control control in DateTimeRange_grp.Controls)
            {
                if (control is RadioButton rb && rb.Name == radioButtonName)
                {
                    rb.Checked = true;
                    break;
                }
            }
        }
    }
}
