using System;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        Destroy(gameObject, lifetime); // Destroy projectile after lifetime expires
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        rb.linearVelocity = transform.forward * speed;
    }

    internal void SetValues(float projectileSpeed, int weaponDamage)
    {
        speed = projectileSpeed;
        damage = weaponDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon") || other.CompareTag("Projectile"))
        {
            return; // Ignore collisions with other projectiles or weapons
        }
        else if (other.CompareTag("Player"))
        {
            return; // Ignore collisions with the player (projectiles should not damage the player)
        }
        else if (other.CompareTag("Enemy"))
        {

            if (other.TryGetComponent<EnemyHealth>(out var damageable))
            {
                damageable.TakeDamage(damage);
                Debug.Log("Projectile hit enemy for " + damage + " damage.");
            }
            else
            {
                Debug.LogWarning("Projectile hit an enemy without an EnemyHealth component.");
            }

        }
        else
        {
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }
            if (audioSource != null && hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }
        }
        Destroy(gameObject);
    }

    internal void SetDamage(int weaponDamage)
    {
        damage = weaponDamage;
    }
}
