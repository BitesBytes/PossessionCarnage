using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }

        set
        {
            currentHealth = Mathf.Clamp(value, 0f, maxHealth);
            if(CurrentHealth <= 0f)
            {
                Die();
            }
        }
    }

    [SerializeField] private float maxHealth = 1f;
    [SerializeField] private float timerSpeed = 1f;

    private float currentHealth;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    private void DecreaseHealthOverTime()
    {
        CurrentHealth -= Time.deltaTime * timerSpeed;
    }

    private void Die()
    {
        //TODO
    }

    public void ChangeHealth(float amount)
    {
        CurrentHealth += amount;
    }

    public void SetTimerSpeed(float timerSpeed)
    {
        this.timerSpeed = timerSpeed;
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    public float GetCurrentHealthNormalized()
    {
        return CurrentHealth / maxHealth;
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
