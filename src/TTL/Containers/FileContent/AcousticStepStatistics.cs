using Opal.src.Utils;
using System.Collections.Generic;
using System.Drawing;

namespace Opal.src.TTL.Containers.FileContent
{
    public class AcousticStepStatistics
    {
        //public string StepName { get; set; }
        public int Count { get; set; }

        public int Pass {  get; set; }

        public int Fail { get; set; }

        public List<StepStat> Measurements { get; set; }

        public List<double[]> UpperLimit { get; set; }

        public List<double[]> LowerLimit { get; set; }
    }

    public class StepStat
    {
        public bool Pass { get; set; }

        public Color Color {
            get
            {
                return Pass ? Colors.Grey : Colors.Red;
            }
        }

        public List<double[]> Measurements { get; set; }
    }
}
