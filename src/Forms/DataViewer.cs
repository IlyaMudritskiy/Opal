using System.Windows.Forms;

namespace Opal.src.Forms
{
    public partial class DataViewer : Form
    {
        public DataViewer()
        {
            InitializeComponent();
            Prepare();
        }

        private void Prepare()
        {
            this.SelectObjectDropDown.Items.AddRange(new object[] {
            "Data Points",
            "Temperature Features",
            "Pressure Features",
            "Acoustic Steps Status"
            });
        }
    }
}
