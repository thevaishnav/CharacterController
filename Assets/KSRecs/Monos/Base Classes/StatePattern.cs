using UnityEngine;

namespace KSRecs.BaseClasses
{
    public abstract class StatePattern<TChildClass> : MonoBehaviour
    where TChildClass : StatePattern<TChildClass>
    {
        private State<TChildClass> ActiveState;

        protected abstract State<TChildClass> GetBaseState();

        protected virtual void Awake()
        {
            SwitchState(GetBaseState());
        }

        protected virtual void Update()
        {
            if (ActiveState != null)
            {
                ActiveState.OnStateStay();
            }
        }

        public void SwitchState(State<TChildClass> newState)
        {
            if (ActiveState != null)
            {
                ActiveState.OnStateExit();
            }
            ActiveState = newState;
            if (ActiveState != null)
            {
                ActiveState.stateMachine = this as TChildClass;
                ActiveState.OnStateEnter();
            }
        }

    }


    public abstract class State<TStatePattern> where TStatePattern : StatePattern<TStatePattern>
    {
        public TStatePattern stateMachine;

        public virtual void OnStateEnter()
        {
        }

        public virtual void OnStateStay()
        {
        }

        public virtual void OnStateExit()
        {
        }
    }
}