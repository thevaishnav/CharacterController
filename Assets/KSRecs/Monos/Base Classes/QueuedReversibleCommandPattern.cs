using System.Collections.Generic;
using UnityEngine;

namespace KSRecs.BaseClasses
{
    public abstract class QueuedReversibleCommandPattern<TChildClass> : MonoBehaviour, ICommandPatternBase<TChildClass>
    where TChildClass : QueuedReversibleCommandPattern<TChildClass>
    {
        private Stack<IReversibleCommand<TChildClass>> _undoStack;
        private Stack<IReversibleCommand<TChildClass>> _redoStack;
        private Queue<IReversibleCommand<TChildClass>> _commands;

        private bool hasMachineStarted = false;
        protected abstract int GetCapacity();

        protected virtual void Awake()
        {
            _commands = new Queue<IReversibleCommand<TChildClass>>();


            int cap = GetCapacity();

            if (cap <= 0)
            {
                _undoStack = new Stack<IReversibleCommand<TChildClass>>();
                _redoStack = new Stack<IReversibleCommand<TChildClass>>();
            }
            else
            {
                _undoStack = new Stack<IReversibleCommand<TChildClass>>(cap);
                _redoStack = new Stack<IReversibleCommand<TChildClass>>(cap);
            }
        }

        public void Execute(IReversibleCommand<TChildClass> command)
        {
            _undoStack.Push(command);

            if (hasMachineStarted)
            {
                _commands.Enqueue(command);
                return;
            }

            hasMachineStarted = true;
            command.Execute();
        }

        public void DoneExecution()
        {
            if (_commands.Count == 0) return;
            var container = _commands.Dequeue();
            container.Execute();
        }

        public void UndoOnce()
        {
            if (_undoStack.Count == 0) return;

            var command = _undoStack.Pop();
            _redoStack.Push(command);
            _commands.Enqueue(new _StackedUndoAction_<TChildClass>(command));
        }

        public void RedoOnce()
        {
            if (_redoStack.Count == 0) return;
            var command = _redoStack.Pop();
            _undoStack.Push(command);
            _commands.Enqueue(command);
        }

        public int UndoMultiple(int steps)
        {
            int min = Mathf.Min(steps, _undoStack.Count);
            for (int i = 0; i < min; i++)
            {
                UndoOnce();
            }

            return min;
        }

        public int RedoMultiple(int steps)
        {
            int min = Mathf.Min(steps, _redoStack.Count);
            for (int i = 0; i < min; i++)
            {
                RedoOnce();
            }

            return min;
        }
    }


    public class _StackedUndoAction_<TChildClass> : IReversibleCommand<TChildClass>
        where TChildClass : QueuedReversibleCommandPattern<TChildClass>
    {
        private IReversibleCommand<TChildClass> command;

        public _StackedUndoAction_(IReversibleCommand<TChildClass> command)
        {
            this.command = command;
        }

        public void Execute()
        {
            command.ExecuteBackwards();
        }

        public void ExecuteBackwards()
        {
            command.Execute();
        }
    }
}