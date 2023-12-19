using ProcessDashboard.src.Model.Common;
using ProcessDashboard.src.Model.Data.Acoustic;
using System.Linq;

namespace ProcessDashboard.src.Model.Data.TTLine.Acoustic
{
    public class TTLAcoustic
    {
        public bool Pass { get; set; }
        public string Serial { get; set; }
        public int Nest { get; set; }
        
        public Measurements2DExt FR { get; set; }
        public Measurements2DExt THD { get; set; }
        public Measurements2DExt RNB { get; set; }
        public Measurements2DExt IMP { get; set; }

        public TTLAcoustic(AcousticFile file)
        {
            Pass = file.DUT.Pass;
            Serial = file.DUT.Serial;
            Nest = file.DUT.Nest;

            FR = getMeasurement(file, "freq");
            THD = getMeasurement(file, "thd");
            RNB = getMeasurement(file, "rnb");
            IMP = getMeasurement(file, "imp");
        }

        private Measurements2DExt getMeasurement(AcousticFile file, string stepname)
        {
            var content = file.Steps.Where(x => x.StepName == stepname).FirstOrDefault();

            return new Measurements2DExt()
            {
                Pass = content.StepPass,
                X = content.Measurement[0].ToList(),
                Y = content.Measurement[1].ToList()
            };
        }
    }
}
