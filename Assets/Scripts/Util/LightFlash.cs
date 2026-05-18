using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlash : MonoBehaviour
{
    Light light;
    private float originalIntensity = 0;
    [SerializeField] private float noFlashTime = 0.5f;
    [SerializeField] private float flashTime = 0.1f;
    [SerializeField] private float noFlashInterval = 0.5f;
    [SerializeField] bool smooth = true;
    void Start()
    {
        light = GetComponent<Light>();
        originalIntensity = light.intensity;
        StartCoroutine(FlashRoutine());
    }


    IEnumerator FlashRoutine()
    {
        while (true)
        {

            float t = 0;
            while (t < noFlashTime)
            {
                t += Time.deltaTime;
                light.intensity = smooth ? Mathf.Lerp(originalIntensity, 0, t / noFlashTime) : 0;
                yield return null;
            }

            yield return new WaitForSeconds(noFlashInterval);

            t = 0;
            while (t < flashTime)
            {
                t += Time.deltaTime;
                light.intensity = smooth ? Mathf.Lerp(0, originalIntensity, t / flashTime) : originalIntensity;
                yield return null;
            }
        }
    }

    public void SpeedMultiplier(float multiplier)
    {
        StopAllCoroutines();
        noFlashTime *= multiplier;
        flashTime *= multiplier;
        noFlashInterval *= multiplier;
        StartCoroutine(FlashRoutine());
    }

}
