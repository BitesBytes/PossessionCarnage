using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Pickup")]
public class PickupSO : ScriptableObject
{
    [Header("GENERAL")]
    public string StringName;
    public float HealthAmount = 15f;
    public float EnergyAmount = 25f;
    public float TimeForDestroying = 20f;

    [Header("MOVEMENT EFFECT")]
    public float MoveSpeed = 2f;
    public float RotationSpeed = 15f;
}
