using UnityEngine;
using KSRecs.BaseClasses;

namespace KSRecs.Examples
{
    public class SimpleCommandPatternTest : SimpleCommandPattern<SimpleCommandPatternTest>
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A)) { Execute(new CommandHello()); }
            if (Input.GetKeyDown(KeyCode.S)) { Execute(new CommandMyNameIs()); }
            if (Input.GetKeyDown(KeyCode.D)) { Execute(new CommandVaishnav()); }
        }
    }


    public class CommandHello : ICommand<SimpleCommandPatternTest>
    {
        public void Execute()
        {
            Debug.Log("Hello World");
        }
    }


    public class CommandMyNameIs : ICommand<SimpleCommandPatternTest>
    {
        public void Execute()
        {
            Debug.Log("My Name Is");
        }
    }

    public class CommandVaishnav : ICommand<SimpleCommandPatternTest>
    {
        public void Execute()
        {
            Debug.Log("Vaishnav");
        }
    }
}