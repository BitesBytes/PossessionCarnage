using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongAttack : MonoBehaviour
{
    private GameObject attackarea = default;
    private bool canattack = false;
    private float timetoattack = 0.5f;
    private float timer = 0.25f;
    



    private void Start()
    {
        attackarea = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            LightAttack();
            Debug.Log("I'm attacking Long");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            HeavyAttack();
            Debug.Log("I'm attacking Long");
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

    private void LightAttack()
    {
        canattack = true;       
        attackarea.SetActive(canattack);
    }

    private void HeavyAttack()
    {
        canattack = true;       
        attackarea.SetActive(canattack);
    }
}
