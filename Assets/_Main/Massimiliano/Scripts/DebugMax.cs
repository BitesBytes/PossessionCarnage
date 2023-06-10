using UnityEngine;

public class DebugMax : MonoBehaviour
{
    private AIBehaviourManager aiBehaviourManager;

    [SerializeField] private Character character1;
    [SerializeField] private Character character2;

    [SerializeField] private Character actualCharacter;

    [SerializeField] private float speed = 5f;

    private CharacterController controller;

    private void Start()
    {
        aiBehaviourManager = AIBehaviourManager.Instance;

        aiBehaviourManager.SetDebugMax(this);

        actualCharacter = character1;

        EventManager.OnPossessedCharacterChangedCall(actualCharacter);

        controller = GetComponent<CharacterController>();

        PlayerInputSystem.OnSpecialAbilityPerformed += PlayerInputSystem_OnSpecialAbilityPerformed;
        PlayerInputSystem.OnLightAttackPerformed += PlayerInputSystem_OnLightAttackPerformed;
        PlayerInputSystem.OnHeavyAttackPerformed += PlayerInputSystem_OnHeavyAttackPerformed;
    }

    private void PlayerInputSystem_OnHeavyAttackPerformed()
    {
        actualCharacter.GetAttackSystem().PerformAttack(AttackType.HEAVY);
    }

    private void PlayerInputSystem_OnLightAttackPerformed()
    {
        actualCharacter.GetAttackSystem().PerformAttack(AttackType.LIGHT);
    }

    private void PlayerInputSystem_OnSpecialAbilityPerformed()
    {
        if (actualCharacter == character1)
        {
            actualCharacter = character2;
        }
        else
        {
            actualCharacter = character1;
        }

        EventManager.OnPossessedCharacterChangedCall(actualCharacter);
    }

    private void FixedUpdate()
    {
        controller.Move(new Vector3(PlayerInputSystem.GetDirectionNormalized().x, 0f, PlayerInputSystem.GetDirectionNormalized().y) * Time.fixedDeltaTime * speed);
    }
}
