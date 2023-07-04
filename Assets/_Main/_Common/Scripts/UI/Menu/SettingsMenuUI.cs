using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuUI : MonoBehaviour
{
    public event EventHandler OnMusicChanged;

    [SerializeField] private Button closeButton;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider FXSlider;

    [SerializeField] private MenuManager menuManager;

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            menuManager.CloseSettingsMenu();
        });
        musicSlider.onValueChanged.AddListener((value) =>
        {
            SoundManager.MusicVolume = value;
            PlayerPrefs.SetFloat(SoundManager.MUSIC_VOLUME, SoundManager.MusicVolume);

            OnMusicChanged?.Invoke(this, EventArgs.Empty);
        });
        FXSlider.onValueChanged.AddListener((value) =>
        {
            SoundManager.FXVolume = value;
            PlayerPrefs.SetFloat(SoundManager.FX_VOLUME, SoundManager.FXVolume);
        });
    }

    public void Show()
    {
        this.gameObject.SetActive(true);

        musicSlider.value = SoundManager.MusicVolume;
        FXSlider.value = SoundManager.FXVolume;
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
