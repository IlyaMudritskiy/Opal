using ProcessDashboard.src.Model.Data;
using ProcessDashboard.src.Model.Screen;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProcessDashboard.src.Controller
{
    public interface ILineScreen
    {
        bool CheckFiles(List<JsonFile> loadedFiles);
        IScreen GetScreen(List<JsonFile> loadedFiles, ref Panel panel);
    }
}
