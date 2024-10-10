using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Opal.src.TTL.Containers.Common;
using Opal.src.TTL.Containers.FileContent;
using Opal.src.TTL.Screen;
using Opal.src.Utils;

namespace Opal.src.TTL.UI.UIElements
{
    public class TableView
    {
        public Label Title { get; set; }
        public DataGridView Table { get; set; }
        public CheckBox CheckBox { get; set; }
        public TableLayoutPanel Layout { get; set; }
        public Feature LastSelectedFeature { get; set; }

        public event EventHandler<EventArgs> CheckboxStateChanged;

        private string title;

        public TableView(string title)
        {
            _createLayout(title);
        }

        public TableView()
        {
            _createGeneralLayout();
        }

        public void SetColor(Color color)
        {
            if (color != null)
            {
                Layout.BackColor = color;
                Title.BackColor = color;
            }
        }

        public void AddData(List<Feature> features, Color color, int amount)
        {
            Layout.BackColor = Colors.Black;
            if (features != null && features.Count > 0)
            {
                SetColor(color);

                // Define columns only if they haven't been defined yet
                if (Table.Columns.Count == 0)
                {
                    // Name Column
                    DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn
                    {
                        Name = "Name",
                        DataPropertyName = "Name",
                        HeaderText = "Feature",
                        ReadOnly = true
                    };

                    // Value Column
                    DataGridViewTextBoxColumn valueColumn = new DataGridViewTextBoxColumn
                    {
                        Name = "Value",
                        DataPropertyName = "Value",
                        HeaderText = "Value",
                        ReadOnly = true,
                        DefaultCellStyle = new DataGridViewCellStyle { Format = "N3" } // 3 decimal places
                    };

                    // Add columns to the DataGridView
                    Table.Columns.AddRange(new DataGridViewColumn[] { nameColumn, valueColumn });
                }

                // Convert features to FeatureWithLimits without limits
                var featureWithLimitsList = new List<FeatureWithLimits>();

                foreach (var feature in features)
                {
                    featureWithLimitsList.Add(new FeatureWithLimits
                    {
                        Name = feature.Name,
                        Value = feature.Value,
                        Min = null, // No limits provided
                        Max = null, // No limits provided
                        Description = feature.Description // Ensure Feature has Description
                    });
                }

                // Bind the list to the DataGridView
                Table.DataSource = featureWithLimitsList;
            }
            Title.Text = $"{title}  |  Amt: {amount}";
        }

        public void AddData(List<Feature> features, ProductLimits limits, Color color, int amount)
        {
            if (limits == null)
            {
                AddData(features, color, amount);
                return;
            }

            ClearAll();

            // Define columns only if they haven't been defined yet
            if (Table.Columns.Count == 0)
            {
                // Name Column
                DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn
                {
                    Name = "Name",
                    DataPropertyName = "Name",
                    HeaderText = "Name",
                    ReadOnly = true
                };

                // Min Column
                DataGridViewTextBoxColumn minColumn = new DataGridViewTextBoxColumn
                {
                    Name = "Min",
                    DataPropertyName = "Min",
                    HeaderText = "Min",
                    ReadOnly = true,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "N3" } // 3 decimal places
                };

                // Value Column
                DataGridViewTextBoxColumn valueColumn = new DataGridViewTextBoxColumn
                {
                    Name = "Value",
                    DataPropertyName = "Value",
                    HeaderText = "Value",
                    ReadOnly = true,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "N3" } // 3 decimal places
                };

                // Max Column
                DataGridViewTextBoxColumn maxColumn = new DataGridViewTextBoxColumn
                {
                    Name = "Max",
                    DataPropertyName = "Max",
                    HeaderText = "Max",
                    ReadOnly = true,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "N3" } // 3 decimal places
                };

