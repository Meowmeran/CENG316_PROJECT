using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class EffectHandlerSpider : MonoBehaviour
{
    public Volume escapeVolume;
    public Volume wakeUpVolume;
    public Volume deathVolume;

    
    void Start()
    {
        wakeUpVolume.weight = 1;
        deathVolume.weight = 0;
        escapeVolume.weight = 0;
        StartCoroutine(SlowlyFadeOut(wakeUpVolume, 3f, 3f));
    }

    public void OnGameOver()
    {
        StartCoroutine(SendDeathEffect(2f));
    }
    public void OnWin()
    {
        StartCoroutine(SlowlyFadeIn(escapeVolume, 3f, 0f));
    }

    IEnumerator SendDeathEffect(float delay = 0)
    {
        wakeUpVolume.weight = 0;
        yield return new WaitForSeconds(delay);
        StartCoroutine(SlowlyFadeIn(deathVolume, 0.2f, 0.01f));
        deathVolume.weight = 1;
        StartCoroutine(SlowlyFadeOut(deathVolume, 5f, 3f));
    }

    IEnumerator SlowlyFadeIn(Volume volume, float duration, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        float time = 0;
        while (time < duration)
        {
            volume.weight = Mathf.Lerp(0, 1, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        volume.weight = 1;
    }
    IEnumerator SlowlyFadeOut(Volume volume, float duration, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        float time = 0;
        while (time < duration)
        {
            volume.weight = Mathf.Lerp(1, 0, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        volume.weight = 0;
    }
}
