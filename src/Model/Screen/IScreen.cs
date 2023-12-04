using ProcessDashboard.src.Model.Data;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProcessDashboard.src.Model.Screen
{
    public interface IScreen
    {
        void Create(ref Panel panel);
        void Update(ref List<JsonFile> data);
        void LoadData(ref List<JsonFile> data);
    }
}
