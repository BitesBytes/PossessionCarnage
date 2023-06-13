using UnityEngine;

[RequireComponent(typeof(PickablePhysics))]
public class EnergyPowerUp : PickupObject
{
    private float energyAmount;

    [SerializeField] private float energyQuantity;

    public new void Picked()
    {
        //return change Energy
        //+= energyQuantity
    }
}
