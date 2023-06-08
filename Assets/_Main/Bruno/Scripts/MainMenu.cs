using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private const string mainScene = "Assets/_Main/_Common/Scenes/MainScene.unity";
    private const string brunoScene = "Assets/_Main/Bruno/Scenes/BrunoScene.unity";
    private const string federicoScene = "Assets/_Main/Federico/Scenes/FedericoScene.unity";
    private const string simoneScene = "Assets/_Main/Simone/Scenes/SimoneScene.unity";
    private const string robertoScene = "Assets/_Main/Roberto/Scenes/RobertoScene.unity";
    private const string massimilianoScene = "Assets/_Main/Massimiliano/Scenes/MassimilianoScene.unity";

    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Canvas settingsCanvas;
    [SerializeField] private Canvas loadCanvas;


    void Start()
    {
        mainCanvas.gameObject.SetActive(true);
        settingsCanvas.gameObject.SetActive(false);
        loadCanvas.gameObject.SetActive(false);
    }

    //switch canvases

    public void OpenMainMenu()
    {
        mainCanvas.gameObject.SetActive(true);
        settingsCanvas.gameObject.SetActive(false);
        loadCanvas.gameObject.SetActive(false);
    }

    public void CloseMainMenu()
    {
        mainCanvas.gameObject.SetActive(false);
    }

    public void OpenSettings()
    {
        mainCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);
    }

    public void OpenLoadPanel()
    {
        mainCanvas.gameObject.SetActive(false);
        loadCanvas.gameObject.SetActive(true);
    }

    public void CloseLoadPanel()
    {
        loadCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);
    }

    //close the game

    public void Quit()
    {
        Application.Quit();
    }

    //Load the our scenes

    public void PlayGame()
    {
        SceneManager.LoadScene(mainScene);
    }

    public void LoadBrunoScene()
    {
        SceneManager.LoadScene(brunoScene);
    }

    public void LoadFedericoScene()
    {
        SceneManager.LoadScene(federicoScene);
    }

    public void LoadSimoneScene()
    {
        SceneManager.LoadScene(simoneScene);
    }

    public void LoadRobertoScene()
    {
        SceneManager.LoadScene(robertoScene);
    }

    public void LoadMassimilianoScene()
    {
        SceneManager.LoadScene(massimilianoScene);
    }
}
