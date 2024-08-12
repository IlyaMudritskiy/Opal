using Opal.src.TTL.Containers.Common;
using Opal.src.TTL.UI.UIElements;
using ScottPlot.Plottable;
using System.Collections.Generic;

namespace Opal.src.TTL.Containers.ScreenData
{
    public class PlotViewDSContainer : DSContainer<PlotView>
    {
        public void AddScatter(params DSContainer<List<ScatterPlot>>[] plotContainers)
        {
            foreach (var container in plotContainers)
            {
                DS11.AddScatter(container.DS11);
                DS12.AddScatter(container.DS12);
                DS21.AddScatter(container.DS21);
                DS22.AddScatter(container.DS22);
            }
            UpdateElementsList();
        }
    }
}
