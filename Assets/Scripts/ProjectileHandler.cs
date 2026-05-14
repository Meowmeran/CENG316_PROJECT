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
        if (CompareTag("Weapon") || CompareTag("Projectile"))
        {
            return; // Ignore collisions with other projectiles or weapons
        }
        if (other.TryGetComponent<Health>(out var health))
        {
            health.TakeDamage(damage);
        }
        else
        {
            if (hitSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(hitSound);
            }
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }
        }
        Destroy(gameObject);
    }

    internal void SetDamage(int weaponDamage)
    {
        damage = weaponDamage;
    }
}
