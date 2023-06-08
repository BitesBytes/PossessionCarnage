using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private const string MAIN_SCENE = "Assets/_Main/_Common/Scenes/MainScene.unity";
    private const string BRUNO_SCENE = "Assets/_Main/Bruno/Scenes/BrunoScene.unity";
    private const string FEDERICO_SCENE = "Assets/_Main/Federico/Scenes/FedericoScene.unity";
    private const string SIMONE_SCENE = "Assets/_Main/Simone/Scenes/SimoneScene.unity";
    private const string ROBERTO_SCENE = "Assets/_Main/Roberto/Scenes/RobertoScene.unity";
    private const string MASSIMILIANO_SCENE = "Assets/_Main/Massimiliano/Scenes/MassimilianoScene.unity";


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
        SceneManager.LoadScene(MAIN_SCENE);
    }

    public void LoadBrunoScene()
    {
        SceneManager.LoadScene(BRUNO_SCENE);
    }

    public void LoadFedericoScene()
    {
        SceneManager.LoadScene(FEDERICO_SCENE);
    }

    public void LoadSimoneScene()
    {
        SceneManager.LoadScene(SIMONE_SCENE);
    }

    public void LoadRobertoScene()
    {
        SceneManager.LoadScene(ROBERTO_SCENE);
    }

    public void LoadMassimilianoScene()
    {
        SceneManager.LoadScene(MASSIMILIANO_SCENE);
    }
}
