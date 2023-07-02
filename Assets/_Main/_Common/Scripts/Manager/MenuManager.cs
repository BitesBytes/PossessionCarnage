using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private SettingsMenuUI settingsMenuUI;
    [SerializeField] private CreditsMenuUI loadMenuUI;

    private void Start()
    {
        mainMenuUI.Show();
    }

    public void OpenMainMenu()
    {
        mainMenuUI.Show();
        settingsMenuUI.Hide();
        loadMenuUI.Hide();
    }

    public void CloseMainMenu()
    {
        mainMenuUI.Hide();
    }

    public void OpenSettingsMenu()
    {
        mainMenuUI.Hide();
        settingsMenuUI.Show();
    }

    public void CloseSettingsMenu()
    {
        settingsMenuUI.Hide();
        mainMenuUI.Show();
    }

    public void OpenLoadMenu()
    {
        mainMenuUI.Hide();
        loadMenuUI.Show();
    }

    public void CloseLoadMenu()
    {
        loadMenuUI.Hide();
        mainMenuUI.Show();
    }

}
