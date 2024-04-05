using ProcessDashboard.src.TTL.Containers.FileContent;
using System;
using System.Linq;

namespace ProcessDashboard.src.TTL.Containers.ScreenData
{
    public class JSONPoints
    {
        // PS01_t2 is Heater = True (on) from old Json
        public JSONPoint PS01_t3 { get; set; }
        // PS01_t4 is Heater = False (off) from old Json
        public JSONPoint PS01_t5 { get; set; }
        public JSONPoint PS01_t6 { get; set; }
        public JSONPoint PS01_t7 { get; set; }
        public JSONPoint PS01_t8 { get; set; }
        public JSONPoint PS01_t9 { get; set; }
        public JSONPoint PS01_t10 { get; set; }

        public JSONPoints(ProcessFile file)
        {
            PS01_t3 = getPoint(file, "ps01_t3");
            PS01_t5 = getPoint(file, "ps01_t5");
            PS01_t6 = getPoint(file, "ps01_t6");
            PS01_t7 = getPoint(file, "ps01_t7");
            PS01_t8 = getPoint(file, "ps01_t8");
            PS01_t9 = getPoint(file, "ps01_t9");
            PS01_t10 = getPoint(file, "ps01_t10");
        }

        public void SetOffset(DateTime processStart)
        {
            PS01_t3.Offset(processStart);
            PS01_t5.Offset(processStart);
            PS01_t6.Offset(processStart);
            PS01_t7.Offset(processStart);
            PS01_t8.Offset(processStart);
            PS01_t9.Offset(processStart);
            PS01_t10.Offset(processStart);
        }

        private JSONPoint getPoint(ProcessFile file, string stepname)
        {
            return new JSONPoint(file.Steps.Where(x => x.StepName == stepname).FirstOrDefault());
        }
    }
}
