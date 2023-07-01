using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private Character character;

    private void OnTriggerEnter(Collider other)
    {
        Character otherCharacter = other.GetComponent<Character>();
        Player player = other.GetComponent<Player>();

        if (otherCharacter != null)
        {
            otherCharacter.GetHealthSystem().ChangeHealthAmount(-character.GetAttackSystem().GetActualDamage());
        }
        else if (player != null)
        {
            player.GetPossessedBodyComponent().GetHealthSystem().ChangeHealthAmount(-character.GetAttackSystem().GetActualDamage());
        }
    }
}
