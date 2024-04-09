using ProcessDashboard.src.TTL.Containers.FileContent;
using System;
using System.Linq;

namespace ProcessDashboard.src.TTL.Containers.ScreenData
{
    public class JSONSinglePoints
    {
        // PS01_t2 is Heater = True (on) from old Json
        public SinglePointMeasurement PS01_t3 { get; set; }
        // PS01_t4 is Heater = False (off) from old Json
        public SinglePointMeasurement PS01_t5 { get; set; }
        public SinglePointMeasurement PS01_t6 { get; set; }
        public SinglePointMeasurement PS01_t7 { get; set; }
        public SinglePointMeasurement PS01_t8 { get; set; }
        public SinglePointMeasurement PS01_t9 { get; set; }
        public SinglePointMeasurement PS01_t10 { get; set; }
        public SinglePointMeasurement PS01_HeaterRuntime { get; set; }

        public JSONSinglePoints(ProcessFile file)
        {
            PS01_t3 = getPoint(file, "ps01_t3");
            PS01_t5 = getPoint(file, "ps01_t5");
            PS01_t6 = getPoint(file, "ps01_t6");
            PS01_t7 = getPoint(file, "ps01_t7");
            PS01_t8 = getPoint(file, "ps01_t8");
            PS01_t9 = getPoint(file, "ps01_t9");
            PS01_t10 = getPoint(file, "ps01_t10");
            PS01_HeaterRuntime = getPoint(file, "ps01_heater_runtime");
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
            PS01_HeaterRuntime.Offset(processStart);
        }

        private SinglePointMeasurement getPoint(ProcessFile file, string stepname)
        {
            var step = file.Steps.Where(x => x.StepName == stepname).FirstOrDefault();
            if (step == null)
            {
                return new SinglePointMeasurement();
            }
            return new SinglePointMeasurement(step);
        }
    }
}
