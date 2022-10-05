using System;
using UnityEngine;

namespace KSRecs.BaseClasses
{
    public abstract class SimpleStatePattern : MonoBehaviour
    {
        private Action ActiveState;

        protected abstract void BaseState();

        protected virtual void Awake()
        {
            ActiveState = BaseState;
        }

        protected virtual void Update()
        {
            ActiveState?.Invoke();
        }

        public void SwitchState(Action newState)
        {
            ActiveState = newState;
        }
    }


    public abstract class SimpleStatePattern<T> : MonoBehaviour
    {
        private Action<T> ActiveState;
        private T arg0;

        protected abstract void BaseState(T arg);

        protected virtual void Awake()
        {
            ActiveState = BaseState;
        }

        protected virtual void Update()
        {
            ActiveState?.Invoke(arg0);
        }

        public void SwitchState(Action<T> newState)
        {
            ActiveState = newState;
        }

        public void SetArg(T arg)
        {
            this.arg0 = arg;
        }
    }


    public abstract class SimpleStatePattern<T0, T1> : MonoBehaviour
    {
        private Action<T0, T1> ActiveState;
        private T0 arg0;
        private T1 arg1;

        protected abstract void BaseState(T0 arg0, T1 arg1);

        protected virtual void Awake()
        {
            ActiveState = BaseState;
        }

        protected virtual void Update()
        {
            ActiveState?.Invoke(arg0, arg1);
        }

        public void SwitchState(Action<T0, T1> newState)
        {
            ActiveState = newState;
        }

        public void SetArg0(T0 arg)
        {
            this.arg0 = arg;
        }

        public void SetArg1(T1 arg)
        {
            this.arg1 = arg;
        }
    }


    public abstract class SimpleStatePattern<T0, T1, T2> : MonoBehaviour
    {
        private Action<T0, T1, T2> ActiveState;
        private T0 arg0;
        private T1 arg1;
        private T2 arg2;

        protected abstract void BaseState(T0 arg0, T1 arg1, T2 arg2);

        protected virtual void Awake()
        {
            ActiveState = BaseState;
        }

        protected virtual void Update()
        {
            ActiveState?.Invoke(arg0, arg1, arg2);
        }

        public void SwitchState(Action<T0, T1, T2> newState)
        {
            ActiveState = newState;
        }

        public void SetArg0(T0 arg)
        {
            this.arg0 = arg;
        }

        public void SetArg1(T1 arg)
        {
            this.arg1 = arg;
        }

        public void SetArg2(T2 arg)
        {
            this.arg2 = arg;
        }
    }


    public abstract class SimpleStatePattern<T0, T1, T2, T3> : MonoBehaviour
    {
        private Action<T0, T1, T2, T3> ActiveState;
        private T0 arg0;
        private T1 arg1;
        private T2 arg2;
        private T3 arg3;

        protected abstract void BaseState(T0 arg0, T1 arg1, T2 arg2, T3 arg3);

        protected virtual void Awake()
        {
            ActiveState = BaseState;
        }

        protected virtual void Update()
        {
            ActiveState?.Invoke(arg0, arg1, arg2, arg3);
        }

        public void SwitchState(Action<T0, T1, T2, T3> newState)
        {
            ActiveState = newState;
        }

        public void SetArg0(T0 arg)
        {
            this.arg0 = arg;
        }

        public void SetArg1(T1 arg)
        {
            this.arg1 = arg;
        }

        public void SetArg2(T2 arg)
        {
            this.arg2 = arg;
        }

        public void SetArg3(T2 arg)
        {
            this.arg2 = arg;
        }
    }
}