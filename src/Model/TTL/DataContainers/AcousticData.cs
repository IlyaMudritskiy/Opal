using System.Collections.Generic;
using System.Linq;
using ProcessDashboard.Model.Common;
using ProcessDashboard.Model.Data.TTLine;
using ProcessDashboard.Model.Misc;
using ProcessDashboard.src.Model.TTL.DataContainers;
using ProcessDashboard.Utils;
using ScottPlot.Plottable;

namespace ProcessDashboard.Model.TTL.DataContainers
{
    public class AcousticData
    {
        private DSContainer<List<Measurements2DExt>> SeparatedData { get; set; }
        public DSContainer<Measurements2D> MeanValues { get; set; }

        public DSContainer<List<ScatterPlot>> Curves { get; set; }
        public DSContainer<ScatterPlot> MeanCurves { get; set; }

        public AcousticData(List<TTLUnit> units, ProcessStep step)
        {
            if (units == null || units.Count == 0) return;

            SeparateAcousticMeasurements(units, step);
            AddCurves(units, step);
            CalculateMeanAcoustic();
            AddMeanAcousticCurves();
        }

        #region Separate data by DS

        private void SeparateAcousticMeasurements(List<TTLUnit> units, ProcessStep step)
        {
            SeparatedData.DS11 = units
                .Where(x => x.TrackNumber == 1 && x.PressNumber == 1)
                .Select(x => GetStepMeasurements(x, step)).ToList();
            SeparatedData.DS12 = units
                .Where(x => x.TrackNumber == 1 && x.PressNumber == 2)
                .Select(x => GetStepMeasurements(x, step)).ToList();
            SeparatedData.DS21 = units
                .Where(x => x.TrackNumber == 2 && x.PressNumber == 1)
                .Select(x => GetStepMeasurements(x, step)).ToList();
            SeparatedData.DS22 = units
                .Where(x => x.TrackNumber == 2 && x.PressNumber == 2)
                .Select(x => GetStepMeasurements(x, step)).ToList();
        }

        private Measurements2DExt GetStepMeasurements(TTLUnit unit, ProcessStep step)
        {
            if (step == ProcessStep.FR) return unit.Acoustic.FR;
            if (step == ProcessStep.THD) return unit.Acoustic.THD;
            if (step == ProcessStep.RNB) return unit.Acoustic.RNB;
            if (step == ProcessStep.IMP) return unit.Acoustic.IMP;
            else return null;
        }

        #endregion

        #region Add acoustic Curves by DS

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
            if (step == ProcessStep.FR) return unit.Acoustic.FRCurve;
            if (step == ProcessStep.THD) return unit.Acoustic.THDCurve;
            if (step == ProcessStep.RNB) return unit.Acoustic.RNBCurve;
            if (step == ProcessStep.IMP) return unit.Acoustic.IMPCurve;
            else return null;
        }

        #endregion

        #region Calculate mean Acoustic data

        private void CalculateMeanAcoustic()
        {
            MeanValues.DS11 = CalcMeanAcoustic(SeparatedData.DS11);
            MeanValues.DS12 = CalcMeanAcoustic(SeparatedData.DS12);
            MeanValues.DS21 = CalcMeanAcoustic(SeparatedData.DS21);
            MeanValues.DS22 = CalcMeanAcoustic(SeparatedData.DS22);
        }

        private Measurements2D CalcMeanAcoustic(List<Measurements2DExt> data)
        {
            Measurements2D mean = data[0];

            for (int i = 1; i < data.Count; i++)
                mean += data[i];

            return mean / data.Count;
        }

        #endregion

        #region Add mean Acoustic curves

        private void AddMeanAcousticCurves()
        {
            MeanCurves.DS11 = new ScatterPlot(MeanValues.DS11.X.ToArray(), MeanValues.DS11.Y.ToArray()) { Color = Colors.DS11C };
            MeanCurves.DS12 = new ScatterPlot(MeanValues.DS12.X.ToArray(), MeanValues.DS12.Y.ToArray()) { Color = Colors.DS12C };
            MeanCurves.DS21 = new ScatterPlot(MeanValues.DS21.X.ToArray(), MeanValues.DS21.Y.ToArray()) { Color = Colors.DS21C };
            MeanCurves.DS22 = new ScatterPlot(MeanValues.DS22.X.ToArray(), MeanValues.DS22.Y.ToArray()) { Color = Colors.DS22C };
        }

        #endregion
    }
}
