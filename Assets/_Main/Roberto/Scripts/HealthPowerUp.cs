using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : MonoBehaviour
{
    private HealthMngr healthManager;
    public float HealAmount;

    private void Start()
    {
        healthManager = FindAnyObjectByType<HealthMngr>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            healthManager.CurrentHealth += HealAmount;
        }
    }
}
//sample code logic