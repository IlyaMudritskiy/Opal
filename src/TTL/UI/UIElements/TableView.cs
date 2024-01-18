using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ProcessDashboard.src.TTL.Containers.Common;
using ProcessDashboard.src.TTL.Screen;
using ProcessDashboard.src.Utils;

namespace ProcessDashboard.src.TTL.UI.UIElements
{
    public class TableView
    {
        public Label Title { get; set; }
        public DataGridView Table { get; set; }
        public BindingSource DataSource { get; set; }
        public CheckBox CheckBox { get; set; }
        public TableLayoutPanel Layout { get; set; }

        public event EventHandler<EventArgs> CheckboxStateChanged;

        private string title;

        public TableView(string title)
        {
            _createLayout(title);
        }

        public void AddData(List<Feature> features, Color color, int amount)
        {
            Title.BackColor = color;

            foreach (Feature feature in features)
                DataSource.Add(feature);

            Title.Text = $"{title}  |  Amount: {amount}";
        }

        public void Clear()
        {
            DataSource.Clear();
            CheckBox.Checked = true;
            Title.Text = "";
        }

        private void _createLayout(string title)
        {
            Title = CommonElements.Header(title);
            this.title = title;
            Table = CommonElements.DataGridView();
            DataSource = new BindingSource();

            Layout = new TableLayoutPanel()
            {
                ColumnCount = 1,
                RowCount = 3,
                Dock = DockStyle.Fill,
                //CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 100F) },
                RowStyles =
                {
                    new RowStyle(SizeType.Absolute, 30),
                    new RowStyle(SizeType.Absolute, 30),
                    new RowStyle(SizeType.Percent, 100F)
                }
            };

            Table.AutoGenerateColumns = false;

            var cell1 = new DataGridViewTextBoxColumn();
            cell1.DataPropertyName = "ID";
            cell1.HeaderText = "Feature";

            var cell2 = new DataGridViewTextBoxColumn();
            cell2.DataPropertyName = "Value";
            cell2.HeaderText = "Val";

            Table.Columns.Add(cell1);

            Table.Columns.Add(cell2);

            Table.DataSource = DataSource;

            CheckBox = new CheckBox()
            {
                Text = "Show",
                Checked = true,
                Font = Fonts.Sennheiser.M
            };

            CheckBox.CheckedChanged += CheckBox_CheckedChanged;

            Layout.SuspendLayout();
            Layout.Controls.Add(CheckBox, 0, 0);
            Layout.Controls.Add(Title, 0, 1);
            Layout.Controls.Add(Table, 0, 2);
            Layout.ResumeLayout();
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // Raise the custom event when the checkbox state changes
            OnCheckboxStateChanged();
        }

        protected virtual void OnCheckboxStateChanged()
        {
            // Raise the event
            CheckboxStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}