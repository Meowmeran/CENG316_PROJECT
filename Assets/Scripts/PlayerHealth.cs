using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] private VisualEffectHandler visualEffectHandler;

    protected override void OnTakeDamage(int amount)
    {
        base.OnTakeDamage(amount);
        if (visualEffectHandler != null)
        {
            visualEffectHandler.TakeDamage();
        }
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        // Handle player death (e.g., show game over screen, respawn, etc.)
        Debug.Log("Player has died!");
    }
}
