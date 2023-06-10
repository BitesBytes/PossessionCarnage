using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private MenuManager menuManager;

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

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
