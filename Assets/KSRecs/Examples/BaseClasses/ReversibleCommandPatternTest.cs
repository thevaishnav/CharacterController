using UnityEngine;
using KSRecs.BaseClasses;

namespace KSRecs.Examples
{
    public class ReversibleCommandPatternTest : ReversibleCommandPattern<ReversibleCommandPatternTest>
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A)) { Execute(new CommandMoveLeft()); }
            if (Input.GetKeyDown(KeyCode.S)) { Execute(new CommandMoveRight()); }
            if (Input.GetKeyDown(KeyCode.D)) { Execute(new CommandJump()); }
            if (Input.GetKeyDown(KeyCode.Z)) { UndoOnce(); }
            if (Input.GetKeyDown(KeyCode.Y)) { RedoOnce(); }
        }

        protected override int GetCapacity() => 5;
    }


    public class CommandMoveLeft : IReversibleCommand<ReversibleCommandPatternTest>
    {
        public void Execute()
        {
            Debug.Log("Move Left");
        }

        public void ExecuteBackwards()
        {
            Debug.Log("Move Left Reversed");
        }
    }


    public class CommandMoveRight : IReversibleCommand<ReversibleCommandPatternTest>
    {
        public void Execute()
        {
            Debug.Log("Move Right");
        }

        public void ExecuteBackwards()
        {
            Debug.Log("Move Right Reversed");
        }
    }

    public class CommandJump : IReversibleCommand<ReversibleCommandPatternTest>
    {
        public void Execute()
        {
            Debug.Log("Jump");
        }

        public void ExecuteBackwards()
        {
            Debug.Log("Jump Reversed");
        }
    }
}