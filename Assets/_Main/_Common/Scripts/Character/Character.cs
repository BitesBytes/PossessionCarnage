using UnityEngine;
using System;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(HealthSystem), typeof(AttackSystem), typeof(AISystem))]
public class Character : MonoBehaviour
{
    [SerializeField] private CharacterTypeSO characterType;

    public bool IsPossessed { get; set; }

    private HealthSystem healthSystem;
    private AttackSystem attackSystem;
    private AISystem aiSystem;
    private Animator animator;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        attackSystem = GetComponent<AttackSystem>();
        aiSystem = GetComponent<AISystem>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        healthSystem.Init(characterType.HealthAmountMax, characterType.HealthDecreaseTimerMax);
        attackSystem.Init(animator, characterType.CharacterAttackType, characterType.LightAttackCountdownMax, characterType.LightAttackDamange, characterType.HeavyAttackCountdownMax, characterType.HeavyAttackDamange, characterType.SpecialAttackCountdownMax, characterType.SpecialAttackDamange, characterType.LightAttackCountdownSpeed, characterType.HeavyAttackCountdownSpeed, characterType.SpecialAttackCountdownSpeed);
        aiSystem.Init(this);

        EventManager.OnPossessedCharacterChanged += EventManager_OnPossessedCharacterChanged;
        healthSystem.OnDie += HealthSystem_OnDie;
    }

    private void HealthSystem_OnDie(object sender, EventArgs e)
    {
        if(!IsPossessed)
        {
            Destroy(this.gameObject);
        }
        else
        {
            healthSystem.PossessedDie();
        }
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

    public Animator GetAnimator()
    {
        return animator;
    }

    public void SetAnimator(Animator animator)
    {
        this.animator = animator;
    }

    private void OnDestroy()
    {
        EventManager.OnPossessedCharacterChanged -= EventManager_OnPossessedCharacterChanged;
        healthSystem.OnDie -= HealthSystem_OnDie;
    }
}
