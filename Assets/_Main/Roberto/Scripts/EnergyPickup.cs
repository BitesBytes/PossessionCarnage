using UnityEngine;

public class EnergyPickup : BasePickup
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (!isCollected)
        {
            Player player = other.GetComponentInParent<Player>();

            Debug.Log("TEST");

            if (player != null && !player.HasMaxEnergy())
            {
                player.GainEnergyAmount(pickup.EnergyAmount);
                visual.SetActive(false);
                visualFX.SetActive(true);

                isCollected = true;

                movementEffectBehaviour.enabled = false;
            }
        }
    }
}
