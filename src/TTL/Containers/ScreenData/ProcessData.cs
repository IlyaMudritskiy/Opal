using System.Collections.Generic;
using System.Linq;
using Opal.Model.AppConfiguration;
using Opal.src.TTL.Containers.Common;
using Opal.src.TTL.Misc;
using ScottPlot.Plottable;

namespace Opal.src.TTL.Containers.ScreenData
{
    public class ProcessData
    {
        public string LineID { get; set; }
        public string ProductID { get; set; }
        public DSContainer<List<ScatterPlot>> Curves { get; set; }
        public DSContainer<List<Feature>> MeanFeatures { get; set; }
        public DSContainer<List<List<Feature>>> Features { get; set; }

        private ProcessStep Step { get; set; }

        private Config _config = Config.Instance;

        public ProcessData()
        {
            Curves = new DSContainer<List<ScatterPlot>>();
            MeanFeatures = new DSContainer<List<Feature>>();
            Features = new DSContainer<List<List<Feature>>>();
        }

        public void AddData(List<TTLUnit> units, ProcessStep step)
        {
            if (units == null || units.Count == 0) return;

            Step = step;
            LineID = units[0].LineID;
            ProductID = units[0].ProductID;

            SeparateProcessFeatures(units);
            AddCurves(units);
            CalculateMeanFeatures();
            //GetDataPoints(units);
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
            return null;
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

        #region Update single unit

        public void UpdateData(TTLUnit unit, ProcessStep step)
        {
            if (!CheckUnit(unit)) return;
            Step = step;

            LineID = _config.LineID;
            ProductID = _config.ProductID;

            var ds = $"DS{unit.TrackNumber}{unit.PressNumber}";

            switch (ds)
            {
                case "DS11":
                    Curves.DS11 = GetCurvesList(GetStepCurves(unit));
                    MeanFeatures.DS11 = GetMeanFeaturesList(unit);
                    Features.DS11 = GetFeaturesList(unit);
                    break;

                case "DS12":
                    Curves.DS12 = GetCurvesList(GetStepCurves(unit));
                    MeanFeatures.DS12 = GetMeanFeaturesList(unit);
                    Features.DS12 = GetFeaturesList(unit);
                    break;

                case "DS21":
                    Curves.DS21 = GetCurvesList(GetStepCurves(unit));
                    MeanFeatures.DS21 = GetMeanFeaturesList(unit);
                    Features.DS21 = GetFeaturesList(unit);
                    break;

                case "DS22":
                    Curves.DS22 = GetCurvesList(GetStepCurves(unit));
                    MeanFeatures.DS22 = GetMeanFeaturesList(unit);
                    Features.DS22 = GetFeaturesList(unit);
                    break;
            }
        }

        private bool CheckUnit(TTLUnit unit)
        {
            if (unit == null) return false;

            if (LineID == null) LineID = unit.LineID;
            if (unit.LineID != LineID) return false;

            if (ProductID == null) ProductID = unit.ProductID;
            if (unit.ProductID != ProductID) return false;

            return true;
        }

        private List<ScatterPlot> GetCurvesList(ScatterPlot plot)
        {
            return new List<ScatterPlot> { plot };
        }

        private List<Feature> GetMeanFeaturesList(TTLUnit unit)
        {
            if (Step == ProcessStep.Temperature)
                return unit.Process.TempFeatures;

            if (Step == ProcessStep.HighPressure)
                return unit.Process.PressFeatures;

            return new List<Feature>();
        }

        private List<List<Feature>> GetFeaturesList(TTLUnit unit)
        {
            if (Step == ProcessStep.Temperature)
                return new List<List<Feature>> { unit.Process.TempFeatures };

            if (Step == ProcessStep.HighPressure)
                return new List<List<Feature>> { unit.Process.PressFeatures };

            return new List<List<Feature>>();
        }

        #endregion
    }
}
