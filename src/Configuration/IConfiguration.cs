namespace ProcessDashboard.src.Configuration
{
    internal interface IConfiguration
    {
        void Create();
        Configuration Read();
        void Write();
        void Refresh();
    }
}
