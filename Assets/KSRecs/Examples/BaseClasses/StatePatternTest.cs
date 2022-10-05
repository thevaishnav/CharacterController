using UnityEngine;
using KSRecs.BaseClasses;

namespace KSRecs.Examples
{
    public class StatePatternTest : StatePattern<StatePatternTest>
    {
        public int integer = 10;
        protected override State<StatePatternTest> GetBaseState() => new SS1();

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Q))
            {
                SwitchState(new SS1());
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                SwitchState(new SSBase());
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                SwitchState(null);
            }
        }
    }


    public class SSBase : State<StatePatternTest>
    {
        public override void OnStateEnter()
        {
            Debug.Log($"Enter W");
        }

        public override void OnStateStay()
        {
            Debug.Log($"Stay W {stateMachine.integer}");
        }

        public override void OnStateExit()
        {
            Debug.Log($"Exit W");
        }
    }


    public class SS1 : State<StatePatternTest>
    {
        public override void OnStateEnter()
        {
            Debug.Log($"Enter Q");
        }

        public override void OnStateStay()
        {
            Debug.Log($"Stay Q {stateMachine.integer}");
        }

        public override void OnStateExit()
        {
            Debug.Log($"Exit Q");
        }
    }
}