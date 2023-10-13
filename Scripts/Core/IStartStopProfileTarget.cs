namespace CCN.Core
{
    public interface IStartStopProfileTarget
    {
        public bool IsEnabled { get; }
        public bool TryEnable();
        public bool TryDisable();
    }
}