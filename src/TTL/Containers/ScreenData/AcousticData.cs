using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ProcessDashboard.src.CommonClasses.Containers;
using ProcessDashboard.src.TTL.Containers.Common;
using ProcessDashboard.src.TTL.Misc;
using ProcessDashboard.src.Utils;
using ScottPlot.Plottable;

namespace ProcessDashboard.src.TTL.Containers.ScreenData
{
    public class AcousticData
    {
        public string MachineID { get; set; }
        public string ProductID { get; set; }
        private DSContainer<List<Measurements2DExt>> SeparatedData { get; set; }
        public DSContainer<Measurements2D> MeanValues { get; set; }

        public DSContainer<List<ScatterPlot>> Curves { get; set; }
        public DSContainer<ScatterPlot> MeanCurves { get; set; }

        private ProcessStep Step { get; set; }

        public AcousticData(List<TTLUnit> units, ProcessStep step)
        {
            if (units == null || units.Count == 0) return;

            SeparatedData = new DSContainer<List<Measurements2DExt>>();
            MeanValues = new DSContainer<Measurements2D>();
            Curves = new DSContainer<List<ScatterPlot>>();
            MeanCurves = new DSContainer<ScatterPlot>();

            Step = step;

            MachineID = units[0].MachineID;
            ProductID = units[0].ProductID;

            SeparateAcousticMeasurements(units);
            AddCurves(units);
            CalculateMeanAcoustic();
            AddMeanAcousticCurves();
        }

        #region Separate data by DS

        private void SeparateAcousticMeasurements(List<TTLUnit> units)
        {
            SeparatedData.DS11 = units
                .Where(x => x.TrackNumber == 1 && x.PressNumber == 1)
                .Where(x => x.Acoustic != null)
                .Select(x => GetStepMeasurements(x)).ToList();
            SeparatedData.DS12 = units
                .Where(x => x.TrackNumber == 1 && x.PressNumber == 2)
                .Where(x => x.Acoustic != null)
                .Select(x => GetStepMeasurements(x)).ToList();
            SeparatedData.DS21 = units
                .Where(x => x.TrackNumber == 2 && x.PressNumber == 1)
                .Where(x => x.Acoustic != null)
                .Select(x => GetStepMeasurements(x)).ToList();
            SeparatedData.DS22 = units
                .Where(x => x.TrackNumber == 2 && x.PressNumber == 2)
                .Where(x => x.Acoustic != null)
                .Select(x => GetStepMeasurements(x)).ToList();
        }

        private Measurements2DExt GetStepMeasurements(TTLUnit unit)
        {
            if (Step == ProcessStep.FR) return unit.Acoustic.FR;
            if (Step == ProcessStep.THD) return unit.Acoustic.THD;
            if (Step == ProcessStep.RNB) return unit.Acoustic.RNB;
            if (Step == ProcessStep.IMP) return unit.Acoustic.IMP;
            else return null;
        }

        #endregion

        #region Add acoustic Curves by DS

        private void AddCurves(List<TTLUnit> units)
        {
            Curves.DS11 = units
                .Where(x => x.TrackNumber == 1 && x.PressNumber == 1)
                .Where(x => x.Acoustic != null)
                .Select(x => GetStepCurves(x)).ToList();
            Curves.DS12 = units
                .Where(x => x.TrackNumber == 1 && x.PressNumber == 2)
                .Where(x => x.Acoustic != null)
                .Select(x => GetStepCurves(x)).ToList();
            Curves.DS21 = units
                .Where(x => x.TrackNumber == 2 && x.PressNumber == 1)
                .Where(x => x.Acoustic != null)
                .Select(x => GetStepCurves(x)).ToList();
            Curves.DS22 = units
                .Where(x => x.TrackNumber == 2 && x.PressNumber == 2)
                .Where(x => x.Acoustic != null)
                .Select(x => GetStepCurves(x)).ToList();
        }

        private ScatterPlot GetStepCurves(TTLUnit unit)
        {
            if (Step == ProcessStep.FR) return unit.Acoustic.FRCurve;
            if (Step == ProcessStep.THD) return unit.Acoustic.THDCurve;
            if (Step == ProcessStep.RNB) return unit.Acoustic.RNBCurve;
            if (Step == ProcessStep.IMP) return unit.Acoustic.IMPCurve;
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
            if (data == null || data.Count == 0) return null;
            Measurements2D mean = data[0];

            for (int i = 1; i < data.Count; i++)
                mean += data[i];

            return mean / data.Count;
        }

        #endregion

        #region Add mean Acoustic curves

        private void AddMeanAcousticCurves()
        {
            if (MeanValues.DS11 != null)
                MeanCurves.DS11 = GetScatter(MeanValues.DS11.X, MeanValues.DS11.Y, Colors.DS11C);

            if (MeanValues.DS12 != null)
                MeanCurves.DS12 = GetScatter(MeanValues.DS12.X, MeanValues.DS12.Y, Colors.DS12C);
         
            if (MeanValues.DS21 != null)
                MeanCurves.DS21 = GetScatter(MeanValues.DS21.X, MeanValues.DS21.Y, Colors.DS21C);

            if (MeanValues.DS22 != null)
                MeanCurves.DS22 = GetScatter(MeanValues.DS22.X, MeanValues.DS22.Y, Colors.DS22C);
        }

        private ScatterPlot GetScatter(List<double> x, List<double> y, Color color)
        {
            return new ScatterPlot(ToLogScale(x), y.ToArray())
            {
                Color = color,
                LineWidth = 2,
                MarkerSize = 0
            };
        }

        #endregion

        private double[] ToLogScale(List<double> X)
        {
            return X.Select(xx => Math.Log10(xx)).ToArray();
        }
    }
}
