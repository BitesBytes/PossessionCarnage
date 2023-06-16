using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Player player;

    private Character actualCharacter;

    private void Start()
    {
        EventManager.OnPossessedCharacterChanged += EventManager_OnPossessedCharacterChanged;
        player.GetHealthSystem().OnHealthAmountChanged += PlayerHealthSystem_OnHealthAmountChanged;
    }

    private void PlayerHealthSystem_OnHealthAmountChanged(object sender, HealthSystem.OnHealthAmountChangedEventArgs e)
    {
        healthBar.fillAmount = player.GetHealthSystem().GetCurrentHealthNormalized();
    }

    private void EventManager_OnPossessedCharacterChanged(Character character)
    {
        if (actualCharacter != null)
        {
            actualCharacter.GetHealthSystem().OnHealthAmountChanged -= ActualCharacterHealthSystem_OnHealthAmountChanged;
        }

        actualCharacter = character;

        if (actualCharacter != null)
        {
            healthBar.fillAmount = actualCharacter.GetHealthSystem().GetCurrentHealthNormalized();
            actualCharacter.GetHealthSystem().OnHealthAmountChanged += ActualCharacterHealthSystem_OnHealthAmountChanged;
        }
        else
        {
            healthBar.fillAmount = player.GetHealthSystem().GetCurrentHealthNormalized();
        }
    }

    private void ActualCharacterHealthSystem_OnHealthAmountChanged(object sender, HealthSystem.OnHealthAmountChangedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void OnDestroy()
    {
        EventManager.OnPossessedCharacterChanged -= EventManager_OnPossessedCharacterChanged;
        player.GetHealthSystem().OnHealthAmountChanged -= PlayerHealthSystem_OnHealthAmountChanged;

        if (actualCharacter != null)
        {
            actualCharacter.GetHealthSystem().OnHealthAmountChanged -= ActualCharacterHealthSystem_OnHealthAmountChanged;
        }
    }
}
