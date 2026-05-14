using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;

    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void SetMaxHealth(int value)
    {
        maxHealth = Mathf.Max(1, value);
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || IsDead())
            return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || IsDead())
            return;

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        
        if (IsDead())
        {
            OnDeath();
        }
        else
        {
            OnTakeDamage(amount);
        }
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    protected virtual void OnEnable()
    {
        currentHealth = maxHealth;
    }
    
    protected virtual void OnTakeDamage(int amount)
    {
        // Override in subclasses to handle damage taken.
    }

    protected virtual void OnDeath()
    {
        // Override in subclasses to handle death.
    }
}
