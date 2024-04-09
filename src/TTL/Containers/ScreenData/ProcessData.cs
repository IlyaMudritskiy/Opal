using System.Collections.Generic;
using System.Linq;
using ProcessDashboard.src.TTL.Containers.Common;
using ProcessDashboard.src.TTL.Misc;
using ScottPlot.Plottable;

namespace ProcessDashboard.src.TTL.Containers.ScreenData
{
    public class ProcessData
    {
        public string MachineID { get; set; }
        public string ProductID { get; set; }
        public DSContainer<List<ScatterPlot>> Curves { get; set; }
        public DSContainer<List<Feature>> MeanFeatures { get; set; }
        public DSContainer<List<List<Feature>>> Features { get; set; }

        private ProcessStep Step { get; set; }

        public ProcessData(List<TTLUnit> units, ProcessStep step)
        {
            if (units == null || units.Count == 0) return;
            Curves = new DSContainer<List<ScatterPlot>>();
            MeanFeatures = new DSContainer<List<Feature>>();
            Features = new DSContainer<List<List<Feature>>>();
            Step = step;
            MachineID = units[0].MachineID;
            ProductID = units[0].ProductID;

            SeparateProcessFeatures(units);
            AddCurves(units);
            CalculateMeanFeatures();
        }

        #region Separate data by DS

        private void SeparateProcessFeatures(List<TTLUnit> units)
        {
            Features.DS11 = units
                .Where(x => x.TrackNumber == 1 && x.PressNumber == 1)
                .Select(x => GetStepMeasurements(x)).ToList();
            Features.DS12 = units
                .Where(x => x.TrackNumber == 1 && x.PressNumber == 2)
                .Select(x => GetStepMeasurements(x)).ToList();
            Features.DS21 = units
                .Where(x => x.TrackNumber == 2 && x.PressNumber == 1)
                .Select(x => GetStepMeasurements(x)).ToList();
            Features.DS22 = units
                .Where(x => x.TrackNumber == 2 && x.PressNumber == 2)
                .Select(x => GetStepMeasurements(x)).ToList();
        }

        private List<Feature> GetStepMeasurements(TTLUnit unit)
        {
            if (Step == ProcessStep.Temperature) return unit.Process.TempFeatures;
            if (Step == ProcessStep.HighPressure) return unit.Process.PressFeatures;
            else return null;
        }

        #endregion

        #region Add process Curves by DS

        private void AddCurves(List<TTLUnit> units)
        {
            Curves.DS11 = units
                .Where(x => x.TrackNumber == 1 && x.PressNumber == 1)
                .Select(x => GetStepCurves(x)).ToList();
            Curves.DS12 = units
                .Where(x => x.TrackNumber == 1 && x.PressNumber == 2)
                .Select(x => GetStepCurves(x)).ToList();
            Curves.DS21 = units
                .Where(x => x.TrackNumber == 2 && x.PressNumber == 1)
                .Select(x => GetStepCurves(x)).ToList();
            Curves.DS22 = units
                .Where(x => x.TrackNumber == 2 && x.PressNumber == 2)
                .Select(x => GetStepCurves(x)).ToList();
        }

        private ScatterPlot GetStepCurves(TTLUnit unit)
        {
            if (Step == ProcessStep.Temperature) return unit.Process.TemperatureCurve;
            if (Step == ProcessStep.HighPressure) return unit.Process.PressureCurve;
            else return null;
        }

        #endregion

        #region Calculate mean Features

        private void CalculateMeanFeatures()
        {
            MeanFeatures.DS11 = CalcMeanFeaturesOneDS(Features.DS11);
            MeanFeatures.DS12 = CalcMeanFeaturesOneDS(Features.DS12);
            MeanFeatures.DS21 = CalcMeanFeaturesOneDS(Features.DS21);
            MeanFeatures.DS22 = CalcMeanFeaturesOneDS(Features.DS22);
        }

        private List<Feature> CalcMeanFeaturesOneDS(List<List<Feature>> source)
        {
            if (source == null || source.Count == 0) return null;

            List<Feature> mean = new List<Feature>();

            for (int i = 0; i < source[0].Count; i++)
            {
                int count = 0;
                Feature sum = new Feature();
                foreach (var featureList in source)
                {
                    if (featureList[i].Available)
                    {
                        sum += featureList[i];
                        count++;
                    }
                }
                mean.Add(count == 0 ? sum / 1 : sum / count); // Divide by the number of lists in source to get the mean
            }

            return mean;
        }

        #endregion
    }
}
