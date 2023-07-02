using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image energyBar;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI timeText;

    [SerializeField] private Player player;
    [SerializeField] private WavesSystem waveSystem;

    private Character actualCharacter;

    private void Start()
    {
        EventManager.OnPossessedCharacterChanged += EventManager_OnPossessedCharacterChanged;

        player.GetHealthSystem().OnHealthAmountChanged += PlayerHealthSystem_OnHealthAmountChanged;
        player.OnEnergyAmountChanged += Player_OnEnergyAmountChanged;

        waveSystem.OnWaveChanged += WaveSystem_OnWaveChanged;
        waveSystem.OnTimeChanged += WaveSystem_OnTimeChanged;
    }

    private void WaveSystem_OnTimeChanged(object sender, WavesSystem.OnTimeChangedEventArgs e)
    {
        timeText.text = $"Time to Win: {e.timeToWin}";
    }

    private void WaveSystem_OnWaveChanged(object sender, WavesSystem.OnWaveChangedEventArgs e)
    {
        waveText.text = $"Wave: {e.currentWave}";
    }

    private void Player_OnEnergyAmountChanged(object sender, System.EventArgs e)
    {
        energyBar.fillAmount = player.GetEnergyAmountNormalized();
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
        healthBar.fillAmount = actualCharacter.GetHealthSystem().GetCurrentHealthNormalized();
    }

    private void OnDestroy()
    {
        EventManager.OnPossessedCharacterChanged -= EventManager_OnPossessedCharacterChanged;
        player.GetHealthSystem().OnHealthAmountChanged -= PlayerHealthSystem_OnHealthAmountChanged;
        player.OnEnergyAmountChanged -= Player_OnEnergyAmountChanged;
        waveSystem.OnWaveChanged -= WaveSystem_OnWaveChanged;
        waveSystem.OnTimeChanged -= WaveSystem_OnTimeChanged;

        if (actualCharacter != null)
        {
            actualCharacter.GetHealthSystem().OnHealthAmountChanged -= ActualCharacterHealthSystem_OnHealthAmountChanged;
        }
    }
}
