using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    [Header("Music Parts")]
    [SerializeField] AudioSource musicStart;
    [SerializeField] AudioSource musicCalmLoop;
    [SerializeField] AudioSource musicCalmToIntense;
    [SerializeField] AudioSource musicIntenseLoop;
    [SerializeField] AudioSource musicEnd;

    [Header("Volume")]
    [SerializeField, Range(0f, 1f)] float masterVolume = 1f;

    [Header("Transition Durations")]
    [SerializeField] float calmToIntenseFadeDuration = 1.5f;
    [SerializeField] float endFadeDuration = 1.5f;

    AudioSource currentSource;

    void Start()
    {
        StartCoroutine(StartMusicSequence());
    }

    IEnumerator StartMusicSequence()
    {
        // Start -> Calm: no transition, instant swap
        PlayInstant(musicStart);
        yield return new WaitForSeconds(musicStart.clip.length);

        PlayInstantLoop(musicCalmLoop);
    }

    // ---------- PUBLIC STATES ----------

    [ContextMenu("Start Survival")]
    public void GoIntense()
    {
        // Calm -> calmToIntense: YES crossfade
        // calmToIntense -> Intense: no transition (handled inside Crossfade)
        musicStart.Stop();
        StartCoroutine(Crossfade(currentSource, musicCalmToIntense, calmToIntenseFadeDuration));
    }

    [ContextMenu("End Survival")]
    public void EndMusic()
    {
        // Intense -> End: YES fade out, then instant play
        musicStart.Stop();
        musicCalmLoop.Stop();
        musicCalmToIntense.Stop();
        StartCoroutine(EndSequence());
    }

    // ---------- SEQUENCES ----------

    IEnumerator EndSequence()
    {
        if (musicEnd == null) yield break;

        // Start end clip at 0 and fade it in, while fading out current — in parallel
        musicEnd.loop = false;
        musicEnd.volume = 0f;
        musicEnd.Play();

        float t = 0f;
        float startVol = (currentSource != null) ? currentSource.volume : masterVolume;

        while (t < endFadeDuration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / endFadeDuration);

            if (currentSource != null)
                currentSource.volume = Mathf.Lerp(startVol, 0f, n);

            musicEnd.volume = Mathf.Lerp(0f, masterVolume, n);

            yield return null;
        }

        musicEnd.volume = masterVolume;
        if (currentSource != null)
        {
            currentSource.volume = 0f;
            currentSource.Stop();
        }

        currentSource = musicEnd;
    }

    // ---------- CROSSFADE ----------

    IEnumerator Crossfade(AudioSource fadeOutSource, AudioSource fadeInSource, float duration)
    {
        if (fadeInSource == null) yield break;

        fadeInSource.loop = false;
        fadeInSource.volume = 0f;
        fadeInSource.Play();
        currentSource = fadeInSource;

        float t = 0f;
        float startVol = (fadeOutSource != null) ? fadeOutSource.volume : masterVolume;

        while (t < duration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / duration);

            if (fadeOutSource != null)
                fadeOutSource.volume = Mathf.Lerp(startVol, 0f, n);

            fadeInSource.volume = Mathf.Lerp(0f, masterVolume, n);

            yield return null;
        }

        fadeInSource.volume = masterVolume;
        if (fadeOutSource != null)
        {
            fadeOutSource.volume = 0f;
            fadeOutSource.Stop();
        }

        // calmToIntense -> Intense: wait for remainder of clip, then instant swap
        float remainingTime = fadeInSource.clip.length - duration;
        if (remainingTime > 0f)
            yield return new WaitForSeconds(remainingTime);

        PlayInstantLoop(musicIntenseLoop);
    }

    // ---------- CORE HELPERS ----------

    void PlayInstant(AudioSource source)
    {
        currentSource = source;
        source.volume = masterVolume;
        source.loop = false;
        source.Play();
    }

    void PlayInstantLoop(AudioSource source)
    {
        currentSource = source;
        source.volume = masterVolume;
        source.loop = true;
        source.Play();
    }

    // ---------- FADE SYSTEM ----------

    IEnumerator FadeOutCurrent(float fadeTime)
    {
        if (currentSource == null) yield break;

        float startVol = currentSource.volume;
        float t = 0f;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            currentSource.volume = Mathf.Lerp(startVol, 0f, Mathf.Clamp01(t / fadeTime));
            yield return null;
        }

        currentSource.volume = 0f;
        currentSource.Stop();
    }
}