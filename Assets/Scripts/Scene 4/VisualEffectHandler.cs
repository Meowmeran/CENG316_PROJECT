using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class VisualEffectHandler : MonoBehaviour
{
    public Volume wakeUpVolume;
    public Volume damageVolume;
    public float wakeUpEffectDuration = 3f;
    public float damageEffectDuration = 1f;
    public AudioSource damageAudioSource;
    public AudioClip[] damageSounds;


    void Start()
    {
        damageVolume.weight = 0;
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


    private void PlayDamageSound()
    {
        if (damageAudioSource != null && damageSounds.Length > 0)
        {
            AudioClip clip = damageSounds[Random.Range(0, damageSounds.Length)];
            damageAudioSource.pitch = Random.Range(0.8f, 1.2f); // Add some pitch variation for more variety
            damageAudioSource.PlayOneShot(clip);
        }
    }
}
