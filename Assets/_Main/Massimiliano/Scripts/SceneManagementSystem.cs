using UnityEngine.SceneManagement;

public static class SceneManagementSystem
{
    private const string MAIN_SCENE = "Assets/_Main/_Common/Scenes/MainScene.unity";
    private const string BRUNO_SCENE = "Assets/_Main/Bruno/Scenes/BrunoScene.unity";
    private const string FEDERICO_SCENE = "Assets/_Main/Federico/Scenes/FedericoScene.unity";
    private const string SIMONE_SCENE = "Assets/_Main/Simone/Scenes/SimoneScene.unity";
    private const string ROBERTO_SCENE = "Assets/_Main/Roberto/Scenes/RobertoScene.unity";
    private const string MASSIMILIANO_SCENE = "Assets/_Main/Massimiliano/Scenes/MassimilianoScene.unity";

    public static void PlayGame()
    {
        SceneManager.LoadScene(MAIN_SCENE);
    }

    public static void LoadBrunoScene()
    {
        SceneManager.LoadScene(BRUNO_SCENE);
    }

    public static void LoadFedericoScene()
    {
        SceneManager.LoadScene(FEDERICO_SCENE);
    }

    public static void LoadSimoneScene()
    {
        SceneManager.LoadScene(SIMONE_SCENE);
    }

    public static void LoadRobertoScene()
    {
        SceneManager.LoadScene(ROBERTO_SCENE);
    }

    public static void LoadMassimilianoScene()
    {
        SceneManager.LoadScene(MASSIMILIANO_SCENE);
    }
}
