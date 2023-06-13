using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public static event EventHandler OnHealthAmountChanged;
    public event EventHandler OnHealthAmountMaxChanged;
    public event EventHandler OnSetTimerSpeedChanged;

    private float timerSpeed;
    private float maxHealthAmount;
    private float healthAmount;

    public void DecreaseHealthOverTime()
    {
        float amount = Time.deltaTime * timerSpeed;
        ChangeHealthAmount(-amount);
    }

    private void Die()
    {
        //TODO
    }

    public void ChangeHealthAmount(float amount)
    {
        healthAmount += amount;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealthAmount);

        OnHealthAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetTimerSpeed(float timerSpeed)
    {
        this.timerSpeed = timerSpeed;

        OnSetTimerSpeedChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetHealthAmountMax(float maxHealth)
    {
        this.maxHealthAmount = maxHealth;

        OnHealthAmountMaxChanged?.Invoke(this, EventArgs.Empty);
    }

    public float GetCurrentHealthNormalized()
    {
        return healthAmount / maxHealthAmount;
    }

    public bool IsNotMaxHealth()
    {
        if(healthAmount < maxHealthAmount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Init(float amountMax, float timerSpeed)
    {
        SetHealthAmountMax(amountMax);
        SetTimerSpeed(timerSpeed);

        healthAmount = amountMax;
    }

    #region _OLD
    //REFACTORING
    //private float timeElapsed;
    //private void TakeDamageAfterTime(float amount, float timeLimit)
    //{
    //    if (timeElapsed >= timeLimit)
    //    {
    //        CurrentHealth -= amount;
    //        timeElapsed = 0;
    //    }
    //}
    #endregion
}
