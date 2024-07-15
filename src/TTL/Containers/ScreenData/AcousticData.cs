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
        private ProcessStep Step { get; set; }
        private List<TTLUnit> PassUnits { get; set; }
        private List<TTLUnit> FailUnits { get; set; }

        #region Line Product Step Identifiers
        /// <summary>
        /// Name of the production line (TTL_M, etc)
        /// </summary>
        public string MachineID { get; set; }

        /// <summary>
        /// ID of the product (588408, 566701, etc)
        /// </summary>
        public string ProductID { get; set; }
        #endregion

        #region Containers with sorted measurements by DS/Nest

        #region PASS Measurements
        /// <summary>
        /// List of measurements with pass/fail for each <b>die-side</b> that <b>passed</b> acoustic test
        /// </summary>
        private DSContainer<List<Measurements2DExt>> DSDataPass { get; set; }
        /// <summary>
        /// List of measurements with pass/fail for each <b>nest (test box)</b> that <b>passed</b> acoustic test
        /// </summary>
        private DSContainer<List<Measurements2DExt>> NestDataPass { get; set; }
        #endregion

        #region FAIL Measurements
        /// <summary>
        /// List of measurements with pass/fail for each <b>die-side</b> that <b>failed</b> acoustic test
        /// </summary>
        private DSContainer<List<Measurements2DExt>> DSDataFail { get; set; }
        /// <summary>
        /// List of measurements with pass/fail for each <b>nest (test box)</b> that <b>failed</b> acoustic test
        /// </summary>
        private DSContainer<List<Measurements2DExt>> NestDataFail { get; set; }
        #endregion

        #region MEAN Measurements
        /// <summary>
        /// Calculated Mean measurement for each <b>die-side</b>
        /// </summary>
        public DSContainer<Measurements2D> MeanDSValues { get; set; }
        /// <summary>
        /// Calculated Mean measurement for each <b>nest (test box)</b>
        /// </summary>
        public DSContainer<Measurements2D> MeanNestValues { get; set; }
        #endregion

        #endregion

        #region Containers with curves sorted by DS/Nest

        #region PASS Curves
        /// <summary>
        /// List of curves for each <b>die-side</b> that <b>passed</b> the test 
        /// </summary>
        public DSContainer<List<ScatterPlot>> DSCurvesPass { get; set; }
        /// <summary>
        /// List of curves for each <b>nest (test box)</b> that <b>passed</b> the test 
        /// </summary>
        public DSContainer<List<ScatterPlot>> NestCurvesPass { get; set; }
        #endregion

        #region FAIL Curves
        /// <summary>
        /// List of curves for each <b>die-side</b> that <b>failed</b> the test 
        /// </summary>
        public DSContainer<List<ScatterPlot>> DSCurvesFail { get; set; }
        /// <summary>
        /// List of curves for each <b>nest (test box)</b> that <b>failed</b> the test 
        /// </summary>
        public DSContainer<List<ScatterPlot>> NestCurvesFail { get; set; }
        #endregion

        #region MEAN Curves
        /// <summary>
        /// Mean curves for each <b>die-side</b>
        /// </summary>
        public DSContainer<ScatterPlot> MeanDSCurves { get; set; }
        /// <summary>
        /// Mean curves for each <b>nest (test box)</b>
        /// </summary>
        public DSContainer<ScatterPlot> MeanNestCurves { get; set; }
        #endregion

        #endregion

        public AcousticData(List<TTLUnit> units, ProcessStep step)
        {
            if (units == null || units.Count == 0) return;

            DSDataPass = new DSContainer<List<Measurements2DExt>>();
            NestDataPass = new DSContainer<List<Measurements2DExt>>();

            DSDataFail = new DSContainer<List<Measurements2DExt>>();
            NestDataFail = new DSContainer<List<Measurements2DExt>>();

            DSCurvesPass = new DSContainer<List<ScatterPlot>>();
            NestCurvesPass = new DSContainer<List<ScatterPlot>>();

            DSCurvesFail = new DSContainer<List<ScatterPlot>>();
            NestCurvesFail = new DSContainer<List<ScatterPlot>>();

            MeanDSValues = new DSContainer<Measurements2D>();
            MeanNestValues = new DSContainer<Measurements2D>();

            MeanDSCurves = new DSContainer<ScatterPlot>();
            MeanNestCurves = new DSContainer<ScatterPlot>();

            PassUnits = units.Where(x => x.Acoustic!= null && x.Acoustic.Pass).ToList();
            FailUnits = units.Where(x => x.Acoustic != null && !x.Acoustic.Pass).ToList();

            Step = step;

            MachineID = units[0].MachineID;
            ProductID = units[0].ProductID;

            SeparateAcousticMeasurementsByDS(DSDataPass, PassUnits);
            SeparateAcousticMeasurementsByDS(DSDataFail, FailUnits);

            SeparateAcousticMeasurementsByNest(NestDataPass, PassUnits);
            SeparateAcousticMeasurementsByNest(NestDataFail, FailUnits);

            AddDSCurves(DSCurvesPass, PassUnits);
            AddDSCurves(DSCurvesFail, FailUnits);

            AddNestCurves(NestCurvesPass, PassUnits);
            AddNestCurves(NestCurvesFail, FailUnits);

            CalculateMeanAcoustic();
            AddMeanAcousticCurves();
        }

        #region Separate data by DS and Nest

        private void SeparateAcousticMeasurementsByDS(DSContainer<List<Measurements2DExt>> target, List<TTLUnit> source)
        {
            target.DS11 = source
                .Where(x => x.TrackNumber == 1 && x.PressNumber == 1)
                .Select(x => GetStepMeasurements(x)).ToList();
            target.DS12 = source
                .Where(x => x.TrackNumber == 1 && x.PressNumber == 2)
                .Select(x => GetStepMeasurements(x)).ToList();
            target.DS21 = source
                .Where(x => x.TrackNumber == 2 && x.PressNumber == 1)
                .Select(x => GetStepMeasurements(x)).ToList();
            target.DS22 = source
                .Where(x => x.TrackNumber == 2 && x.PressNumber == 2)
                .Select(x => GetStepMeasurements(x)).ToList();
        }

        private void SeparateAcousticMeasurementsByNest(DSContainer<List<Measurements2DExt>> target, List<TTLUnit> source)
        {
            target.DS11 = source
                .Where(x => x.Acoustic.Nest == 1)
                .Select(x => GetStepMeasurements(x)).ToList();
            target.DS12 = source
                .Where(x => x.Acoustic.Nest == 2)
                .Select(x => GetStepMeasurements(x)).ToList();
            target.DS21 = source
                .Where(x => x.Acoustic.Nest == 3)
                .Select(x => GetStepMeasurements(x)).ToList();
            target.DS22 = source
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

        private void AddDSCurves(DSContainer<List<ScatterPlot>> target, List<TTLUnit> source)
        {
            // Curves separated by DS
            target.DS11 = source
                .Where(x => x.TrackNumber == 1 && x.PressNumber == 1)
                .Select(x => GetStepCurves(x)).ToList();
            target.DS12 = source
                .Where(x => x.TrackNumber == 1 && x.PressNumber == 2)
                .Select(x => GetStepCurves(x)).ToList();
            target.DS21 = source
                .Where(x => x.TrackNumber == 2 && x.PressNumber == 1)
                .Select(x => GetStepCurves(x)).ToList();
            target.DS22 = source
                .Where(x => x.TrackNumber == 2 && x.PressNumber == 2)
                .Select(x => GetStepCurves(x)).ToList();
        }

        private void AddNestCurves(DSContainer<List<ScatterPlot>> target, List<TTLUnit> source)
        {
            // Curves separated by Nest
            target.DS11 = source
                .Where(x => x.Acoustic.Nest == 1)
                .Select(x => GetStepCurves(x)).ToList();
            target.DS12 = source
                .Where(x => x.Acoustic.Nest == 2)
                .Select(x => GetStepCurves(x)).ToList();
            target.DS21 = source
                .Where(x => x.Acoustic.Nest == 3)
                .Select(x => GetStepCurves(x)).ToList();
            target.DS22 = source
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
            MeanDSValues = calcMeanAcoustic(DSDataPass);
            MeanNestValues = calcMeanAcoustic(NestDataPass);
        }

        private DSContainer<Measurements2D> calcMeanAcoustic(DSContainer<List<Measurements2DExt>> container)
        {
            if (container == null) return null;

            DSContainer<Measurements2D> result = new DSContainer<Measurements2D>();

            for (int i = 0; i < container.Count; i++)
                result.Set(i, calcMeanAcousticPerDS(container.Elements[i]));

            return result;
        }

        private Measurements2D calcMeanAcousticPerDS(List<Measurements2DExt> data)
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
