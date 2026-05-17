using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class SoundInstancer : MonoBehaviour
{
    [SerializeField] private GameObject sound;
    [SerializeField] AudioClip[] clips;
    [Header("Volume")]
    [SerializeField] float minVolume = 0.1f;
    [SerializeField] float maxVolume = 1f;
    [Header("Pitch")]
    [SerializeField] float minPitch = 0.95f;
    [SerializeField] float maxPitch = 1.05f;
    [Header("Other")]
    [SerializeField] float delay = 0f;
    [SerializeField] float fadeOut = 1f;
    [SerializeField] float destroyTime = 3f;

    private void Start()
    {
        if (sound == null) Destroy(gameObject);
        if (fadeOut < destroyTime) destroyTime = fadeOut + 0.05f;
    }
    [ContextMenu("Play")]

    public void PlaySound()
    {
        GameObject gm = Instantiate(sound, transform.position, Quaternion.identity);
        AudioSource audioSource = gm.GetComponent<AudioSource>();
        audioSource.clip = clips[Random.Range(0, clips.Length)];

        float randomVolume = Random.Range(minVolume, maxVolume);
        audioSource.volume = randomVolume;

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayDelayed(delay);

        StartCoroutine(fadeOutSound(audioSource, randomVolume));
        Destroy(gm, destroyTime);
    }

    private IEnumerator fadeOutSound(AudioSource audioSource, float startVolume)
    {
        float t = 0f;
        while (t < fadeOut)
        {
            if (audioSource == null) yield break; 

            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeOut);
            yield return null;
        }
    }

    public void PlaySound(float delayTime = 0f)
    {
        
        GameObject gm = Instantiate(sound, transform.position, Quaternion.identity);
        AudioSource audioSource = gm.GetComponent<AudioSource>();
        audioSource.clip = clips[Random.Range(0, clips.Length)];

        float randomVolume = Random.Range(minVolume, maxVolume);
        audioSource.volume = randomVolume;

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayDelayed(delayTime);

        StartCoroutine(fadeOutSound(audioSource, randomVolume));
        Destroy(gm, destroyTime);
    }

}
