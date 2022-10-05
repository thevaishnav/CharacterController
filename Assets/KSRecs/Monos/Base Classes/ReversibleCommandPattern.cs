using System.Collections.Generic;
using UnityEngine;

namespace KSRecs.BaseClasses
{
    public abstract class ReversibleCommandPattern<TChildClass> : MonoBehaviour, ICommandPatternBase<TChildClass>
    where TChildClass : ReversibleCommandPattern<TChildClass>
    {
        private Stack<IReversibleCommand<TChildClass>> _undoStack;
        private Stack<IReversibleCommand<TChildClass>> _redoStack;
        protected abstract int GetCapacity();

        protected virtual void Awake()
        {
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
            _redoStack.Clear();
            command.Execute();
        }

        public bool UndoOnce()
        {
            if (_undoStack.Count == 0) return false;
            var action = _undoStack.Pop();
            _redoStack.Push(action);
            action.ExecuteBackwards();
            return true;
        }

        public bool RedoOnce()
        {
            if (_redoStack.Count == 0) return false;
            var action = _redoStack.Pop();
            _undoStack.Push(action);
            action.Execute();
            return true;
        }

        public int UndoMultiple(int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                if (!UndoOnce()) return i - 1;
            }

            return steps;
        }

        public int RedoMultiple(int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                if (!RedoOnce()) return i - 1;
            }
            return steps;
        }
    }
}