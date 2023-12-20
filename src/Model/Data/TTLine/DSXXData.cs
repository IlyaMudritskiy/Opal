using ProcessDashboard.src.Model.Data.Acoustic;
using System.Collections.Generic;
using System.Linq;

namespace ProcessDashboard.src.Model.Data.TTLine
{
    public class DSXXData
    {
        // General information about DieSide
        public int TypeID { get; set; }
        public string LineID { get; set; }
        public int Track { get; set; }
        public int Press { get; set; }
        public int Amount { get; set; }

        // Measurements data
        public List<Measurements> Temperature { get; set; }
        public List<Measurements> Pressure { get; set; }
        public double HoldPressureMean { get; set; }
        public double PrePressureMean { get; set; }
        public double HeaterCurrentMean { get; set; }
        public List<Feature> TempFeaturesMean { get; set; }
        public List<Feature> PressFeaturesMean { get; set; }
        public List<List<DataPoint>> DataPoints { get; set; }

        // Acoustic data
        public List<AcousticFile> AcousticFiles { get; set; }
        public Dictionary<string, Dictionary<string, List<double>>> AcousticMean { get; set; }

        public DSXXData(List<TTLUnitDataOld> DSXX, List<AcousticFile> acousticFiles)
        {
            TypeID = DSXX[0].TypeID;
            LineID = DSXX[0].LineID;
            Track = DSXX[0].Track;
            Press = DSXX[0].Press;
            Amount = DSXX.Count;
            Temperature = DSXX.Select(x => x.Temperature).ToList();
            Pressure = DSXX.Select(x => x.HighPressure).ToList();
            //HoldPressureMean = DSXX.Average(x => x.HoldPressure);
            //PrePressureMean = DSXX.Average(x => x.PrePressure);
            //HeaterCurrentMean = DSXX.Average(x => x.HeaterCurrent);
            TempFeaturesMean = DSXX
                .SelectMany(unit => unit.TempFeatures)
                .GroupBy(feature => feature.ID)
                .Select(group => new Feature()
                {
                    ID = group.Key,
                    Name = group.First().Name,
                    Description = group.First().Description,
                    Value = group.Average(x => x.Value)
                }).ToList();
            PressFeaturesMean = DSXX
                .SelectMany(unit => unit.PressFeatures)
                .GroupBy(feature => feature.ID)
                .Select(group => new Feature()
                {
                    ID = group.Key,
                    Name = group.First().Name,
                    Description = group.First().Description,
                    Value = group.Average(x => x.Value)
                }).ToList();
            //DataPoints = new List<List<DataPoint>>();
            //foreach(var ds in DSXX)
            {
                //DataPoints.Add(ds.DataPoints);
            }
            AcousticFiles = acousticFiles;
        }
    }
}
