using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Opal.Forms;
using Opal.src.CommonClasses.Containers;

namespace ProcessDashboard.src.CommonClasses.SreenProvider
{
    public interface IScreen
    {
        void Show(Panel panel);
        void Update(List<JObject> data, MainForm form);
        void Update(JObject data, MainForm form);
        Func<Dictionary<string, TableDataContainer>> GetDVCallback();
        void Clear();
        void ClearAll();
    }
}
