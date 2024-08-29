namespace Opal.src.CommonClasses.SreenProvider
{
    public interface IScreenCreator<TBase>
    {
        string Name { get; }
        TBase Create();
    }
}
