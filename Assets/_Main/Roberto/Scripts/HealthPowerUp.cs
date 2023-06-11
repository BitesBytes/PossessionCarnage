using UnityEngine;

public class HealPowerUpSystem : MonoBehaviour
{

    public float Heal = 50f;


    private void OnTriggerEnter(Collider col)
    {
        //if(check) if player health is lower than maxHealth

        //if player isPossessed can pick PowerUp

        col.GetComponent<Character>().GetHealthSystem().ChangeHealthAmount(Heal);

        Destroy(gameObject);

    }
}