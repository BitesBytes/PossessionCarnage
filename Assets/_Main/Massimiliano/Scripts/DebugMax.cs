using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class DebugMax : MonoBehaviour
{
    private HealthManager healthManager;

    private void Start()
    {
        healthManager = HealthManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            healthManager.PossessedCharacterChanged(this.gameObject);
        }
    }
}
