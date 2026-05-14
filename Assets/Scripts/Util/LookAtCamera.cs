using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private bool lookAtCamera = true;
    [SerializeField] private bool onlyYRotation = false;
    [SerializeField] private bool inverseRotation = true;
    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera not found. Please assign a camera to LookAtCamera script.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lookAtCamera)
        {
            LookAtPlayer();
        }
    }

    void LookAtPlayer()
    {
        if (mainCamera != null)
        {
            Vector3 targetPosition = mainCamera.transform.position;
            if (onlyYRotation)
            {
                targetPosition.y = transform.position.y; // Keep the y position unchanged
            }
            if (inverseRotation)
            {
                transform.LookAt(targetPosition, Vector3.up);
            }
            else
            {
                transform.LookAt(targetPosition);
            }
        }
    }
}
