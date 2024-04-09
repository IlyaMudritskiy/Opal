using ProcessDashboard.Forms;
using ProcessDashboard.src.Forms;
using ProcessDashboard.src.TTL.Containers.Common;
using ProcessDashboard.src.TTL.Containers.ScreenData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ProcessDashboard.src.TTL.UI.EventControllers
{
    public class DataViewerController
    {
        private MainForm mainForm;
        private DataViewer DV;
        private DataGridView DGV;

        public static List<DataPointsRow> DataPointsRows { get; set; }
        private TTLData TTLData { get; set; }

        public DataViewerController(MainForm mainForm)
        {
            this.mainForm = mainForm;
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

        public void AddDataToTable(List<DataPointsRow> data)
        {
            DGV.Columns.Clear();
            DGV.DataSource = null;

            var serialColumn = new DataGridViewTextBoxColumn
            {
                Name = "Serial",
                HeaderText = "Serial",
                ToolTipText = "Serial number"
            };
            DGV.Columns.Add(serialColumn);

            int maxValuesCount = data.Max(row => row.Values.Count);

            for (int i = 0; i < maxValuesCount; i++)
            {
                var valueColumn = new DataGridViewTextBoxColumn
                {
                    Name = data.First().Values[i].Name,
                    HeaderText = data.First().Values[i].Name,
                    ToolTipText = data.First().Values[i].Descritpion
                };
                DGV.Columns.Add(valueColumn);
            }

            foreach (var row in data)
            {
                var rowValues = new object[maxValuesCount + 1]; // +1 for the Serial
                rowValues[0] = row.Serial;
                for (int i = 0; i < row.Values.Count; i++)
                {
                    rowValues[i + 1] = row.Values[i].ToString(); // +1 to skip the Serial column
                }
                DGV.Rows.Add(rowValues);
            }
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
            DV.DataViewerSearchButton.Click += new EventHandler(DataViewerSearchButton_Click);
            DV.DataViewerInputField.KeyPress += new KeyPressEventHandler(DataViewerInputField_KeyPress);
            DV.FormClosing += new FormClosingEventHandler(DataViewer_FormClosing);
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
            if (DV.ActiveControl == sender)
                FindRow();
        }

        private void SelectObjectDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolStripComboBox comboBox = sender as ToolStripComboBox;

            if (comboBox.SelectedItem != null && comboBox.SelectedItem.ToString() == "Data Points")
            {
                AddDataToTable(TTLData.DataPoints);
            }
        }

        private void DataViewerInputField_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (DV.ActiveControl == sender)
                {
                    FindRow();
                    e.Handled = true;
                }
            }
        }
    }
}
