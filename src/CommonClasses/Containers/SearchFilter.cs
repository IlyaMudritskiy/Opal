using Newtonsoft.Json;
using System;

namespace Opal.src.CommonClasses.Containers
{
    public class SearchFilter
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public string Serial { get; set; }

        public string CustomQueryFile { get; set; }

        public string SelectedRbn { get; set; }
    }
}
