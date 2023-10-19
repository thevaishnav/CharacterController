using System;
using CCN.Core;
using CCN.InputSystemWrapper;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CCN.Behaviours
{
    /// <summary> Run or Walk slowly behaviour for the agent </summary>
    [Serializable]
    public class SetMoveSpeed : AgentBehaviour
    {
        protected override void Reset(Agent agent)
        {
            base.Reset(agent);

            id = -3;
            moveSpeedMultiplier = 2f;
            
            #if UNITY_EDITOR
            interactionProfile = AssetDatabase.LoadAssetAtPath<InteractionProfileBase>(AssetDatabase.GUIDToAssetPath("0c76dba6aa377f044ba03340f1faa72c"));
            #endif
        }
    }
}