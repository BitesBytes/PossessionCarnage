using UnityEngine;

[RequireComponent(typeof(HealthSystem), typeof(AttackSystem), typeof(AISystem))]
public class Character : MonoBehaviour
{
    [SerializeField] private CharacterTypeSO characterType;

    private HealthSystem healthSystem;
    private AttackSystem attackSystem;
    private AISystem aiSystem;

    private void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.Init(characterType.HealthAmountMax, characterType.HealthDecreaseTimerMax);

        attackSystem = GetComponent<AttackSystem>();
        attackSystem.Init(characterType.CharacterAttackType, characterType.LightAttackCountdownMax, characterType.LightAttackDamange, characterType.HeavyAttackCountdownMax, characterType.HeavyAttackDamange, characterType.SpecialAttackCountdownMax, characterType.SpecialAttackDamange, characterType.LightAttackCountdownSpeed, characterType.HeavyAttackCountdownSpeed, characterType.SpecialAttackCountdownSpeed);

        aiSystem = GetComponent<AISystem>();
        aiSystem.Init(this);

        EventManager.OnPossessedCharacterChanged += EventManager_OnPossessedCharacterChanged;
    }

    private void EventManager_OnPossessedCharacterChanged(Character character)
    {
        if(character == null || character != this)
        {
            aiSystem.enabled = true;
        }
        else
        {
            aiSystem.enabled = false;
        }
    }

    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }

    public AttackSystem GetAttackSystem()
    {
        return attackSystem;
    }

    public AISystem GetAISystem()
    {
        return aiSystem;
    }

    public CharacterTypeSO GetCharacterType()
    {
        return characterType;
    }

    private void OnDestroy()
    {
        EventManager.OnPossessedCharacterChanged -= EventManager_OnPossessedCharacterChanged;
    }
}
