using UnityEngine;

[DefaultAbilityIndex(-2)]
[DefaultAbilityStartType(KeyCode.Space)]
[DefaultAbilityEndType(KeyCode.Space)]
public class JumpAbility : Ability
{
    [SerializeField] private float jumpForce = 10f;
    private void OnEnable()
    {
        Controller.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
    }
}