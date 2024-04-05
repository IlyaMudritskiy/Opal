using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace ProcessDashboard.src.CommonClasses
{
    public interface IScreen
    {
        void Create(ref Panel panel);
        void Update(List<JObject> data);
        void LoadData(List<JObject> data);
        void Clear();
    }
}
