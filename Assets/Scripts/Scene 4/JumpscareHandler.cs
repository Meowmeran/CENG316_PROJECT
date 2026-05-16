using UnityEngine;
using System.Collections;
using System;

public class JumpscareHandler : MonoBehaviour
{
    public GameObject[] entities;
    [SerializeField] private Transform originalPosition;
    [SerializeField] private Camera jumpscareCamera;

    [Header("Settings")]
    [SerializeField] private float duration = 0.6f;
    [SerializeField] private float stoppingDistance = 0.3f; // Don't clip through the lens
    [SerializeField] private float jitterIntensity = 0.15f;
    [SerializeField] private float jitterFrequency = 0.05f; // How often it "jumps"

    void Start()
    {
        if (originalPosition == null)
        {
            if (entities.Length > 0)
            {
                originalPosition = entities[0].transform;
            }
        }
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i].SetActive(false);
        }
    }
    public void TriggerJumpscare(int index)
    {
        if (index >= 0 && index < entities.Length)
        {
            StartCoroutine(AnimateJumpscare(entities[index]));
        }
    }

    private IEnumerator AnimateJumpscare(GameObject entity)
    {
        Vector3 startPos = originalPosition.transform.position;
        
        
        float elapsed = 0f;
        entity.SetActive(true);
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // 1. Calculate the smooth base path
            // Move from start toward a point slightly in front of the camera
            Vector3 targetBasePos = jumpscareCamera.transform.position + (jumpscareCamera.transform.forward * stoppingDistance);
            Vector3 basePosition = Vector3.Lerp(startPos, targetBasePos, t);

            // 2. Calculate the "Jumpy" Offset
            // We use a small timer or random check to create sharp jumps
            Vector3 jitter = Vector3.zero;
            if (UnityEngine.Random.value < 0.3f) // 30% chance per frame to jitter
            {
                // Randomly jump forward or backward along the camera's view axis
                float forwardJitter = UnityEngine.Random.Range(-jitterIntensity, jitterIntensity);
                jitter = jumpscareCamera.transform.forward * forwardJitter;
            }

            // 3. Apply position and ensure it faces the camera
            entity.transform.position = basePosition + jitter;
            entity.transform.LookAt(jumpscareCamera.transform);

            yield return null;
        }

        // Final snap to target
        entity.transform.position = jumpscareCamera.transform.position + (jumpscareCamera.transform.forward * stoppingDistance);
        
        // Optional: Trigger a callback or hide entity after a delay
        yield return new WaitForSeconds(0.1f);
        entity.SetActive(false);
    }
}

public enum Entity
{
    Ambush,
    Gast,
    Smiley
}