using System.Windows.Forms;

namespace Opal.src.Forms
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void BufferSize_txb_TextChanged(object sender, System.EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(BufferSize_txb.Text, "[^0-9]"))
            {
                BufferSize_txb.Text = "";
            }
        }
    }
}
