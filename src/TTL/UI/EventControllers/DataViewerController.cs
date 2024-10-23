using Opal.src.CommonClasses.Containers;
using Opal.src.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Opal.src.TTL.UI.EventControllers
{
    public class DataViewerController
    {
        private DataViewer DV;
        private DataGridView DGV;

        private Dictionary<string, TableDataContainer> TableData;  // Also tooltip text is missing
        private static Func<Dictionary<string, TableDataContainer>> _callback;

        public DataViewerController()
        {
            DV = new DataViewer();
            DGV = DV.DataViewerMainTable;
            DGV.AutoGenerateColumns = false;
            TableData = new Dictionary<string, TableDataContainer>();
            RegisterEvents();
        }

        public void Show()
        {
            DV.ShowDialog();
        }

        public void AddData(string key, TableDataContainer data)
        {
            if (string.IsNullOrEmpty(key)) return;
            if (!data.IsValid()) return;

            if (!TableData.ContainsKey(key))
            {
                TableData.Add(key, data);
                DV.SelectObjectDropDown.Items.Add(key);
            }
            else
            {
                TableData[key] = data;
            }
        }

        public void AddData(Dictionary<string, TableDataContainer> data)
        {
            foreach (var pair in data)
            {
                AddData(pair.Key, pair.Value);
            }
        }

        public void AddData()
        {
            if (_callback == null) return;

            Dictionary<string, TableDataContainer> data = _callback();

            AddData(data);
        }

        public void AddDataCallback(Func<Dictionary<string, TableDataContainer>> callback)
        {
            _callback = callback;
        }

        public void Clear()
        {
            TableData.Clear();
            ClearScreen();
            DV.SelectObjectDropDown.Items.Clear();
        }

        private void ClearScreen()
        {
            DGV.Columns.Clear();
            DGV.Rows.Clear();
        }

        private void ShowData(string key)
        {
            if (string.IsNullOrEmpty(key)) return;
            if (!TableData.ContainsKey(key)) return;

            ClearScreen();

            var data = TableData[key];

            foreach (var colName in data.ColumnsNames)
            {
                var col = new DataGridViewTextBoxColumn
                {
                    Name = colName,
                    HeaderText = colName
                };

                DGV.Columns.Add(col);
            }

            foreach (var row in data.DataRows)
            {
                DGV.Rows.Add(row.ToArray());
            }

            DGV.Refresh();
        }

        private void FindRow()
        {
            // Search with choosing the column where to perform the search
            // or
            // Search across all columns

            var searchValue = DV.DataViewerInputField.Text;

            if (string.IsNullOrEmpty(searchValue)) return;

            var row = DGV.Rows.Cast<DataGridViewRow>()
                .FirstOrDefault(r => r.Cells[0].Value != null && r.Cells[0].Value.ToString() == searchValue);

            if (row != null)
            {
                DGV.CurrentCell = row.Cells[0];
                DGV.Rows[DGV.CurrentCell.RowIndex].Selected = true;
            }
        }

        private void RegisterEvents()
        {
            DV.DataViewerSearchButton.Click += DataViewerSearchButton_Click;
            DV.DataViewerInputField.KeyPress += DataViewerInputField_KeyPress;
            DV.FormClosing += DataViewer_FormClosing;
            DV.SelectObjectDropDown.SelectedIndexChanged += SelectObjectDropDown_SelectedIndexChanged;
        }

        private void DataViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                DV.Hide();
                // Why hide instead of dispose?
            }
        }

        private void DataViewerSearchButton_Click(object sender, EventArgs e)
        {
            FindRow();
        }

        private void DataViewerInputField_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                FindRow();
                e.Handled = true;
            }
        }

        private void SelectObjectDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolStripComboBox comboBox = sender as ToolStripComboBox;
            string selectedKey = comboBox?.SelectedItem?.ToString();

            if (selectedKey != null && TableData.ContainsKey(selectedKey))
            {
                ShowData(selectedKey);
            }
        }
    }
}
