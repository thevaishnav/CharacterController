namespace KSRecs.BaseClasses
{
    public interface ICommandPatternBase<TChildClass>
        where TChildClass : ICommandPatternBase<TChildClass>
    {
    }


    public interface ICommand<TCommandPatternBase> where TCommandPatternBase : ICommandPatternBase<TCommandPatternBase>
    {
        public void Execute();
    }


    public interface IReversibleCommand<TCommandPatternBase> where TCommandPatternBase : ICommandPatternBase<TCommandPatternBase>
    {
        public void Execute();
        public void ExecuteBackwards();
    }
}