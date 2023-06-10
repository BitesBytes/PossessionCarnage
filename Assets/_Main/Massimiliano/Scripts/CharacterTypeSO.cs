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
}
