namespace ProcessDashboard.src.Model.Data.Embossing
{
    public class ProcessStep
    {
        public string StepName { get; set; }
        public string UnitX { get; set; }
        public string UnitY { get; set; }
        public Measurements Measurements { get; set; }

        public ProcessStep(Step step)
        {
            StepName = step.StepName;
            UnitX = step.UnitX;
            UnitY = step.UnitY;
            Measurements = new Measurements(step.Measurements);
        }
    }
}
