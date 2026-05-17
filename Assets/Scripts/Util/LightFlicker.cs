using System.Collections;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    Light targeLight;
    private float originalIntensity;
    [SerializeField] float flickerInterval = 0.1f;
    [SerializeField] float flickerIntensity = 3f;
    void Start()
    {
        if (gameObject.TryGetComponent<Light>(out targeLight))
        {
            originalIntensity = targeLight.intensity;
            StartCoroutine(Flicker());       
        }
        else
        {
            Debug.LogError("No light component found on " + gameObject.name);
        }
    }
    void OnEnable()
    {
        if (targeLight != null)
        {
            StartCoroutine(Flicker());
        }
    }
    void OnDisable()
    {
        if (targeLight != null)
        {
            StopAllCoroutines();
        }
    }
    IEnumerator Flicker()
    {
        while (true)
        {
            targeLight.intensity = originalIntensity + Random.Range(-flickerIntensity, flickerIntensity);
            yield return new WaitForSeconds(flickerInterval);
        }
    }
}
