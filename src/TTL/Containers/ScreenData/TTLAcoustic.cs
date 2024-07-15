using System;
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
        public List<StepStatus> StepsStatus { get; set; }

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
            StepsStatus = new List<StepStatus>();

            Pass = file.DUT.Pass;
            Serial = file.DUT.Serial;
            Nest = file.DUT.Nest;

            TrackNumber = file.DUT.Track;
            PressNumber = file.DUT.Press;

            FR = getMeasurement(file, "fr");
            THD = getMeasurement(file, "thd");
            RNB = getMeasurement(file, "rbz");
            IMP = getMeasurement(file, "imp");

            FRCurve = new ScatterPlot(ToLogScale(FR.X), FR.Y.ToArray())
            {
                Color = GetColor(FR.Pass),
                LineWidth = 1,
                MarkerSize = 0
            };
            THDCurve = new ScatterPlot(ToLogScale(THD.X), THD.Y.ToArray())
            {
                Color = GetColor(THD.Pass),
                LineWidth = 1,
                MarkerSize = 0
            };
            RNBCurve = new ScatterPlot(ToLogScale(RNB.X), RNB.Y.ToArray())
            {
                Color = GetColor(RNB.Pass),
                LineWidth = 1,
                MarkerSize = 0
            };
            IMPCurve = new ScatterPlot(ToLogScale(IMP.X), IMP.Y.ToArray())
            {
                Color = GetColor(IMP.Pass),
                LineWidth = 1,
                MarkerSize = 0
            };

            FindFailReasons();
            AddStepsStatus(file);
        }

        private Measurements2DExt getMeasurement(AcousticFile file, string stepname)
        {
            if (file == null) return null;

            string step = StepNameGetter.GetStepName(stepname);
            
            if (step == "FR")
            {
                var acStep = getASxFR(file);
                return new Measurements2DExt()
                {
                    Pass = acStep.StepPass,
                    X = acStep.Measurement[0].ToList(),
                    Y = acStep.Measurement[1].ToList()
                };
            }

            if (step == "THD")
            {
                var acStep = getASxTHD(file);
                return new Measurements2DExt()
                {
                    Pass = acStep.StepPass,
                    X = acStep.Measurement[0].ToList(),
                    Y = acStep.Measurement[1].ToList()
                };
            }

            AcousticStep content = file.Steps.Where(x => x.StepName == step).FirstOrDefault();

            if (content == null) return null;

            return new Measurements2DExt()
            {
                Pass = content.StepPass,
                X = content.Measurement[0].ToList(),
                Y = content.Measurement[1].ToList()
            };
        }

        private AcousticStep getASxFR(AcousticFile file)
        {
            List<double> freq = new List<double>();
            List<double> dbspl = new List<double>();
            AcousticStep result = new AcousticStep()
            {
                StepName = "FR",
                StepPass = true,
                Measurement = new List<double[]>()
            };

            foreach (var stepData in file.Steps)
            {
                if (stepData.StepName == "FR Low +")
                {
                    freq.AddRange(stepData.Measurement[0]);
                    dbspl.AddRange(stepData.Measurement[1]);
                    result.StepPass = result.StepPass && stepData.StepPass;
                }

                if (stepData.StepName == "FR Low -")
                    result.StepPass = result.StepPass && stepData.StepPass;

                if (stepData.StepName == "FR Low Mid +")
                {
                    freq.AddRange(stepData.Measurement[0]);
                    dbspl.AddRange(stepData.Measurement[1]);
                    result.StepPass = result.StepPass && stepData.StepPass;
                }

                if (stepData.StepName == "FR Low Mid -")
                    result.StepPass = result.StepPass && stepData.StepPass;

                if (stepData.StepName == "FR High Mid +")
                {
                    freq.AddRange(stepData.Measurement[0]);
                    dbspl.AddRange(stepData.Measurement[1]);
                    result.StepPass = result.StepPass && stepData.StepPass;
                }

                if (stepData.StepName == "FR High Mid -")
                    result.StepPass = result.StepPass && stepData.StepPass;

                if (stepData.StepName == "FR High +")
                {
                    freq.AddRange(stepData.Measurement[0]);
                    dbspl.AddRange(stepData.Measurement[1]);
                    result.StepPass = result.StepPass && stepData.StepPass;
                }

                if (stepData.StepName == "FR High -")
                    result.StepPass = result.StepPass && stepData.StepPass;
            }
            result.Measurement.Add(freq.ToArray());
            result.Measurement.Add(dbspl.ToArray());
            return result;
        }

        private AcousticStep getASxTHD(AcousticFile file)
        {
            List<double> freq = new List<double>();
            List<double> thd = new List<double>();

            AcousticStep result = new AcousticStep()
            {
                StepName = "THD",
                StepPass = true,
                Measurement = new List<double[]>()
            };

            foreach (var stepData in file.Steps)
            {
                if (stepData.StepName == "THD Low")
                {
                    freq.AddRange(stepData.Measurement[0]);
                    thd.AddRange(stepData.Measurement[1]);
                    result.StepPass = result.StepPass && stepData.StepPass;
                }
                if (stepData.StepName == "THD Mid")
                {
                    freq.AddRange(stepData.Measurement[0]);
                    thd.AddRange(stepData.Measurement[1]);
                    result.StepPass = result.StepPass && stepData.StepPass;
                }
                if (stepData.StepName == "THD High")
                {
                    freq.AddRange(stepData.Measurement[0]);
                    thd.AddRange(stepData.Measurement[1]);
                    result.StepPass = result.StepPass && stepData.StepPass;
                }
            }
            result.Measurement.Add(freq.ToArray());
            result.Measurement.Add(thd.ToArray());

            return result;
        }

        private void AddStepsStatus(AcousticFile file)
        {
            foreach (var step in file.Steps)
            {
                StepsStatus.Add(new StepStatus
                {
                    StepName = step.StepName,
                    StepPass = step.StepPass,
                });
            }
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
            if (!Pass) return Colors.Red;
            if (!acousticpass) return Colors.Purple;
            return Colors.Light.Grey;
        }

        private double[] ToLogScale(List<double> X)
        {
            return X.Select(xx => Math.Log10(xx)).ToArray();
        }
    }
}
