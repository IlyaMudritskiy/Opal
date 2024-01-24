using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ProcessDashboard.src.CommonClasses.Containers;
using ProcessDashboard.src.TTL.Containers.Common;
using ProcessDashboard.src.TTL.Containers.FileContent;
using ProcessDashboard.src.TTL.Misc;
using ProcessDashboard.src.Utils;
using ScottPlot.Plottable;

namespace ProcessDashboard.src.TTL.Containers.ScreenData
{
    public class AcousticData
    {
        public string MachineID { get; set; }
        public string ProductID { get; set; }

        private ProcessStep Step { get; set; }

        private DSContainer<List<Measurements2DExt>> DSData { get; set; }
        private DSContainer<List<Measurements2DExt>> NestData { get; set; }

        public DSContainer<List<ScatterPlot>> DSCurves { get; set; }
        public DSContainer<List<ScatterPlot>> NestCurves { get; set; }

        public DSContainer<Measurements2D> MeanDSValues { get; set; }
        public DSContainer<Measurements2D> MeanNestValues { get; set; }

        public DSContainer<ScatterPlot> MeanDSCurves { get; set; }
        public DSContainer<ScatterPlot> MeanNestCurves { get; set; }

        private List<TTLUnit> UnitsWithAcoustic { get; set; }

        public AcousticData(List<TTLUnit> units, ProcessStep step)
        {
            if (units == null || units.Count == 0) return;

            DSData = new DSContainer<List<Measurements2DExt>>();
            NestData = new DSContainer<List<Measurements2DExt>>();

            DSCurves = new DSContainer<List<ScatterPlot>>();
            NestCurves = new DSContainer<List<ScatterPlot>>();

            MeanDSValues = new DSContainer<Measurements2D>();
            MeanNestValues = new DSContainer<Measurements2D>();

            MeanDSCurves = new DSContainer<ScatterPlot>();
            MeanNestCurves = new DSContainer<ScatterPlot>();

            UnitsWithAcoustic = units.Where(x => x.Acoustic != null).ToList();

            Step = step;

            MachineID = units[0].MachineID;
            ProductID = units[0].ProductID;

            SeparateAcousticMeasurementsByDS();
            SeparateAcousticMeasurementsByNest();
            AddCurves();
            CalculateMeanAcoustic();
            AddMeanAcousticCurves();
        }

        #region Separate data by DS and Nest

        private void SeparateAcousticMeasurementsByDS()
        {
            DSData.DS11 = UnitsWithAcoustic
                .Where(x => x.TrackNumber == 1 && x.PressNumber == 1)
                .Select(x => GetStepMeasurements(x)).ToList();
            DSData.DS12 = UnitsWithAcoustic
                .Where(x => x.TrackNumber == 1 && x.PressNumber == 2)
                .Select(x => GetStepMeasurements(x)).ToList();
            DSData.DS21 = UnitsWithAcoustic
                .Where(x => x.TrackNumber == 2 && x.PressNumber == 1)
                .Select(x => GetStepMeasurements(x)).ToList();
            DSData.DS22 = UnitsWithAcoustic
                .Where(x => x.TrackNumber == 2 && x.PressNumber == 2)
                .Select(x => GetStepMeasurements(x)).ToList();
        }

