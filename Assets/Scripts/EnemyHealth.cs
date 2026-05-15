using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] private ParticleSystem hitEffect;
    GameManagerFinal gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManagerFinal>();
        if (gameManager == null)
        {
            Debug.LogError("GameManagerFinal not found in scene.");
        }
    }
    protected override void OnTakeDamage(int amount)
    {
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
        base.OnTakeDamage(amount);

    }
    protected override void OnDeath()
    {
        base.OnDeath();
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        gameManager.OnKill();
        Destroy(gameObject);
    }
}
