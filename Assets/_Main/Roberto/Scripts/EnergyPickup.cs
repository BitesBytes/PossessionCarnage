using UnityEngine;

public class EnergyPickup : BasePickup
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (!isCollected)
        {
            //TODO
            //Player player = other.gameObject.GetComponent<Player>();

            //if (player != null)
            //{
            //    if (player.GetHealthSystem().IsNotMaxHealth())
            //    {
            //        character.GetHealthSystem().ChangeHealthAmount(pickup.HealthAmount);
            //        visual.SetActive(false);
            //        visualFX.SetActive(true);
            //
            //        isCollected = true;
            //
            //        movementEffectBehaviour.enabled = false;
            //    }
            //}
        }
    }
}
