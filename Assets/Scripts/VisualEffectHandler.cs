using UnityEngine;
using UnityEngine.Rendering;

public class VisualEffectHandler : MonoBehaviour
{
    public Volume volume;
    public float effectDuration = 1f;
    public AudioSource damageAudioSource;
    public AudioClip[] damageSounds;


    void Start()
    {
        volume.weight = 0;
        if (damageAudioSource == null)
        {
            damageAudioSource = GetComponent<AudioSource>();
        }
    }
    public void TakeDamage()
    {
        volume.weight = 1;
        PlayDamageSound();
        StartCoroutine(DecreaseWeight());

    }

    private System.Collections.IEnumerator DecreaseWeight()
    {
        float time = 0;
        while (time < effectDuration)
        {
            volume.weight = Mathf.Lerp(1, 0, time / effectDuration);
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
