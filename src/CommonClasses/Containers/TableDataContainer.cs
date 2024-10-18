using System.Collections.Generic;

namespace Opal.src.CommonClasses.Containers
{
    public class TableDataContainer
    {
        public List<string> ColumnsNames { get; set; }
        public List<List<string>> DataRows { get; set; }

        public TableDataContainer(List<string> columnNames, List<List<string>> values)
        {
            ColumnsNames = columnNames;
            DataRows = values;
        }

        public bool IsValid()
        {
            foreach (var row in DataRows)
            {
                if (row.Count != ColumnsNames.Count)
                    return false;
            }

            return true;
        }
    }
}
