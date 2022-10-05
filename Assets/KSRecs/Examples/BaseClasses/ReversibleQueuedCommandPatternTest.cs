using System.Collections;
using UnityEngine;
using KSRecs.BaseClasses;

namespace KSRecs.Examples
{
    public class ReversibleQueuedCommandPatternTest : QueuedReversibleCommandPattern<ReversibleQueuedCommandPatternTest>
    {
        public static ReversibleQueuedCommandPatternTest Instance;

        void Start()
        {
            Instance = this;
        }

        protected override int GetCapacity() => 15;

        public IEnumerator StartCommand(string commandName, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            Debug.Log($"{commandName} Done");
            DoneExecution();
        }
    }


    public class CommandMoveLeftWithTime : IReversibleCommand<ReversibleQueuedCommandPatternTest>
    {
        public void Execute()
        {
            Debug.Log("Move Left started");
            ReversibleQueuedCommandPatternTest.Instance.StartCoroutine(
                ReversibleQueuedCommandPatternTest.Instance.StartCommand("Move Left", 2f));
        }

        public void ExecuteBackwards()
        {
            Debug.Log("Move Left Reversed");
            ReversibleQueuedCommandPatternTest.Instance.StartCoroutine(
                ReversibleQueuedCommandPatternTest.Instance.StartCommand("Move Left Reversed", 2f));
        }
    }


    public class CommandMoveRightWithTime : IReversibleCommand<ReversibleQueuedCommandPatternTest>
    {
        public void Execute()
        {
            Debug.Log("Move Right started");
            ReversibleQueuedCommandPatternTest.Instance.StartCoroutine(
                ReversibleQueuedCommandPatternTest.Instance.StartCommand("Move Right", 4f));
        }

        public void ExecuteBackwards()
        {
            Debug.Log("Move Right Reversed");
            ReversibleQueuedCommandPatternTest.Instance.StartCoroutine(
                ReversibleQueuedCommandPatternTest.Instance.StartCommand("Move Right Reversed", 5f));
        }
    }

    public class CommandJumpWithTime : IReversibleCommand<ReversibleQueuedCommandPatternTest>
    {
        public void Execute()
        {
            Debug.Log("Jump started");
            ReversibleQueuedCommandPatternTest.Instance.StartCoroutine(
                ReversibleQueuedCommandPatternTest.Instance.StartCommand("Jump", 3f));
        }

        public void ExecuteBackwards()
        {
            Debug.Log("Jump Reversed");
            ReversibleQueuedCommandPatternTest.Instance.StartCoroutine(
                ReversibleQueuedCommandPatternTest.Instance.StartCommand("Jump Reversed", 1f));
        }
    }
}