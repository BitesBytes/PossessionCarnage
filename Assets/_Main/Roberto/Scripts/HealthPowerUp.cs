using UnityEngine;

public class HealthPowerUp : MonoBehaviour
{
    private HealthSystem healthManager;

    public float HealAmount;

    private void Start()
    {
        healthManager = FindAnyObjectByType<HealthSystem>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            healthManager.CurrentHealth += HealAmount;
        }
    }
}