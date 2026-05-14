using System;
using UnityEngine;

public class FusionRifleHandler : MonoBehaviour
{
    [Header("Weapon Stats")]
    [SerializeField] private int weaponDamage = 10;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Effects")]
    [SerializeField] private ParticleSystem fireEffect;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioSource audioSource;
    [Header("States")]
    [SerializeField] private bool isFiring = false;
    [SerializeField] private bool canFire = true;
    [Header("References")]
    [SerializeField] private Transform weaponModel; // For shake effect when firing
    [SerializeField] private Transform firePoint; // Where the projectile spawns from
    void Start()
    {
        canFire = true; // temporary, should be set based on player input
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            Debug.LogWarning("AudioSource not assigned for " + gameObject.name + ". Attempting to get from component.");
        }
        if (fireEffect == null)
        {
            Debug.LogWarning("FireEffect not assigned for " + gameObject.name + ".");
        }
        if (projectilePrefab == null)
        {
            Debug.LogWarning("ProjectilePrefab not assigned for " + gameObject.name + ".");
        }
        weaponDamage = Mathf.Max(1, weaponDamage);
        fireRate = Mathf.Max(0.1f, fireRate);
        projectileSpeed = Mathf.Max(0.1f, projectileSpeed);
        
    }
    void Update()
    {
        if (isFiring && canFire)
        {
            FireWeapon();
        }
    }
    
    public void OnFireButtonPressed()
    {
        isFiring = true;
    }
    public void OnFireButtonReleased()
    {
        isFiring = false;
    }

    private void FireWeapon()
    {
        canFire = false;
        ShakeWeapon();  

        // Play fire effect
        if (fireEffect != null)
        {
            fireEffect.Play();
        }

        // Play fire sound
        if (audioSource != null && fireSound != null)
        {
            audioSource.pitch = UnityEngine.Random.Range(0.95f, 1.02f);
            audioSource.PlayOneShot(fireSound);
        }

        // Spawn projectile
        if (projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.forward * projectileSpeed;
            }
            ProjectileHandler handler = projectile.GetComponent<ProjectileHandler>();
            handler.SetValues(projectileSpeed, weaponDamage);
            if (handler != null)
            {
                handler.SetDamage(weaponDamage);
            }
        }

        // Reset firing state after fire rate delay
        Invoke(nameof(ResetFire), fireRate);
    }

    void ShakeWeapon()
    {
        if (weaponModel != null)
        {
            Vector3 originalPosition = weaponModel.localPosition;
            Vector3 shakeOffset = UnityEngine.Random.insideUnitSphere * 0.025f;
            weaponModel.localPosition = originalPosition + shakeOffset;
            Invoke(nameof(ResetWeaponPosition), 0.1f);
        }
    }

    private void ResetWeaponPosition()
    {
        if (weaponModel != null)
        {
            weaponModel.localPosition = Vector3.zero;
        }
    }

    private object ResetFire()
    {
        canFire = true;
        return null;
    }
}
