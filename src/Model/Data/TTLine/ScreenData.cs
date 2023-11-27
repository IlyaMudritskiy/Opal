﻿using System.Collections.Generic;
using System.Linq;

namespace ProcessDashboard.src.Model.Data.TTLine
{
    public class ScreenData
    {
        public DSXXData DS11 { get; set; }
        public DSXXData DS12 { get; set; }
        public DSXXData DS21 { get; set; }
        public DSXXData DS22 { get; set; }

        public ScreenData(List<TTLUnitData> data)
        {
            try
            {
                DS11 = new DSXXData(data.Where(x => x.Track == 1 && x.Press == 1).ToList());
            }
            catch { }

            try
            {
                DS12 = new DSXXData(data.Where(x => x.Track == 1 && x.Press == 2).ToList());
            }
            catch { }
            try
            {
                DS21 = new DSXXData(data.Where(x => x.Track == 2 && x.Press == 1).ToList());
            }
            catch { }
            try
            {
                DS22 = new DSXXData(data.Where(x => x.Track == 2 && x.Press == 2).ToList());
            }
            catch { }
        }
    }
}