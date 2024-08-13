using Opal.src.TTL.Screen;
using ProcessDashboard.src.CommonClasses.SreenProvider;

namespace Opal.src.CommonClasses.SreenProvider
{
    public class TTLScreenCreator : IScreenCreator<IScreen>
    {
        public string Name => "TTL_M";

        public IScreen Create() => TTLScreen.Instance;
    }
}
