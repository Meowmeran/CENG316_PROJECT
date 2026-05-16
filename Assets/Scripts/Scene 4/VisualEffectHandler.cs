
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Cinemachine;
using System.Collections;
using System;

public class VisualEffectHandler : MonoBehaviour
{
    [SerializeField] private GameObject CameraOffset;
    public Volume wakeUpVolume;
    public Volume damageVolume;
    public Volume winVolume;
    public Volume deathVolume;
    public float wakeUpEffectDuration = 3f;
    public float damageEffectDuration = 1f;
    public AudioSource damageAudioSource;
    public AudioClip[] damageSounds;
    public GameObject player;
    [SerializeField] private float minDistanceToShake = 10f;
    [SerializeField] private float shakeMultiplier = 0.1f;



    void Start()
    {
        wakeUpVolume.weight = 1;
        winVolume.weight = 0;
        damageVolume.weight = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            player = Camera.main.gameObject;
            if (player == null)
            {
                Debug.LogError("Player not found.");
            }
        }
        if (damageAudioSource == null)
        {
            damageAudioSource = GetComponent<AudioSource>();
        }
        WakeUp();
    }
    [ContextMenu("TakeDamage")]
    public void TakeDamage()
    {
        damageVolume.weight = 1;
        PlayDamageSound();
        StartCoroutine(DecreaseWeight(damageVolume, damageEffectDuration));

    }

    [ContextMenu("WakeUp")]
    public void WakeUp()
    {
        wakeUpVolume.weight = 1;
        StartCoroutine(DecreaseWeight(wakeUpVolume, wakeUpEffectDuration, 3f));
    }

    public void OnWin()
    {
        winVolume.weight = 0;
        wakeUpVolume.weight = 0;
        damageVolume.weight = 0;
        StartCoroutine(IncreaseWeight(winVolume, 10f));
    }
    public void OnDeath()
    {
        wakeUpVolume.weight = 0;
        damageVolume.weight = 0;
        winVolume.weight = 0;
        StartCoroutine(IncreaseWeight(deathVolume, 10f));
    }

    private System.Collections.IEnumerator IncreaseWeight(Volume volume, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            volume.weight = Mathf.Lerp(0, 1, time / damageEffectDuration);
            time += Time.deltaTime;
            yield return null;
        }
        volume.weight = 1;
    }

    private System.Collections.IEnumerator DecreaseWeight(Volume volume, float duration, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        float time = 0;
        while (time < duration)
        {
            volume.weight = Mathf.Lerp(1, 0, time / damageEffectDuration);
            time += Time.deltaTime;
            yield return null;
        }
        volume.weight = 0;
    }

    private void Update()
    {
        ShakeCamera(GetClosestDistanceToPlayer(player.transform.position));
    }

    private float GetClosestDistanceToPlayer(Vector3 playerPosition)
    {
        float minDistance = float.MaxValue;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float distance = Vector3.Distance(enemy.transform.position, playerPosition);
            if (distance < minDistance)
            {
                minDistance = distance;
            }
        }
        return minDistance;
    }

    private void ShakeCamera(float distanceToPlayer)
    {
        if (distanceToPlayer > minDistanceToShake)
        {
            CameraOffset.transform.localPosition = Vector3.zero;
            return;
        }
        
        float shakeAmount = Mathf.Clamp01(1f / (Mathf.Pow(distanceToPlayer, 2) + 0.001f));
        shakeAmount *= shakeMultiplier;

        CameraOffset.transform.localPosition = new Vector3(
            UnityEngine.Random.Range(-shakeAmount, shakeAmount),
            UnityEngine.Random.Range(-shakeAmount, shakeAmount),
            0f
        );
    }


    private void PlayDamageSound()
    {
        if (damageAudioSource != null && damageSounds.Length > 0)
        {
            AudioClip clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
            damageAudioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f); // Add some pitch variation for more variety
            damageAudioSource.PlayOneShot(clip);
        }
    }
}
