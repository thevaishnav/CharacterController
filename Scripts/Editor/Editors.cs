using MenuManagement.Editor;
using Omnix.CCN.Core;
using Omnix.CCN.Health;
using UnityEditor;

namespace Omnix.CCN.EditorSpace
{
    [CustomEditor(typeof(DamageReceiver))]
    public class Ed_DamageReceiver : BaseEditorWithGroups { }
    
    [CustomEditor(typeof(AgentItem), true)]
    public class Ed_AgentItem : BaseEditorWithGroups { }
}