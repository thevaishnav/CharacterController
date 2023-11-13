using System;
using System.Collections;
using Omnix.CCN.Core;
using UnityEngine;

namespace Omnix.CCN.Utils
{
    public static class Utilitiees
    {
        public static IEnumerator TempSetManagedIntOne(Agent agent, int value, float duration)
        {
            agent.ManagedInt1 = value;
            yield return new WaitForSeconds(duration);
            if (agent.ManagedInt1 == value) agent.ManagedInt1 = 0;
        }
        
        public static IEnumerator TempSetManagedIntTwo(Agent agent, int value, float duration)
        {
            agent.ManagedInt2 = value;
            yield return new WaitForSeconds(duration);
            if (agent.ManagedInt2 == value) agent.ManagedInt2 = 0;
        }

        public static IEnumerator TempSetManagedFloatOne(Agent agent, int value, float duration)
        {
            agent.ManagedFloat1 = value;
            yield return new WaitForSeconds(duration);
            if (Math.Abs(agent.ManagedFloat1 - value) < 0.0001f) agent.ManagedFloat1 = 0;
        }
        
        public static IEnumerator TempSetManagedFloatTwo(Agent agent, int value, float duration)
        {
            agent.ManagedFloat2 = value;
            yield return new WaitForSeconds(duration);
            if (Math.Abs(agent.ManagedFloat2 - value) < 0.0001f) agent.ManagedFloat2 = 0;
        }
    }
}