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


    //DEBUG
    [SerializeField]
    private ParticleSystem bullet;

    [SerializeField]
    private Transform spawn; // da posizionare accanto alla bocca dello scettro




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
                //DEBUG
                bullet.Play();
                PerformLightAttack();
                break;
            case AttackType.HEAVY:
                //DEBUG
                bullet.Play();
                PerformHeavyAttack();
                break;
            case AttackType.SPECIAL:
                PerformSpecialAttack();
                break;
        }
    }

    private void PerformLightAttack()
    {
        if (lightAttackCountdown <= 0f)
        {
            lightAttackCountdown = lightAttackCountdownMax;

            Debug.Log(lightAttackDamange);
        }
        else
        {
            Debug.Log($"Devi attendere ancora {lightAttackCountdown} per poter fare l'attacco LEGGERO!");
            //DEBUG
            bullet.Stop();
        }
    }

    private void PerformHeavyAttack()
    {
        if (heavyAttackCountdown <= 0f)
        {
            heavyAttackCountdown = heavyAttackCountdownMax;

            Debug.Log(heavyAttackDamange);
        }
        else
        {
            Debug.Log($"Devi attendere ancora {heavyAttackCountdown} per poter fare l'attacco PESANTE!");
            //DEBUG
            bullet.Stop();
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

    
}
