using UnityEngine;

public class EnergyPickup : BasePickup
{
    [SerializeField] private GameObject fxObject;
    [SerializeField] private float energyAmount;

    protected override void OnTriggerEnter(Collider other)
    {
        throw new System.NotImplementedException();
    }

    protected override void Update()
    {
        throw new System.NotImplementedException();
    }
}
