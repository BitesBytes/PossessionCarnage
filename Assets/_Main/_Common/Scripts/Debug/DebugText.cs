using TMPro;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;

    private static float damageDebug;

    private void Start()
    {
        debugText.text = "";
    }

    private void Update()
    {
        debugText.text = damageDebug.ToString();
    }

    public static void SetDamageDebug(float value)
    {
        damageDebug = value;
    }
}
