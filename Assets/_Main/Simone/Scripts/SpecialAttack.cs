using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    private GameObject attackarea = default;
    private bool canattack = false;
    private float timetoattack = 0.7f;
    private float timer = 0f;
    //private float energyreached;

    private void Start()
    {
        attackarea = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
           
            UltimateAttack();
            Debug.Log("I'm ultimate attacking");

        }


        if (canattack)
        {
            timer += Time.deltaTime;

            if (timer >= timetoattack)
            {
                timer = 0f;
                canattack = false;
                attackarea.SetActive(canattack);

            }
        }
    }

    private void UltimateAttack()
    {
        canattack = true;
        attackarea.SetActive(canattack);
    }

    
}
