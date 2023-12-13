using System.Drawing;
using System.Windows.Forms;

namespace ProcessDashboard.src.Model.Screen
{
    public abstract class AbsTab
    {
        public TabPage Tab { get; set; }
        public Label Title { get; set; }
        protected abstract void createLayout(string title);
        public abstract void AddScatter(int track, int press, double[] x, double[] y, Color color, string flag = "");
        public abstract void Clear();
    }
}
