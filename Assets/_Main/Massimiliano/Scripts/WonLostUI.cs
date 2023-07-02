using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WonLostUI : MonoBehaviour
{
    private const string WON_TEXT = "You Won!";
    private const string LOST_TEXT = "You loose!";

    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(() =>
        {
            SceneManagementSystem.PlayGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneManagementSystem.ExitToMainMenu();
        });
    }

    private void Start()
    {
        if (GameManager.MatchWon)
        {
            infoText.text = WON_TEXT;
            infoText.color = Color.green;
        }
        else
        {
            infoText.text = LOST_TEXT;
            infoText.color = Color.red;
        }

        Cursor.visible = true;
    }
}