        private void SeparateAcousticMeasurementsByNest()
        {
            NestData.DS11 = UnitsWithAcoustic
                .Where(x => x.Acoustic.Nest == 1)
                .Select(x => GetStepMeasurements(x)).ToList();
            NestData.DS12 = UnitsWithAcoustic
                .Where(x => x.Acoustic.Nest == 2)
                .Select(x => GetStepMeasurements(x)).ToList();
            NestData.DS21 = UnitsWithAcoustic
                .Where(x => x.Acoustic.Nest == 3)
                .Select(x => GetStepMeasurements(x)).ToList();
            NestData.DS22 = UnitsWithAcoustic
                .Where(x => x.Acoustic.Nest == 4)
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

        #region Add acoustic Curves by DS and Nest

        private void AddCurves()
        {
            // Curves separated by DS
            DSCurves.DS11 = UnitsWithAcoustic
                .Where(x => x.TrackNumber == 1 && x.PressNumber == 1)
                .Select(x => GetStepCurves(x)).ToList();
            DSCurves.DS12 = UnitsWithAcoustic
                .Where(x => x.TrackNumber == 1 && x.PressNumber == 2)
                .Select(x => GetStepCurves(x)).ToList();
            DSCurves.DS21 = UnitsWithAcoustic
                .Where(x => x.TrackNumber == 2 && x.PressNumber == 1)
                .Select(x => GetStepCurves(x)).ToList();
            DSCurves.DS22 = UnitsWithAcoustic
                .Where(x => x.TrackNumber == 2 && x.PressNumber == 2)
                .Select(x => GetStepCurves(x)).ToList();

            // Curves separated by Nest
            NestCurves.DS11 = UnitsWithAcoustic
                .Where(x => x.Acoustic.Nest == 1)
                .Select(x => GetStepCurves(x)).ToList();
            NestCurves.DS12 = UnitsWithAcoustic
                .Where(x => x.Acoustic.Nest == 2)
                .Select(x => GetStepCurves(x)).ToList();
            NestCurves.DS21 = UnitsWithAcoustic
                .Where(x => x.Acoustic.Nest == 3)
                .Select(x => GetStepCurves(x)).ToList();
            NestCurves.DS22 = UnitsWithAcoustic
                .Where(x => x.Acoustic.Nest == 4)
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
            MeanDSValues.DS11 = CalcMeanAcoustic(DSData.DS11);
            MeanDSValues.DS12 = CalcMeanAcoustic(DSData.DS12);
            MeanDSValues.DS21 = CalcMeanAcoustic(DSData.DS21);
            MeanDSValues.DS22 = CalcMeanAcoustic(DSData.DS22);

            MeanNestValues.DS11 = CalcMeanAcoustic(NestData.DS11);
            MeanNestValues.DS12 = CalcMeanAcoustic(NestData.DS12);
            MeanNestValues.DS21 = CalcMeanAcoustic(NestData.DS21);
            MeanNestValues.DS22 = CalcMeanAcoustic(NestData.DS22);
        }

        private Measurements2D CalcMeanAcoustic(List<Measurements2DExt> data)
        {
            if (data == null || data.Count == 0) return null;
            int passAmount = 1;
            Measurements2D mean = data[0];

            for (int i = 1; i < data.Count; i++)
                if (data[i].Pass)
                {
                    mean += data[i];
                    passAmount++;
                }
                   
            return mean / passAmount;
        }

        #endregion

        #region Add mean Acoustic curves

        private void AddMeanAcousticCurves()
        {
            // Separated by DS
            if (MeanDSValues.DS11 != null)
                MeanDSCurves.DS11 = GetScatter(MeanDSValues.DS11, Colors.DS11C);

            if (MeanDSValues.DS12 != null)
                MeanDSCurves.DS12 = GetScatter(MeanDSValues.DS12, Colors.DS12C);

            if (MeanDSValues.DS21 != null)
                MeanDSCurves.DS21 = GetScatter(MeanDSValues.DS21, Colors.DS21C);

            if (MeanDSValues.DS22 != null)
                MeanDSCurves.DS22 = GetScatter(MeanDSValues.DS22, Colors.DS22C);

            // Separated by Nest
            if (MeanNestValues.DS11 != null)
                MeanNestCurves.DS11 = GetScatter(MeanNestValues.DS11, Colors.DS11C);

            if (MeanNestValues.DS12 != null)
                MeanNestCurves.DS12 = GetScatter(MeanNestValues.DS12, Colors.DS12C);

            if (MeanNestValues.DS21 != null)
                MeanNestCurves.DS21 = GetScatter(MeanNestValues.DS21, Colors.DS21C);

            if (MeanNestValues.DS22 != null)
                MeanNestCurves.DS22 = GetScatter(MeanNestValues.DS22, Colors.DS22C);
        }

        private ScatterPlot GetScatter(Measurements2D measurement, Color color)
        {
            return new ScatterPlot(ToLogScale(measurement.X), measurement.Y.ToArray())
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
