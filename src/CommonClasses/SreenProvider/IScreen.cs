using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace ProcessDashboard.src.CommonClasses.SreenProvider
{
    public interface IScreen
    {
        void Show(ref Panel panel);
        void Update(List<JObject> data);
        void Clear();
    }
}
