using UnityEngine;

public class PlayerHealth : Health
{

    [SerializeField] private VisualEffectHandler visualEffectHandler;
    [SerializeField] private GameManagerFinal gameManager;

    void Start()
    {
        if (visualEffectHandler == null)
        {
            Debug.LogError("VisualEffectHandler is not assigned in PlayerHealth.");
        }
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManagerFinal>();

            if (gameManager == null)
            {
                Debug.LogError("GameManagerFinal not found in scene.");
            }

        }
    }
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
        gameManager.OnDeath();
        Debug.Log("Player has died!");
    }


}