                // Add columns to the DataGridView
                Table.Columns.AddRange(new DataGridViewColumn[] { nameColumn, minColumn, valueColumn, maxColumn });
            }

            Layout.BackColor = Colors.Black;

            if (features != null && features.Count > 0)
            {
                SetColor(color);
                Clear();

                // Convert features and limits to FeatureWithLimits
                var featureWithLimitsList = new List<FeatureWithLimits>();

                foreach (var feature in features)
                {
                    if (limits.MeanLimits.TryGetValue(feature.Name, out MeanLimit meanLimit))
                    {
                        featureWithLimitsList.Add(new FeatureWithLimits
                        {
                            Name = feature.Name,
                            Min = meanLimit.Min,
                            Value = feature.Value,
                            Max = meanLimit.Max,
                            Description = feature.Description // Ensure Feature has Description
                        });
                    }
                    else
                    {
                        // Features without corresponding limits
                        featureWithLimitsList.Add(new FeatureWithLimits
                        {
                            Name = feature.Name,
                            Min = null, // Represents "N/A"
                            Value = feature.Value,
                            Max = null,  // Represents "N/A"
                            Description = feature.Description // Ensure Feature has Description
                        });
                    }
                }

                // Bind the list to the DataGridView
                Table.DataSource = featureWithLimitsList;
            }

            // Update the title
            Title.Text = $"{title}  |  Amt: {amount}";

            // Attach CellFormatting event handler for conditional styling
            Table.CellFormatting -= Table_CellFormatting; // Prevent multiple subscriptions
            Table.CellFormatting += Table_CellFormatting;
        }

        public void AddData(List<Feature> features, int amount)
        {
            Layout.BackColor = Colors.Black;
            if (features != null && features.Count > 0)
            {
                Table.DataSource = features;
            }
            Title.Text = $"{title}  |  Amt: {amount}";
        }

        public void Clear()
        {
            Table.DataSource = null;
          
            if (CheckBox != null)
            {
                CheckBox.Checked = true;
            }
            Title.Text = "";
        }

        public void ClearAll()
        {
            //DataSource.Clear();
            Table.DataSource = null;
            Table.Rows.Clear();
            Table.Columns.Clear();
            if (CheckBox != null)
            {
                CheckBox.Checked = true;
            }
            Title.Text = "";
            Table.Refresh();
        }

        public void SetTitle(string title)
        {
            Title.Text = title;
        }

        private void _createGeneralLayout()
        {
            Title = CommonElements.Header();
            Table = new DataGridView()
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                Font = Fonts.Sennheiser.SM,
                AutoGenerateColumns = false,
                RowHeadersVisible = false,
                ReadOnly = true
            };

            var titleArea = new TableLayoutPanel()
            {
                ColumnCount = 1,
                RowCount = 1,
                Dock = DockStyle.Fill,
                ColumnStyles =
                {
                    new ColumnStyle(SizeType.Percent, 100)
                },
                RowStyles =
                {
                    new RowStyle(SizeType.Percent, 100)
                }
            };

            Layout = new TableLayoutPanel()
            {
                ColumnCount = 1,
                RowCount = 2,
                Dock = DockStyle.Fill,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 100F) },
                RowStyles =
                {
                    new RowStyle(SizeType.Absolute, 30),
                    new RowStyle(SizeType.Percent, 100F)
                }
            };

            titleArea.SuspendLayout();
            titleArea.Controls.Add(Title, 0, 0);
            titleArea.ResumeLayout();

            Layout.SuspendLayout();
            Layout.Controls.Add(titleArea, 0, 0);
            Layout.Controls.Add(Table, 0, 1);
            Layout.ResumeLayout();
        }

        private void _createLayout(string title)
        {
            Title = CommonElements.Header(title);
            this.title = title;
            Table = new DataGridView()
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                Font = Fonts.Sennheiser.M,
                AutoGenerateColumns = false,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true
            };

            Table.CellMouseEnter += Table_CellMouseEnter;

            var titleArea = new TableLayoutPanel()
            {
                ColumnCount = 2,
                RowCount = 1,
                Dock = DockStyle.Fill,
                ColumnStyles =
                {
                    new ColumnStyle(SizeType.Absolute, 70),
                    new ColumnStyle(SizeType.Percent, 100)
                },
                RowStyles =
                {
                    new RowStyle(SizeType.Percent, 100)
                }
            };

            Layout = new TableLayoutPanel()
            {
                ColumnCount = 1,
                RowCount = 2,
                Dock = DockStyle.Fill,
                //CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 100F) },
                RowStyles =
                {
                    new RowStyle(SizeType.Absolute, 30),
                    new RowStyle(SizeType.Percent, 100F)
                }
            };

            CheckBox = new CheckBox()
            {
                Text = "Show",
                Checked = true,
                Font = Fonts.Sennheiser.SM,
                ForeColor = Colors.White
            };

            CheckBox.CheckedChanged += CheckBox_CheckedChanged;

            titleArea.SuspendLayout();
            titleArea.Controls.Add(CheckBox, 0, 0);
            titleArea.Controls.Add(Title, 1, 0);
            titleArea.ResumeLayout();

            Layout.SuspendLayout();
            Layout.Controls.Add(titleArea, 0, 0);
            Layout.Controls.Add(Table, 0, 1);
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

        private void Table_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < Table.Rows.Count) // Ensure it's a valid row
            {
                var row = Table.Rows[e.RowIndex];
                if (e.ColumnIndex >= 0)
                {
                    // Retrieve the FeatureWithLimits object
                    var feature = row.DataBoundItem as FeatureWithLimits;
                    if (feature != null)
                    {
                        // Set the tooltip text to the Description property or another relevant property
                        Table.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = feature.Description ?? "No Description Available";
                    }
                }
            }
        }

        private void Table_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Ensure we're formatting the correct columns
            string dataProperty = Table.Columns[e.ColumnIndex].DataPropertyName;

            if (dataProperty == "Min" || dataProperty == "Max" || dataProperty == "Name")
            {
                var row = Table.Rows[e.RowIndex];
                var feature = row.DataBoundItem as FeatureWithLimits;

                if (feature != null)
                {
                    if (feature.HasLimits)
                    {
                        if (dataProperty == "Name")
                        {
                            if (feature.Value >= feature.Max)
                            {
                                e.CellStyle.BackColor = Colors.Red;
                            }
                            else if (feature.Value <= feature.Min)
                            {
                                e.CellStyle.BackColor = Colors.Red;
                            }
                            else
                            {
                                e.CellStyle.BackColor = Table.DefaultCellStyle.BackColor;
                            }
                        }
                        else if (dataProperty == "Min")
                        {
                            if (feature.Value <= feature.Min)
                            {
                                e.CellStyle.BackColor = Colors.Red;
                            }
                            else
                            {
                                e.CellStyle.BackColor = Table.DefaultCellStyle.BackColor;
                            }
                        }
                        else if (dataProperty == "Max")
                        {
                            if (feature.Value >= feature.Max)
                            {
                                e.CellStyle.BackColor = Colors.Red;
                            }
                            else
                            {
                                e.CellStyle.BackColor = Table.DefaultCellStyle.BackColor;
                            }
                        }
                    }
                    else
                    {
                        // Features without limits: Color the entire row if desired
                        if (dataProperty == "Name" || dataProperty == "Min" || dataProperty == "Max")
                        {
                            e.CellStyle.BackColor = Color.LightGray;
                        }
                    }
                }
            }

            // Handle "N/A" display for Min and Max
            if (dataProperty == "Min" || dataProperty == "Max")
            {
                if (e.Value == null)
                {
                    e.Value = "N/A";
                    e.FormattingApplied = true;
                }
            }
        }
    }
}