using UnityEngine;

[DefaultAbilityIndex(-2)]
[DefaultAbilityStartType(KeyCode.Space)]
[DefaultAbilityEndType(KeyCode.Space)]
public class JumpAbility : Ability
{
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravity = -10f;

    private Vector3 gravityOnStart;
    private void OnEnable()
    {
        gravityOnStart = Controller.Gravity;
        Controller.Gravity = new Vector3(0, gravity, 0);
        Controller.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
    }

    private void OnDisable()
    {
        Controller.Gravity = gravityOnStart;
    }
}