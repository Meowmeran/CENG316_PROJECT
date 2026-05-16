using UnityEngine;

[RequireComponent(typeof(SoundInstancer))]
public class WalkHandler : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("How far the player must move (in meters) before a footstep plays.")]
    [SerializeField] private float stepDistance = 1.2f; 

    private SoundInstancer soundInstancer;
    private Vector3 lastPosition;
    private float distanceTraveled;

    private void Start()
    {
        // Automatically grab the SoundInstancer component attached to this GameObject
        soundInstancer = GetComponent<SoundInstancer>();
        
        // Initialize position tracking
        lastPosition = transform.position;
    }

    private void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        Vector3 currentPosition = transform.position;
        
        // Calculate distance moved on the horizontal plane (ignoring vertical movement)
        Vector3 deltaPosition = currentPosition - lastPosition;
        deltaPosition.y = 0; 

        distanceTraveled += deltaPosition.magnitude;
        lastPosition = currentPosition;

        // Trigger footstep if the player has walked the stride distance
        if (distanceTraveled >= stepDistance)
        {
            soundInstancer.PlaySound();
            distanceTraveled = 0f; // Reset tracker
        }
    }
}