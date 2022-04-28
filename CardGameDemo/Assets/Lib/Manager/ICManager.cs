namespace Lib.Manager
{
    public interface ICManager
    {
        void Build();
        void Begin();
        void ReBegin();
        void Register();
        void UnRegister();
        void End();
    }
}