using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private SoundInstancer soundInstancer;
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
        soundInstancer.PlaySound();
        base.OnTakeDamage(amount);

    }
    protected override void OnDeath()
    {
        base.OnDeath();
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        soundInstancer.PlaySound();
        gameManager.OnKill();
        Destroy(gameObject);
    }
    public void OnDisappear()
    {
        Destroy(gameObject);
    }
}
