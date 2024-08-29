using System.Drawing;
using Opal.Model.Data.Acoustic;
using Opal.src.TTL.Containers.FileContent;

namespace Opal.src.TTL.Containers.ScreenData
{
    public class TTLUnit
    {
        public bool HasAcoustic { get; set; }
        public string ProductID { get; set; }
        public string SerialNumber { get; set; }
        public int TrackNumber { get; set; }
        public int PressNumber { get; set; }
        public string LineID { get; set; }
        public string WPC { get; set; }

        public TTLProcess Process { get; set; }
        public TTLAcoustic Acoustic { get; set; }

        private Color color { get; set; }

        public TTLUnit(ProcessFile process, AcousticFile acoustic = null)
        {
            HasAcoustic = acoustic != null;
            ProductID = process.DUT.TypeID;
            SerialNumber = process.DUT.SerialNumber;
            TrackNumber = int.Parse(process.DUT.TrackNumber);
            PressNumber = int.Parse(process.DUT.PressNumber);
            LineID = process.DUT.MachineID;
            WPC = process.DUT.WPC;

            Process = new TTLProcess(process, color);

            if (HasAcoustic)
                Acoustic = new TTLAcoustic(acoustic);
        }
    }
}
