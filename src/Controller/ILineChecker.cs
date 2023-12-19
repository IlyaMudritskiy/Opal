using ProcessDashboard.src.Model.Data;
using ProcessDashboard.src.Model.Screen;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProcessDashboard.src.Controller
{
    public interface ILineScreen
    {
        bool CheckFiles(List<ProcessFile> loadedFiles);
        IScreen GetScreen(List<ProcessFile> loadedFiles, ref Panel panel);
    }
}
