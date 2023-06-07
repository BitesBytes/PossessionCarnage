using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class HealthMngr : MonoBehaviour
{
    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = Mathf.Clamp(value, 0, maxHealth); }
    }

    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;

    private bool isCoolDown = false;
    private float timeElapsed;

    private void Start()
    {
        CurrentHealth = maxHealth;
    }
    private void Update()
    {
        //decreaseHealthOverTime();

        //takeDmgAfterAwhile(10f, 4f);

        timeElapsed += Time.deltaTime; 
        TakeDmgAfterTime(20f, 4f);

        if (Input.GetKeyDown(KeyCode.E))
        {
            removeHealth(10f);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            restoreHealth(10f);
        }
    }

    private void decreaseHealthOverTime(float amount = 3f)
    {
        removeHealth(amount * Time.deltaTime);
    }

    private void removeHealth(float amount)
    {
        CurrentHealth -= amount;
    }

    private void restoreHealth(float amount)
    {
        CurrentHealth += amount;
    }

    private void takeDmgAfterAwhile(float amount, float time)
    {
        if (isCoolDown)
        {
            return;
        }

        Invoke(nameof(resetCoolDown), time);
        isCoolDown = true;

        CurrentHealth = CurrentHealth - amount;
    }
    private void resetCoolDown()
    {
        isCoolDown = false;
    }

    private void TakeDmgAfterTime(float amount, float timeLimit)
    {
        if (timeElapsed >= timeLimit)
        {
            CurrentHealth -= amount;
            timeElapsed = 0;
        }
    }

}
