using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] private ParticleSystem hitEffect;

    protected override void OnTakeDamage(int amount)
    {
        base.OnTakeDamage(amount);
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
        
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
