using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;

    private void Start()
    {
        EventManager.OnPossessedCharacterChanged += EventManager_OnPossessedCharacterChanged;
        HealthSystem.OnHealthAmountChanged += HealthSystem_OnHealthAmountChanged;
    }

    private void EventManager_OnPossessedCharacterChanged(Character character)
    {
        healthBar.fillAmount = character.GetHealthSystem().GetCurrentHealthNormalized();
    }

    private void HealthSystem_OnHealthAmountChanged(object sender, System.EventArgs e)
    {
        HealthSystem currentCharacterHealthSystem = sender as HealthSystem;

        healthBar.fillAmount = currentCharacterHealthSystem.GetCurrentHealthNormalized();
    }

    private void OnDestroy()
    {
        EventManager.OnPossessedCharacterChanged -= EventManager_OnPossessedCharacterChanged;
        HealthSystem.OnHealthAmountChanged -= HealthSystem_OnHealthAmountChanged;
    }
}
