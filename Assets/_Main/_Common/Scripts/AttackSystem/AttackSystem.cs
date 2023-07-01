using UnityEngine;


public enum CharacterAttackType
{
    MELEE,
    RANGED,
    LAST
}

public enum AttackType
{
    LIGHT,
    HEAVY,
    SPECIAL,
    LAST
}

[RequireComponent(typeof(SphereCollider))]
public class AttackSystem : MonoBehaviour
{
    private CharacterAttackType characterAttackType;
    private AttackType attackType;

    private float lightAttackCountdownMax;
    private float lightAttackDamange;
    private float heavyAttackCountdownMax;
    private float heavyAttackDamange;
    private float specialAttackCountdownMax;
    private float specialAttackDamange;

    private float lightAttackCountdown;
    private float heavyAttackCountdown;
    private float specialAttackCountdown;

    private float lightAttackCountdownSpeed;
    private float heavyAttackCountdownSpeed;
    private float specialAttackCountdownSpeed;

    private float actualDamage;

    private SphereCollider hitSphereCollider;

    private Animator animator;

    ////Debug Attack
    //[SerializeField] private Transform bulletSpawn;
    //[SerializeField] private GameObject bulletPrefab;
    //[SerializeField] private GameObject rangedDebug; //MEGA Debug

    private void Awake()
    {
        hitSphereCollider = GetComponent<SphereCollider>();

        hitSphereCollider.enabled = false;
        hitSphereCollider.isTrigger = true;
    }

    private void Update()
    {
        if (lightAttackCountdown > 0f)
        {
            lightAttackCountdown -= Time.deltaTime * lightAttackCountdownSpeed;
        }

        if (heavyAttackCountdown > 0f)
        {
            heavyAttackCountdown -= Time.deltaTime * heavyAttackCountdownSpeed;
        }

        if (specialAttackCountdown > 0f)
        {
            specialAttackCountdown -= Time.deltaTime * specialAttackCountdownSpeed;
        }
    }

    public void PerformAttack(AttackType attackType)
    {
        this.attackType = attackType;

        switch (characterAttackType)
        {
            case CharacterAttackType.MELEE:
                PerformMeleeAttack();
                break;
            case CharacterAttackType.RANGED:
                PerformRangedAttack();
                break;
        }
    }

    private void PerformMeleeAttack()
    {
        switch (attackType)
        {
            case AttackType.LIGHT:
                PerformLightAttack();
                break;
            case AttackType.HEAVY:
                PerformHeavyAttack();
                break;
            case AttackType.SPECIAL:
                PerformSpecialAttack();
                break;
        }
    }

    private void PerformRangedAttack()
    {
        switch (attackType)
        {
            case AttackType.LIGHT:
                PerformLightAttack();
                break;
            case AttackType.HEAVY:
                PerformHeavyAttack();
                break;
            case AttackType.SPECIAL:
                PerformSpecialAttack();
                break;
        }
    }

    private void PerformLightAttack()
    {
        actualDamage = lightAttackDamange;

        if (lightAttackCountdown <= 0f)
        {
            lightAttackCountdown = lightAttackCountdownMax;

            AnimatorSystem.DoLightAttack(animator);

            //Debug.Log(lightAttackDamange);
            //GameObject g = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            //g.GetComponent<MagicBullet>().RangedDebug = rangedDebug; // mega debug
        }
    }

    private void PerformHeavyAttack()
    {
        actualDamage = heavyAttackDamange;

        if (heavyAttackCountdown <= 0f)
        {
            heavyAttackCountdown = heavyAttackCountdownMax;

            AnimatorSystem.DoHeavyAttack(animator);
        }
    }

    private void PerformSpecialAttack()
    {
        actualDamage = specialAttackDamange;

        if (specialAttackCountdown <= 0f)
        {
            specialAttackCountdown = specialAttackCountdownMax;

            AnimatorSystem.DoSpecialAttack(animator);
        }
    }

    public void Init(Animator animator, CharacterAttackType characterAttackType, float lightAttackCountdownMax, float lightAttackDamange, float heavyAttackCountdownMax, float heavyAttackDamange, float specialAttackCountdownMax, float specialAttackDamange, float lightAttackCountdownSpeed, float heavyAttackCountdownSpeed, float specialAttackCountdownSpeed)
    {
        this.animator = animator;

        this.characterAttackType = characterAttackType;

        this.lightAttackCountdownMax = lightAttackCountdownMax;
        this.lightAttackDamange = lightAttackDamange;
        this.heavyAttackCountdownMax = heavyAttackCountdownMax;
        this.heavyAttackDamange = heavyAttackDamange;
        this.specialAttackCountdownMax = specialAttackCountdownMax;
        this.specialAttackDamange = specialAttackDamange;
        this.lightAttackCountdownSpeed = lightAttackCountdownSpeed;
        this.heavyAttackCountdownSpeed = heavyAttackCountdownSpeed;
        this.specialAttackCountdownSpeed = specialAttackCountdownSpeed;
    }

    public float GetActualDamage()
    {
        return actualDamage;
    }

    public void EnableHitSphere()
    {
        hitSphereCollider.enabled = true;
    }

    public void DisableHitSphere()
    {
        hitSphereCollider.enabled = false;
    }
}
