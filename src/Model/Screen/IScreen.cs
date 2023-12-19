using Newtonsoft.Json.Linq;
using ProcessDashboard.src.Model.Data;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProcessDashboard.src.Model.Screen
{
    public interface IScreen
    {
        void Create(ref Panel panel, ref List<JObject> files);
        void Update(ref List<ProcessFile> data);
        void LoadData(ref List<ProcessFile> data);
    }
}
