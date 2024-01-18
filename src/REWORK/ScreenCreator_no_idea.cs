using ProcessDashboard.src.CommonClasses;
using ProcessDashboard.src.TTL.Screen;

namespace ProcessDashboard.Controller
{
    public static class ScreenCreator
    {
        public static IScreen GetIScreen(string lineCode)
        {
            switch (lineCode)
            {
                case "TTL_M":
                    return TTLScreen.Instance;

                default:
                    return null;
            }
        }
    }
}
