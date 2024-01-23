using System.Drawing;
using ProcessDashboard.Model.Data.Acoustic;
using ProcessDashboard.src.TTL.Containers.FileContent;
using ProcessDashboard.src.Utils;

namespace ProcessDashboard.src.TTL.Containers.ScreenData
{
    public class TTLUnit
    {
        public bool HasAcoustic { get; set; }
        public string ProductID { get; set; }
        public string SerialNumber { get; set; }
        public int TrackNumber { get; set; }
        public int PressNumber { get; set; }
        public string MachineID { get; set; }

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
            MachineID = process.DUT.MachineID;

            Process = new TTLProcess(process, color);

            if (HasAcoustic)
                Acoustic = new TTLAcoustic(acoustic);
        }

        private void GetColor()
        {
            if (TrackNumber == 1 && PressNumber == 1) color = Colors.DS11C;
            if (TrackNumber == 1 && PressNumber == 2) color = Colors.DS12C;
            if (TrackNumber == 2 && PressNumber == 1) color = Colors.DS21C;
            if (TrackNumber == 2 && PressNumber == 2) color = Colors.DS22C;
        }
    }
}
