using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Character thisCharacter;

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if(character != null)
        {
            character.GetHealthSystem().ChangeHealthAmount(thisCharacter.GetAttackSystem().GetActualDamage());
        }
    }
}
