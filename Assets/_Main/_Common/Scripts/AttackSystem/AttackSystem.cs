using UnityEngine;

public enum CharacterAttackType
{
    MELEE,
    RANGED
}

public enum AttackType
{
    LIGHT,
    HEAVY,
    SPECIAL
}

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


    //Debug (Bruno)
    [SerializeField] private Animator animatorController;
    [SerializeField] private BoxCollider hitbox;

    //Debug Attack
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject rangedDebug; //MEGA Debug

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
                animatorController.SetBool("LightAttack", true); //test
                PerformLightAttack();
                break;
            case AttackType.HEAVY:
                animatorController.SetBool("HeavyAttack", true); //test
                PerformHeavyAttack();
                break;
            case AttackType.SPECIAL:
                animatorController.SetBool("SpecialAttack", true); //test
                PerformSpecialAttack();
                break;
        }
    }

    private void PerformRangedAttack()
    {
        switch (attackType)
        {
            case AttackType.LIGHT:
                animatorController.SetBool("LightAttack", true); //test
                PerformLightAttack();
                break;
            case AttackType.HEAVY:
                animatorController.SetBool("LightAttack", true); //test
                PerformHeavyAttack();
                break;
            case AttackType.SPECIAL:
                animatorController.SetBool("SpecialAttack", true); //test
                PerformSpecialAttack();
                break;
        }
    }

    private void PerformLightAttack()
    {
        if (lightAttackCountdown <= 0f)
        {
            lightAttackCountdown = lightAttackCountdownMax;
            //animatorController.SetBool("LightAttack", true);
            Debug.Log(lightAttackDamange);
            GameObject g = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            g.GetComponent<MagicBullet>().RangedDebug = rangedDebug; // mega debug
        }
        else
        {
            Debug.Log($"Devi attendere ancora {lightAttackCountdown} per poter fare l'attacco LEGGERO!");
            animatorController.SetBool("LightAttack", false);
        }
    }

    private void PerformHeavyAttack()
    {
        if (heavyAttackCountdown <= 0f)
        {
            heavyAttackCountdown = heavyAttackCountdownMax;

            //animatorController.SetBool("HeavyAttack", true);

            Debug.Log(heavyAttackDamange);
        }
        else
        {
            Debug.Log($"Devi attendere ancora {heavyAttackCountdown} per poter fare l'attacco PESANTE!");
            animatorController.SetBool("HeavyAttack", false);
        }
    }

    private void PerformSpecialAttack()
    {
        if (specialAttackCountdown <= 0f)
        {
            specialAttackCountdown = specialAttackCountdownMax;

            Debug.Log(specialAttackDamange);
        }
        else
        {
            Debug.Log($"Devi attendere ancora {specialAttackCountdown} per poter fare l'attacco SPECIALE!");
            animatorController.SetBool("SpecialAttack", false);
        }
    }

    public void Init(CharacterAttackType characterAttackType, float lightAttackCountdownMax, float lightAttackDamange, float heavyAttackCountdownMax, float heavyAttackDamange, float specialAttackCountdownMax, float specialAttackDamange, float lightAttackCountdownSpeed, float heavyAttackCountdownSpeed, float specialAttackCountdownSpeed)
    {
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

    //Test (Bruno)

    public void EnableHitBox()
    {
        hitbox.enabled = true;
        Debug.Log("enabled");
    }

    public void DisableHitbox()
    {
        hitbox.enabled = false;
        Debug.Log("disabled");
    }
}
