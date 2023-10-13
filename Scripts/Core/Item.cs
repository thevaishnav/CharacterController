using UnityEngine;

namespace CCN.Core
{
    
    /// <summary> This item will be enabled only when its being used. </summary>
    public class Item : MonoBehaviour
    {
        [SerializeField, Tooltip("Unique identifier for this item. When this item is equipped, \"Item ID\" in animator will be set to this parameter value.")]
        private int itemId;
        
        public virtual bool CanEquip()
        {
            return true;
        }

        
        public virtual bool IsEquipped()
        {
            return enabled;
        }

        public virtual bool IsInUse()
        {
            return false;
        }
        
        
        public virtual void OnEquip()
        {
            
        }

        public virtual void OnUnequip()
        {
            
        }

        public virtual void OnUse()
        {
            
        }
    }
}