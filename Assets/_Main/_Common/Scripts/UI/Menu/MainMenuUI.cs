using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private MenuManager menuManager;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip musicClip;

    [SerializeField] private SettingsMenuUI settingsMenuUI;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            SceneManagementSystem.PlayGame();
        });

        loadButton.onClick.AddListener(() =>
        {
            menuManager.OpenLoadMenu();
        });

        settingsButton.onClick.AddListener(() =>
        {
            menuManager.OpenSettingsMenu();
        });

        closeButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void Start()
    {
        Cursor.visible = true;

        SoundManager.Init();

        SoundManager.PlayMusic(musicSource, musicClip, SoundManager.MusicVolume);

        settingsMenuUI.OnMusicChanged += SettingsMenuUI_OnMusicChanged;
    }

    private void SettingsMenuUI_OnMusicChanged(object sender, System.EventArgs e)
    {
        SoundManager.ChangeMusicVolume(musicSource);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        settingsMenuUI.OnMusicChanged -= SettingsMenuUI_OnMusicChanged;
    }
}
