using System.Windows.Forms;

namespace ProcessDashboard.src.Model.Screen
{
    public interface IScreen
    {
        void Create(ref Panel panel);
        void Update();
        void LoadData<T>(T data);
        void Hide(ref Panel panel);
    }
}
