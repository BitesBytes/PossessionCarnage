using System;
using Unity.VisualScripting;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler<OnHealthAmountChangedEventArgs> OnHealthAmountChanged;
    public class OnHealthAmountChangedEventArgs : EventArgs
    {
        public float amount;
    }
    public event EventHandler OnHealthAmountMaxChanged;
    public event EventHandler OnSetTimerSpeedChanged;
    public event EventHandler OnDie;
    public event EventHandler OnPossessedDie;

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
        OnDie?.Invoke(this, EventArgs.Empty);
    }

    public void ChangeHealthAmount(float amount)
    {
        healthAmount += amount;
        healthAmount = Mathf.Clamp(healthAmount, 0f, maxHealthAmount);

        OnHealthAmountChanged?.Invoke(this, new OnHealthAmountChangedEventArgs { amount = amount });

        if(healthAmount <= 0f)
        {
            Die();
        }
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

    public float GetCurrentHealth()
    {
        return healthAmount;
    }

    public bool IsNotMaxHealth()
    {
        return healthAmount < maxHealthAmount;
    }

    public void PossessedDie()
    {
        OnPossessedDie?.Invoke(this, EventArgs.Empty);
    }

    public void Init(float amountMax, float timerSpeed)
    {
        SetHealthAmountMax(amountMax);
        SetTimerSpeed(timerSpeed);

        healthAmount = amountMax;
    }
}
