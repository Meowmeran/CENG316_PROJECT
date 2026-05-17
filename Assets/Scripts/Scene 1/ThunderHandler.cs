using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SoundInstancer))]
public class ThunderHandler : MonoBehaviour
{
    [SerializeField] private Light[] lights;
    private SoundInstancer soundInstancer;
    [SerializeField] private float lightningMinDelay = 5f;
    [SerializeField] private float lightningMaxDelay = 20f;
    [SerializeField] private float lightningSoundMinDelay = 1f;
    [SerializeField] private float lightningSoundMaxDelay = 5f;
    [SerializeField] private float lightningMinDuration = 0.6f;
    [SerializeField] private float lightningMaxDuration = 2f;
    void Start()
    {
        soundInstancer = GetComponent<SoundInstancer>();
        DisableAllights();
        StartCoroutine(LightningRoutine());
    }



    IEnumerator LightningRoutine()
    {
        while (true)
        {
            Light randomLight = getRandomLight();
            randomLight.enabled = true;
            yield return new WaitForSeconds(Random.Range(lightningMinDuration, lightningMaxDuration));
            randomLight.enabled = false;
            float soundDelay = Random.Range(lightningSoundMinDelay, lightningSoundMaxDelay);
            soundInstancer.PlaySound(soundDelay);
            yield return new WaitForSeconds(Random.Range(lightningMinDelay, lightningMaxDelay));
        }
    }

    private Light getRandomLight()
    {
        return lights[Random.Range(0, lights.Length)];
    }
    private void DisableAllights()
    {
        foreach (Light light in lights)
        {
            light.enabled = false;
        }
    }
    void OnDestroy()
    {
        StopAllCoroutines();
    }
}
