using ProcessDashboard.src.Model.Screen;
using ProcessDashboard.src.Model.Screen.TTLine;

namespace ProcessDashboard.src.Controller
{
    public static class ScreenCreator
    {
        public static IScreen GetIScreen(string lineCode)
        {
            switch (lineCode)
            {
                case "TTL_M":
                    return TTLineScreen.Instance;

                default:
                    return null;
            }
        }
    }
}
