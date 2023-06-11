using UnityEngine;
using UnityEngine.UI;

public class LoadMenuUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button loadHealthSceneButton;
    [SerializeField] private Button loadAISceneButton;
    [SerializeField] private Button loadPossessionSceneButton;
    [SerializeField] private Button loadAttackSceneButton;
    [SerializeField] private Button loadTestSceneButton;

    [SerializeField] private MenuManager menuManager;

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            menuManager.CloseLoadMenu();
        });

        loadHealthSceneButton.onClick.AddListener(() =>
        {
            SceneManagementSystem.LoadRobertoScene();
        });

        loadAISceneButton.onClick.AddListener(() =>
        {
            SceneManagementSystem.LoadBrunoScene();
        });

        loadPossessionSceneButton.onClick.AddListener(() =>
        {
            SceneManagementSystem.LoadFedericoScene();
        });

        loadAttackSceneButton.onClick.AddListener(() =>
        {
            SceneManagementSystem.LoadSimoneScene();
        });

        loadTestSceneButton.onClick.AddListener(() =>
        {
            SceneManagementSystem.LoadMassimilianoScene();
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
