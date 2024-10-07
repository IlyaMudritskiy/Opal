using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Opal.src.CommonClasses.Containers;
using Opal.src.TTL.Containers.Common;
using Opal.src.TTL.Containers.FileContent;
using Opal.src.TTL.Processing;
using Opal.src.Utils;
using ScottPlot.Plottable;

namespace Opal.src.TTL.Containers.ScreenData
{
    public class TTLProcess
    {
        public int TrackNumber { get; set; }
        public int PressNumber { get; set; }

        public Measurements2D HighPressure { get; set; }
        public Measurements2D Temperature { get; set; }
        public Heater Heater { get; set; }
        public JSONSinglePoints JsonPoints { get; set; }

        public ScatterPlot TemperatureCurve { get; set; }
        public ScatterPlot PressureCurve { get; set; }

        public List<Feature> TempFeatures { get; set; }
        public List<Feature> PressFeatures { get; set; }
        public List<DataPoint> DataPoints { get; set; }

        //private Color color { get; set; }

        public TTLProcess(ProcessFile file, Color color)
        {
            TempFeatures = new List<Feature>();
            PressFeatures = new List<Feature>();
            DataPoints = new List<DataPoint>();
            Temperature = new Measurements2D();
            HighPressure = new Measurements2D();
            JsonPoints = new JSONSinglePoints(file);

            TrackNumber = int.Parse(file.DUT.TrackNumber);
            PressNumber = int.Parse(file.DUT.PressNumber);

            color = GetColor();

            var temp = file.Steps.Where(x => x.StepName == "ps01_temperature_actual").FirstOrDefault().Measurements;
            var press = file.Steps.Where(x => x.StepName == "ps01_high_pressure_actual").FirstOrDefault().Measurements;

            Temperature.FromMeasurements(temp.Select(x => x.DateTime).ToList(), temp.Select(x => x.MeasurementValue).ToList());
            HighPressure.FromMeasurements(press.Select(x => x.DateTime).ToList(), press.Select(x => x.MeasurementValue).ToList());
            Heater = new Heater(file.Steps.Where(x => x.StepName == "ps01_heater_on").FirstOrDefault().Measurements);

            TemperatureCurve = new ScatterPlot(Temperature.X.ToArray(), Temperature.Y.ToArray()) 
            { 
                Color = color ,
                MarkerSize = 0,
                LineWidth = 1
            };
            PressureCurve = new ScatterPlot(HighPressure.X.ToArray(), HighPressure.Y.ToArray()) 
            { 
                Color = color,
                MarkerSize = 0,
                LineWidth = 1
            };

            FeatureCalculations.Calculate(this);
        }

        private Color GetColor()
        {
            if (TrackNumber == 1 && PressNumber == 1) return Colors.DS11C;
            if (TrackNumber == 1 && PressNumber == 2) return Colors.DS12C;
            if (TrackNumber == 2 && PressNumber == 1) return Colors.DS21C;
            if (TrackNumber == 2 && PressNumber == 2) return Colors.DS22C;
            return Colors.Grey;
        }
    }
}
