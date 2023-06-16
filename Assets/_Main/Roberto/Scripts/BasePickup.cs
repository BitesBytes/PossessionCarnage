using UnityEngine;

[RequireComponent(typeof(PickupMovementEffect))]
public abstract class BasePickup : MonoBehaviour
{
    protected Character character;

    protected abstract void OnTriggerEnter(Collider other);
    protected abstract void Update();
}
