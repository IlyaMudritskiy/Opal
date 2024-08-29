using System.Collections.Generic;
using System.Linq;
using Opal.src.TTL.Misc;

namespace Opal.src.TTL.Containers.ScreenData
{
    public class PassFailUnits
    {
        public List<TTLUnit> Pass { get; set; }
        public List<TTLUnit> Fail { get; set; }
        public List<TTLUnit> FailFR { get; set; }
        public List<TTLUnit> FailTHD { get; set; }
        public List<TTLUnit> FailRNB { get; set; }
        public List<TTLUnit> FailIMP { get; set; }
        public List<TTLUnit> FailOther { get; set; }

        public PassFailUnits(List<TTLUnit> data)
        {
            Pass = data.Where(x => x.Acoustic != null && x.Acoustic.Pass == true).ToList();
            Fail = data.Where(x => x.Acoustic != null && x.Acoustic.Pass == false).ToList();
            FailFR = data.Where(x => x.Acoustic != null && x.Acoustic.FailReasons.Contains(FailReason.FR)).ToList();
            FailTHD = data.Where(x => x.Acoustic != null && x.Acoustic.FailReasons.Contains(FailReason.THD)).ToList();
            FailRNB = data.Where(x => x.Acoustic != null && x.Acoustic.FailReasons.Contains(FailReason.RNB)).ToList();
            FailIMP = data.Where(x => x.Acoustic != null && x.Acoustic.FailReasons.Contains(FailReason.IMP)).ToList();
            FailOther = data.Where(x => x.Acoustic != null && x.Acoustic.FailReasons.Contains(FailReason.Other)).ToList();
        }
    }
}
