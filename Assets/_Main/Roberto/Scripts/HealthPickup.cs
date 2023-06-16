using UnityEngine;

public class HealthPickup : BasePickup
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (!isCollected)
        {
            Player player = other.gameObject.GetComponent<Player>();

            if (player != null)
            {
                Character possessedCharacter = player.GetPossessedBodyComponent();

                if ((possessedCharacter != null) && (possessedCharacter.GetHealthSystem().IsNotMaxHealth()))
                {
                    possessedCharacter.GetHealthSystem().ChangeHealthAmount(pickup.HealthAmount);
                    visual.SetActive(false);
                    visualFX.SetActive(true);

                    isCollected = true;

                    movementEffectBehaviour.enabled = false;
                }
            }
        }
    }
}