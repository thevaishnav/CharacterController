using System;
using System.Threading.Tasks;
using KS.CharaCon.Attributes;
using UnityEngine;

namespace KS.CharaCon.Abilities
{
    /// <summary> Jump ability for the player </summary>
    [Serializable]
    [DefaultAbilityIndex(-2)]
    [DefaultAbilityStartType(KeyCode.Space)]
    [DefaultAbilityEndType(KeyCode.Space)]
    public class JumpAbility : KS.CharaCon.Ability
    {
        [SerializeField] private float jumpForce = 10f;
        
        protected override async void OnAbilityEnabled()
        {
            if (Controller.IsGrounded)
            {
                Controller.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            }
            await Task.Delay(100); // Delay so that animator can sync the Ability Index
            TryDisable(true);
        }
    }
}