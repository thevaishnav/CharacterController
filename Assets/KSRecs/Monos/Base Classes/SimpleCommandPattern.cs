using UnityEngine;

namespace KSRecs.BaseClasses
{
    public abstract class SimpleCommandPattern<TChildClass> : MonoBehaviour, ICommandPatternBase<TChildClass>
    where TChildClass : SimpleCommandPattern<TChildClass>
    {
        public void Execute(ICommand<TChildClass> command)
        {
            command.Execute();
        }

    }
}