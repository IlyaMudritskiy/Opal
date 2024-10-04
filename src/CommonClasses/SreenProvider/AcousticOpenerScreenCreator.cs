using Opal.src.TTL.Screen;
using ProcessDashboard.src.CommonClasses.SreenProvider;

namespace Opal.src.CommonClasses.SreenProvider
{
    internal class AcousticOpenerScreenCreator : IScreenCreator<IScreen>
    {
        public string Name => "acoustic";
        public IScreen Create() => AcousticOpenerScreen.Instance;
    }
}
