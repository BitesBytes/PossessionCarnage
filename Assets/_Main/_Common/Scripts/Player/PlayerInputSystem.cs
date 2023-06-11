using UnityEngine;

public delegate void onButtonPressed();
public static class PlayerInputSystem
{
    public static event onButtonPressed OnSpecialAbilityPerformed;
    public static event onButtonPressed OnLightAttackPerformed;
    public static event onButtonPressed OnHeavyAttackPerformed;
    public static event onButtonPressed OnExitToMainMenuPerformed;

    private static PlayerInputAction playerInputAction;

    static PlayerInputSystem()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Enable();

        playerInputAction.Player.SpecialAbility.performed += SpecialAbility_performed;
        playerInputAction.Player.LightAttack.performed += LightAttack_performed;
        playerInputAction.Player.HeavyAttack.performed += HeavyAttack_performed;
        playerInputAction.Player.ExitToMainMenu.performed += ExitToMainMenu_performed;
    }

    private static void ExitToMainMenu_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnExitToMainMenuPerformed?.Invoke();
    }

    private static void HeavyAttack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnHeavyAttackPerformed?.Invoke();
    }

    private static void LightAttack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnLightAttackPerformed?.Invoke();
    }

    private static void SpecialAbility_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnSpecialAbilityPerformed?.Invoke();
    }

    public static Vector2 GetDirectionNormalized()
    {
        return playerInputAction.Player.Move.ReadValue<Vector2>();
    }
}
