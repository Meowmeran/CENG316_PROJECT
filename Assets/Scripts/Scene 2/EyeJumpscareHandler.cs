using System.Collections;
using UnityEngine;

public class EyeJumpscareHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera overlayCamera;
    [SerializeField] private Transform cameraOffsetRoot; // IMPORTANT (parent rig offset)
    [SerializeField] private GameObject jumpscareEntity;

    [Header("Settings")]
    [SerializeField] private float jumpscareDuration = 1f;
    [SerializeField] private float jumpscareShakeAmount = 0.2f;
    [SerializeField] private float spawnDistance = 1.5f;

    private Vector3 originalOffsetPos;

    private void Start()
    {
        if (cameraOffsetRoot != null)
            originalOffsetPos = cameraOffsetRoot.localPosition;
    }

    public void PlayJumpscare()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateJumpscare());
    }

    private IEnumerator AnimateJumpscare()
    {
        // Reset state
        cameraOffsetRoot.localPosition = originalOffsetPos;

        // Place entity in front of player
        Transform cam = overlayCamera.transform;
        jumpscareEntity.transform.position =
            cam.position + cam.forward * spawnDistance;

        jumpscareEntity.transform.rotation =
            Quaternion.LookRotation(cam.forward);

        jumpscareEntity.SetActive(true);

        float t = 0f;

        while (t < jumpscareDuration)
        {
            t += Time.deltaTime;

            float intensity = Mathf.Lerp(jumpscareShakeAmount, 0f, t / jumpscareDuration);

            Vector3 shake = new Vector3(
                Random.Range(-intensity, intensity),
                Random.Range(-intensity, intensity),
                0f
            );

            cameraOffsetRoot.localPosition = originalOffsetPos + shake;

            yield return null;
        }

        // Reset everything
        cameraOffsetRoot.localPosition = originalOffsetPos;
        jumpscareEntity.SetActive(false);
    }
}