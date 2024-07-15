using ProcessDashboard.Forms;
using ProcessDashboard.src.Forms;
using ProcessDashboard.src.TTL.Containers.Common;
using ProcessDashboard.src.TTL.Containers.ScreenData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ProcessDashboard.src.TTL.UI.EventControllers
{
    public class DataViewerController
    {
        //private MainForm mainForm;
        private DataViewer DV;
        private DataGridView DGV;

        private TTLData TTLData { get; set; }

        public DataViewerController(MainForm mainForm)
        {
            //this.mainForm = mainForm;
            DV = new DataViewer();
            DGV = DV.DataViewerMainTable;
            DGV.AutoGenerateColumns = false;
            RegisterEvents();
        }

        public void Show()
        {
            DV.Show();
        }

        public void AddData(TTLData data)
        {
            this.TTLData = data;
        }

        private void ClearData()
        {
            DGV.Columns.Clear();
            DGV.DataSource = null;
            DGV.Refresh();
        }

        private void AddSerialColumn()
        {
            var serialColumn = new DataGridViewTextBoxColumn
            {
                Name = "Serial",
                HeaderText = "Serial",
                ToolTipText = "Serial number"
            };
            DGV.Columns.Add(serialColumn);
        }

        private void AddWPCColumn()
        {
            var WPCcolumn = new DataGridViewTextBoxColumn
            {
                Name = "WPC",
                HeaderText = "WPC",
                ToolTipText = "Work Piece Carrier"
            };
            DGV.Columns.Add(WPCcolumn);
        }

        private void ShowData(List<DataPointsRow<IValueDescription>> data)
        {
            if (data == null) return;
            //Data = data;
            ClearData();
            AddSerialColumn();
            AddWPCColumn();

            var newRows = new List<DataPointsRow<IValueDescription>>();

            foreach (var dataRow in data)
            {
                if (dataRow ==  null) continue;
                newRows.Add(dataRow);
            }

            int maxValuesCount = newRows.Max(row => row.Values.Count);

            for (int i = 0; i < maxValuesCount; i++)
            {
                var valueColumn = new DataGridViewTextBoxColumn
                {
                    Name = newRows.First().Values[i].Name,
                    HeaderText = newRows.First().Values[i].Name,
                    ToolTipText = newRows.First().Values[i].Description
                };
                DGV.Columns.Add(valueColumn);
            }

            foreach (var row in newRows)
            {
                var rowValues = new object[maxValuesCount + 2]; // +1 for the Serial
                rowValues[0] = row.Serial;
                rowValues[1] = row.WPC;
                for (int i = 0; i < row.Values.Count; i++)
                {
                    rowValues[i + 2] = row.Values[i].sValue; // +1 to skip the Serial column
                }
                DGV.Rows.Add(rowValues);
            }
            DGV.Refresh();
        }

        private void FindRow()
        {
            string searchValue = DV.DataViewerInputField.Text; // Assuming DataViewerInputField is your TextBox
            int rowIndex = -1;

            // Use LINQ to find the row where the Serial column matches the searchValue
            DataGridViewRow row = DGV.Rows
                .Cast<DataGridViewRow>()
                .FirstOrDefault(r => r.Cells["Serial"].Value.ToString().Equals(searchValue));

            if (row != null)
            {
                rowIndex = row.Index;
                // Perform your action here, for example, select the row
                DGV.CurrentCell = DGV.Rows[rowIndex].Cells[0];
                DGV.Rows[DGV.CurrentCell.RowIndex].Selected = true;
            }
            else
            {

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
            }
        }

        private void DataViewerSearchButton_Click(object sender, EventArgs e)
        {
            FindRow();
        }

        private void SelectObjectDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolStripComboBox comboBox = sender as ToolStripComboBox;

            if (comboBox.SelectedItem != null && comboBox.SelectedItem.ToString() == "Data Points")
            {
                ShowData(TTLData.DataPoints);
            }

            if (comboBox.SelectedItem != null && comboBox.SelectedItem.ToString() == "Temperature Features")
            {
                ShowData(TTLData.TempFeatures);
            }

            if (comboBox.SelectedItem != null && comboBox.SelectedItem.ToString() == "Pressure Features")
            {
                ShowData(TTLData.PressFeatures);
            }

            if (comboBox.SelectedItem != null && comboBox.SelectedItem.ToString() == "Acoustic Steps Status")
            {
                ShowData(TTLData.StepsStatus);
            }
        }

        private void DataViewerInputField_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (!string.IsNullOrEmpty(DV.DataViewerInputField.Text))
                {
                    FindRow();
                    e.Handled = true;
                }
            }
        }
    }
}
