using ProcessDashboard.src.Model.Data;
using ProcessDashboard.src.Model.Screen;
using ProcessDashboard.src.Model.Screen.TTLine;
using System.Collections.Generic;

namespace ProcessDashboard.src.Controller
{
    public static class ScreenCreator
    {
        public static IScreen GetIScreen(ref List<JsonFile> selectedFiles)
        {
            string line = checkLine(ref selectedFiles);
            string product = checkProduct(ref selectedFiles);

            switch (line)
            {
                case "TTL_M":
                    return TTLineScreen.Instance;

                default:
                    return null;
            }
        }

        private static string checkLine(ref List<JsonFile> selectedFiles)
        {
            string line = "";
            int counter = selectedFiles.Count;

            foreach (var f in selectedFiles)
            {
                if (line == "")
                    line = f.DUT.MachineID;

                if (line != f.DUT.MachineID)
                    return "";
                else
                    counter--;
            }
            return counter == 0 ? line : "";
        }

        private static string checkProduct(ref List<JsonFile> selectedFiles)
        {
            string product = "";
            int counter = selectedFiles.Count;

            foreach (var f in selectedFiles)
            {
                if (product == "")
                    product = f.DUT.TypeID;

                if (product != f.DUT.TypeID)
                    return "";
                else
                    counter--;
            }
            return counter == 0 ? product : "";
        }
    }
}
