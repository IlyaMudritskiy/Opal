using Opal.src.Utils;
using System.Drawing;
using System.Windows.Forms;

namespace Opal.src.TTL.UI.UIElements
{
    public class HeaderView<T> : Control where T : Control
    {
        public Label Title { get; private set; }
        public TableLayoutPanel Layout { get; private set; }
        public T Control { get; private set; }

        public HeaderView()
        {
            Initialize();
        }

        private void Initialize()
        {
            Title = new Label()
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Colors.Black,
                ForeColor = Colors.White,
                Font = Fonts.Sennheiser.ML,
            };

            Layout = new TableLayoutPanel()
            {
                ColumnCount = 1,
                RowCount = 2,
                Dock = DockStyle.Fill,
                BackColor = Colors.Black,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 100) },
                RowStyles =
                {
                    new RowStyle(SizeType.Absolute, 35),
                    new RowStyle(SizeType.Percent, 100)
                }
            };

            Layout.SuspendLayout();
            Layout.Controls.Add(Title, 0, 0);
            Layout.ResumeLayout();
        }

        public void SetColor(Color color)
        {
            Layout.BackColor = color;
            Title.BackColor = color;
        }

        public void AddControl(T control)
        {
            Control = control;
            Layout.SuspendLayout();
            Layout.Controls.Add(control, 0, 1);
            Layout.ResumeLayout();
        }

        public void SetText(string title)
        {
            Title.Text = title;
        }
    }
}
