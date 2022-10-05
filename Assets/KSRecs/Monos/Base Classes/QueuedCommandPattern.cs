using System.Collections.Generic;
using UnityEngine;


namespace KSRecs.BaseClasses
{
    public abstract class QueuedCommandPattern<TChildClass> : MonoBehaviour, ICommandPatternBase<TChildClass>
    where TChildClass : QueuedCommandPattern<TChildClass>
    {

        private Queue<ICommand<TChildClass>> _commands;

        protected virtual void Awake()
        {
            _commands = new Queue<ICommand<TChildClass>>();
        }

        public void Execute(ICommand<TChildClass> command)
        {
            if (_commands.Count == 0)
            {
                command.Execute();
            }
            else
            {
                _commands.Enqueue(command);
            }
        }

        public void DoneExecution()
        {
            if (_commands.Count == 0) return;
            var command = _commands.Dequeue();
            command.Execute();
        }
    }
}