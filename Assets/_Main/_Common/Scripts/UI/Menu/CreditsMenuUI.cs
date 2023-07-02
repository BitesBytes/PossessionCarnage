using UnityEngine;
using UnityEngine.UI;

public class CreditsMenuUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private MenuManager menuManager;

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            menuManager.CloseLoadMenu();
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
