using ProcessDashboard.src.CommonClasses.SreenProvider;

namespace Opal.src.TTL.Screen
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
