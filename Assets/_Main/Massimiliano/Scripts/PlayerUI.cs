using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private HealthManager healthManager;

    [SerializeField] private Image HealthBar;

    private void Start()
    {
        healthManager = HealthManager.Instance;

        healthManager.OnPossessedCharacterChanged += HealthManager_OnPossessedCharacterChanged;
    }

    private void HealthManager_OnPossessedCharacterChanged(object sender, HealthManager.OnPossessedCharacterChangedEventArgs e)
    {
        HealthBar.fillAmount = e.healthSystem.GetCurrentHealthNormalized();
    }

}
