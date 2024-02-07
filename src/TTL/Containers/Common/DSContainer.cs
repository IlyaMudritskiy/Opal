using System;
using System.Collections.Generic;

namespace ProcessDashboard.src.TTL.Containers.Common
{
    public class DSContainer<T>
    {
        public T DS11 { get; set; }
        public T DS12 { get; set; }
        public T DS21 { get; set; }
        public T DS22 { get; set; }
        public List<T> Elements { get; private set; }

        public DSContainer()
        {
            DS11 = default;
            DS12 = default;
            DS21 = default;
            DS22 = default;
        }

        public void Apply(Action<T> action)
        {
            Elements = new List<T> { DS11, DS12, DS21, DS22 };
            if (action == null) return;

            foreach (var element in Elements)
                action(element);
        }
    }
}
