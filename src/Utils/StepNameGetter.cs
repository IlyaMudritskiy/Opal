using ProcessDashboard.Model.AppConfiguration;

namespace ProcessDashboard.src.Utils
{
    public static class StepNameGetter
    {
        
        public static string GetStepName(string stepName)
        {
            Config config = Config.Instance;

            if (config.IsASxReports)
                return getNewName(stepName);
            return getOldName(stepName);
        }

        private static string getNewName(string name)
        {
            switch (name)
            {
                case "fr":
                    return "FR";
                case "thd":
                    return "THD";
                case "rbz":
                    return "Rub n Buzz";
                case "imp":
                    return "Impedance";
                default:
                    return name;
            }
        }

        private static string getOldName(string name)
        {
            switch (name)
            {
                case "fr":
                    return "freq";
                case "thd":
                    return "thd";
                case "rbz":
                    return "rnb";
                case "imp":
                    return "imp";
                default:
                    return name;
            }
        }
    }
}
