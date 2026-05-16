using UnityEngine;

public class PlayerMovementDetection : MonoBehaviour
{
    [Header("Tracked Objects")]
    [SerializeField] private Transform playerHead;
    [SerializeField] private Transform playerRightHand;
    [SerializeField] private Transform playerLeftHand;

    [Header("Settings")]
    [SerializeField] private float movementThreshold = 0.1f;

    private Vector3 lastHeadPosition;
    private Vector3 lastRightHandPosition;
    private Vector3 lastLeftHandPosition;

    private void Start()
    {
        CaptureLastPositions();
    }

    // =====================================================
    // SNAPSHOT CURRENT WORLD POSITIONS
    // =====================================================
    public void CaptureLastPositions()
    {
        if (playerHead != null)
            lastHeadPosition = playerHead.position;

        if (playerRightHand != null)
            lastRightHandPosition = playerRightHand.position;

        if (playerLeftHand != null)
            lastLeftHandPosition = playerLeftHand.position;
    }

    // =====================================================
    // MOVEMENT CHECK (WORLD SPACE)
    // =====================================================
    public bool HasMoved()
    {
        if (playerHead == null || playerRightHand == null || playerLeftHand == null)
            return false;

        return
            Vector3.Distance(playerHead.position, lastHeadPosition) > movementThreshold ||
            Vector3.Distance(playerRightHand.position, lastRightHandPosition) > movementThreshold ||
            Vector3.Distance(playerLeftHand.position, lastLeftHandPosition) > movementThreshold;
    }
}