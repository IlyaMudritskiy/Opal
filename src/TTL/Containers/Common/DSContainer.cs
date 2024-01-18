using System;

namespace ProcessDashboard.src.TTL.Containers.Common
{
    public class DSContainer<T>
    {
        public T DS11 { get; set; }
        public T DS12 { get; set; }
        public T DS21 { get; set; }
        public T DS22 { get; set; }

        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public DSContainer()
        {
            DS11 = default;
            DS12 = default;
            DS21 = default;
            DS22 = default;
        }

        public DSContainer(params T[] ds)
        {
            if (ds == null || ds.Length != 4)
            {
                Log.Trace($"Length of params is [{ds.Length}] instead of [4]");
                return;
            }

            DS11 = ds[0];
            DS12 = ds[1];
            DS21 = ds[2];
            DS22 = ds[3];
        }
    }
}
