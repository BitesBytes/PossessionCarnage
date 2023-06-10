using UnityEngine;

[RequireComponent(typeof(HealthSystem), typeof(AttackSystem), typeof(AISystem))]
public class Character : MonoBehaviour
{
    [SerializeField] private CharacterTypeSO characterType;

    private HealthSystem healthSystem;
    private AttackSystem attackSystem;
    private AISystem aiSystem;

    public bool IsPlayer;

    private void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.Init(characterType.HealthAmountMax, characterType.HealthDecreaseTimerMax);

        attackSystem = GetComponent<AttackSystem>();
        attackSystem.Init(characterType.CharacterAttackType, characterType.LightAttackCountdownMax, characterType.LightAttackDamange, characterType.HeavyAttackCountdownMax, characterType.HeavyAttackDamange, characterType.SpecialAttackCountdownMax, characterType.SpecialAttackDamange, characterType.LightAttackCountdownSpeed, characterType.HeavyAttackCountdownSpeed, characterType.SpecialAttackCountdownSpeed);

        aiSystem = GetComponent<AISystem>();
    }
    private void Update()
    {
        if (IsPlayer)
        {
            healthSystem.DecreaseHealthOverTime();
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

}
