using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CloseAttack : MonoBehaviour
{
    private GameObject attackarea = default;
    private bool canattack = false;
    private float timetoattack = 0.25f;
    private float timer = 0f;
    
    private void Start()
    {
        attackarea = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
       if (Input.GetKeyDown(KeyCode.Space)) 
       {
            LightAttack();
            Debug.Log("I'm attacking light close");

       }

       if (Input.GetKeyDown(KeyCode.E))
       {
            HeavyAttack();
            Debug.Log("I'm attacking heavy close");
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



