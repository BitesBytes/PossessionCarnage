using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;

    [SerializeField] private MenuManager menuManager;

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            menuManager.CloseSettingsMenu();
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
