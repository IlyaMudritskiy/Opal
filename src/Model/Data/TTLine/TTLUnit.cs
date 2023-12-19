using ProcessDashboard.src.Model.Data.Acoustic;
using ProcessDashboard.src.Model.Data.TTLine.Acoustic;

namespace ProcessDashboard.src.Model.Data.TTLine.Process
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

        public TTLUnit(ProcessFile process, AcousticFile acoustic = null)
        {
            HasAcoustic = acoustic != null;
            ProductID = process.DUT.TypeID;
            SerialNumber = process.DUT.SerialNumber;
            TrackNumber = int.Parse(process.DUT.TrackNumber);
            PressNumber = int.Parse(process.DUT.PressNumber);
            MachineID = process.DUT.MachineID;

            Process = new TTLProcess(process);

            if (HasAcoustic)
                Acoustic = new TTLAcoustic(acoustic);
        }
    }
}
