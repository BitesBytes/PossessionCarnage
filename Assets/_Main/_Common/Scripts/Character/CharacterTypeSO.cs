using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CharacterType")]
public class CharacterTypeSO : ScriptableObject
{
    [Header("GENERAL")]
    public string NameString;
    public Transform Prefab;

    [Header("HEALTH SYSTEM")]
    public float HealthAmountMax = 100f;
    public float HealthDecreaseTimerMax = 0f;

    [Header("ATTACK SYSTEM")]
    public CharacterAttackType CharacterAttackType;
    public float LightAttackCountdownMax = 0.5f;
    public float LightAttackCountdownSpeed = 1f;
    public float LightAttackDamange = 10f;
    public float HeavyAttackCountdownMax = 1.5f;
    public float HeavyAttackCountdownSpeed = 1f;
    public float HeavyAttackDamange = 35f;
    public float SpecialAttackCountdownMax = 5f;
    public float SpecialAttackCountdownSpeed = 1f;
    public float SpecialAttackDamange = 55f;

    [Header("AI SYSTEM")]
    public float CharacterSpeed = 2f;
    public float NavMeshBorderOffset = 5f;
    public float SearchPlayerRay = 7f;
    public float AttackRange = 1.5f;
    public float DistanceToKeepFromPlayer = 0f;
    public MeshCollider MeshCollider;
    public float StunTimer = 0.5f;
    public float ImpactForce = -3.9f;



    //best way to handle animations functions without writing 10k useless and repetitive classes imho (Bruno)

    public void PlayBoolAnimation(Animator anim ,string name, bool condition)
    {
        anim.SetBool(name, condition);
    }

    public void PlayTriggerAnimation(Animator anim, string name)
    {
        anim.SetTrigger(name);
    }


}
