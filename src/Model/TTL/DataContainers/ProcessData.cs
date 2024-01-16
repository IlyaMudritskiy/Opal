using System.Collections.Generic;
using System.Linq;
using ProcessDashboard.Model.Data.TTLine;
using ProcessDashboard.Model.Misc;
using ProcessDashboard.src.Model.TTL.DataContainers;
using ScottPlot.Plottable;

namespace ProcessDashboard.Model.TTL.DataContainers
{
    public class ProcessData
    {
        public DSContainer<List<ScatterPlot>> Curves { get; set; }
        public DSContainer<List<Feature>> MeanFeatures { get; set; }
        public DSContainer<List<List<Feature>>> Features { get; set; }

        public ProcessData(List<TTLUnit> units, ProcessStep step)
        {
            if (units == null || units.Count == 0) return;

            SeparateProcessFeatures(units, step);
            AddCurves(units, step);
            CalculateMeanFeatures();
        }

        #region Separate data by DS

        private void SeparateProcessFeatures(List<TTLUnit> units, ProcessStep step)
        {
            Features.DS11 = units
                .Where(x => x.TrackNumber == 1 && x.PressNumber == 1)
                .Select(x => GetStepMeasurements(x, step)).ToList();
            Features.DS12 = units
                .Where(x => x.TrackNumber == 1 && x.PressNumber == 2)
                .Select(x => GetStepMeasurements(x, step)).ToList();
            Features.DS21 = units
                .Where(x => x.TrackNumber == 2 && x.PressNumber == 1)
                .Select(x => GetStepMeasurements(x, step)).ToList();
            Features.DS22 = units
                .Where(x => x.TrackNumber == 2 && x.PressNumber == 2)
                .Select(x => GetStepMeasurements(x, step)).ToList();
        }

        private List<Feature> GetStepMeasurements(TTLUnit unit, ProcessStep step)
        {
            if (step == ProcessStep.Temperature) return unit.Process.TempFeatures;
            if (step == ProcessStep.HighPressure) return unit.Process.PressFeatures;
            else return null;
        }

        #endregion

        #region Add process Curves by DS

        private void AddCurves(List<TTLUnit> units, ProcessStep step)
        {
            Curves.DS11 = units
                .Where(x => x.TrackNumber == 1 && x.PressNumber == 1)
                .Select(x => GetStepCurves(x, step)).ToList();
            Curves.DS12 = units
                .Where(x => x.TrackNumber == 1 && x.PressNumber == 2)
                .Select(x => GetStepCurves(x, step)).ToList();
            Curves.DS21 = units
                .Where(x => x.TrackNumber == 2 && x.PressNumber == 1)
                .Select(x => GetStepCurves(x, step)).ToList();
            Curves.DS22 = units
                .Where(x => x.TrackNumber == 2 && x.PressNumber == 2)
                .Select(x => GetStepCurves(x, step)).ToList();
        }

        private ScatterPlot GetStepCurves(TTLUnit unit, ProcessStep step)
        {
            if (step == ProcessStep.Temperature) return unit.Process.TemperatureCurve;
            if (step == ProcessStep.HighPressure) return unit.Process.PressureCurve;
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
            List<Feature> mean = source[0];

            foreach (var featureList in source)
                for (int i = 1; i < featureList.Count; i++)
                    mean[i] += featureList[i];

            for (int i = 0; i < mean.Count; i++)
                mean[i] /= source.Count;

            return mean;
        }

        #endregion
    }
}
