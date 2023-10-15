using System;
using CCN.Core;
using CCN.InputSystemWrapper;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CCN.Behaviours
{
    /// <summary> Crouch behaviour for the Agent </summary>
    [Serializable]
    public class Crouch : AgentBehaviour
    {
        protected override void Reset(Agent agent)
        {
            base.Reset(agent);
            id = -1;
            #if UNITY_EDITOR
            interactionProfile = AssetDatabase.LoadAssetAtPath<InteractionProfileBase>(AssetDatabase.GUIDToAssetPath("2d01a28be11ac5a438bd55ecda8ef326"));
            #endif
        }
    }
}