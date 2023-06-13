using UnityEngine;

public class HealPowerUpSystem : PickupObject
{
    private float healAmount;

    protected HealPowerUpSystem()
    {
        healAmount = 50f;
    }

    public new void Picked()
    {
        character.GetComponent<Character>().GetHealthSystem().ChangeHealthAmount(healAmount);
    }


    //[SerializeField] private float Heal = 50f;

    //private void OnTriggerEnter(Collider col)
    //{
    //    //if(check) if player health is lower than maxHealth and if player isPossessed can pick PowerUp

    //    //if (GetComponent<Character>().GetHealthSystem().IsNotMaxHealth() && GetComponent<Character>().IsPlayer)

    //    col.GetComponent<Character>().GetHealthSystem().ChangeHealthAmount(Heal);
    //    Destroy(gameObject);
    //}
}