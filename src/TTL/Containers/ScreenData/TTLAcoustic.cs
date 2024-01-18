using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ProcessDashboard.Model.Data.Acoustic;
using ProcessDashboard.src.CommonClasses.Containers;
using ProcessDashboard.src.TTL.Misc;
using ProcessDashboard.src.Utils;
using ScottPlot.Plottable;

namespace ProcessDashboard.src.TTL.Containers.ScreenData
{
    public class TTLAcoustic
    {
        public bool Pass { get; set; }
        public string Serial { get; set; }
        public int Nest { get; set; }
        public int TrackNumber { get; set; }
        public int PressNumber { get; set; }
        public List<FailReason> FailReasons { get; set; }

        public Measurements2DExt FR { get; set; }
        public Measurements2DExt THD { get; set; }
        public Measurements2DExt RNB { get; set; }
        public Measurements2DExt IMP { get; set; }

        public ScatterPlot FRCurve { get; set; }
        public ScatterPlot THDCurve { get; set; }
        public ScatterPlot RNBCurve { get; set; }
        public ScatterPlot IMPCurve { get; set; }

        public TTLAcoustic(AcousticFile file)
        {
            FailReasons = new List<FailReason>();

            Pass = file.DUT.Pass;
            Serial = file.DUT.Serial;
            Nest = file.DUT.Nest;

            TrackNumber = file.DUT.Track;
            PressNumber = file.DUT.Press;

            FR = getMeasurement(file, "freq");
            THD = getMeasurement(file, "thd");
            RNB = getMeasurement(file, "rnb");
            IMP = getMeasurement(file, "imp");

            FRCurve = new ScatterPlot(FR.X.ToArray(), FR.Y.ToArray()) { Color = GetColor(FR.Pass) };
            THDCurve = new ScatterPlot(THD.X.ToArray(), THD.Y.ToArray()) { Color = GetColor(THD.Pass) };
            RNBCurve = new ScatterPlot(RNB.X.ToArray(), RNB.Y.ToArray()) { Color = GetColor(RNB.Pass) };
            IMPCurve = new ScatterPlot(IMP.X.ToArray(), IMP.Y.ToArray()) { Color = GetColor(IMP.Pass) };

            FindFailReasons();
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

        private void FindFailReasons()
        {
            if (Pass) return;

            if (!FR.Pass) FailReasons.Add(FailReason.FR);
            if (!THD.Pass) FailReasons.Add(FailReason.THD);
            if (!RNB.Pass) FailReasons.Add(FailReason.RNB);
            if (!IMP.Pass) FailReasons.Add(FailReason.IMP);

            if (FailReasons.Count == 0 && !Pass) FailReasons.Add(FailReason.Other);
        }

        private Color GetColor(bool acousticpass)
        {
            if (!Pass) return Colors.Light.Red;
            if (!acousticpass) return Colors.Light.Purple;
            return Colors.Light.Grey;
        }
    }
}
