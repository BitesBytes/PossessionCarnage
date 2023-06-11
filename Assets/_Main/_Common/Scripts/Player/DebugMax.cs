using UnityEngine;

public class DebugMax : MonoBehaviour
{
    [SerializeField] private Character melee;
    [SerializeField] private Character ranged;

    [SerializeField] private Character actualCharacter;

    [SerializeField] private float speed = 5f;

    private CharacterController controller;

    private void Start()
    {
        actualCharacter = melee;

        EventManager.OnPossessedCharacterChangedCall(actualCharacter);

        controller = GetComponent<CharacterController>();

        PlayerInputSystem.OnSpecialAbilityPerformed += PlayerInputSystem_OnSpecialAbilityPerformed;
        PlayerInputSystem.OnLightAttackPerformed += PlayerInputSystem_OnLightAttackPerformed;
        PlayerInputSystem.OnHeavyAttackPerformed += PlayerInputSystem_OnHeavyAttackPerformed;
        PlayerInputSystem.OnExitToMainMenuPerformed += PlayerInputSystem_OnExitToMainMenuPerformed;
    }

    private void PlayerInputSystem_OnExitToMainMenuPerformed()
    {
        SceneManagementSystem.ExitToMainMenu();
    }

    private void PlayerInputSystem_OnHeavyAttackPerformed()
    {
        actualCharacter.GetAttackSystem().PerformAttack(AttackType.HEAVY);
        DebugText.SetDamageDebug(actualCharacter.GetCharacterType().HeavyAttackDamange); //DEBUG
    }

    private void PlayerInputSystem_OnLightAttackPerformed()
    {
        actualCharacter.GetAttackSystem().PerformAttack(AttackType.LIGHT);
        DebugText.SetDamageDebug(actualCharacter.GetCharacterType().LightAttackDamange); //DEBUG
    }

    private void PlayerInputSystem_OnSpecialAbilityPerformed()
    {
        if (actualCharacter == melee)
        {
            actualCharacter = ranged;
        }
        else
        {
            actualCharacter = melee;
        }

        EventManager.OnPossessedCharacterChangedCall(actualCharacter);
    }

    private void FixedUpdate()
    {
        controller.Move(new Vector3(PlayerInputSystem.GetDirectionNormalized().x, 0f, PlayerInputSystem.GetDirectionNormalized().y) * Time.fixedDeltaTime * speed);
    }

    private void OnDestroy()
    {
        PlayerInputSystem.OnSpecialAbilityPerformed -= PlayerInputSystem_OnSpecialAbilityPerformed;
        PlayerInputSystem.OnLightAttackPerformed -= PlayerInputSystem_OnLightAttackPerformed;
        PlayerInputSystem.OnHeavyAttackPerformed -= PlayerInputSystem_OnHeavyAttackPerformed;
        PlayerInputSystem.OnExitToMainMenuPerformed -= PlayerInputSystem_OnExitToMainMenuPerformed;
    }

    public Character GetActualCharacter()
    {
        return actualCharacter;
    }
}
