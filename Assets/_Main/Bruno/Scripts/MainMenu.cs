using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Canvas settingsCanvas;
    [SerializeField] private Canvas loadCanvas;

    private const string brunoScene = "Assets/_Main/Bruno/Scenes/BrunoScene.unity";
    private const string federicoScene = "Assets/_Main/Federico/Scenes/FedericoScene.unity";
    private const string simoneScene = "Assets/_Main/Simone/Scenes/SimoneScene.unity";
    private const string robertoScene = "Assets/_Main/Roberto/Scenes/RobertoScene.unity";
    private const string massimilianoScene = "Assets/_Main/Massimiliano/Scenes/MassimilianoScene.unity";

    // Start is called before the first frame update
    void Start()
    {
        settingsCanvas.gameObject.SetActive(false);
        loadCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Load the our scenes

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
