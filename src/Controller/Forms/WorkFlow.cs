using ProcessDashboard.src.Data;
using ProcessDashboard.src.Model.Data;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProcessDashboard.src.Controller.Forms
{
    public static class WorkFlow
    {
        public static void LoadFiles(CommonDialog dialog)
        {
            List<TransducerData> loadedData = EmbossingDataProcessor.OpenFiles(dialog);
        }

        public static void GetProcessSteps()
        {

        }

        public static void GetProcessScreen()
        {
            
        }
    }
}
