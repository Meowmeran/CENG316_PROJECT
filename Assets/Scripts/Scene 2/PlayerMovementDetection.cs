using UnityEngine;

public class PlayerMovementDetection : MonoBehaviour
{
    [SerializeField] private Transform playerHead;
    [SerializeField] private Transform playerRightHand;
    [SerializeField] private Transform playerLeftHand;

    private Transform lastHeadPosition;
    private Transform lastRightHandPosition;
    private Transform lastLeftHandPosition;

    [SerializeField] private float movementThreshold = 0.1f;
    
    private void Start()
    {
        lastHeadPosition = playerHead;
        lastRightHandPosition = playerRightHand;
        lastLeftHandPosition = playerLeftHand;
    }

    public void CaptureLastPositions()
    {
        lastHeadPosition = playerHead;
        lastRightHandPosition = playerRightHand;
        lastLeftHandPosition = playerLeftHand;
    }

    public bool HasMoved()
    {
        return Mathf.Abs(playerHead.position.x - lastHeadPosition.position.x) > movementThreshold ||
               Mathf.Abs(playerHead.position.y - lastHeadPosition.position.y) > movementThreshold ||
               Mathf.Abs(playerHead.position.z - lastHeadPosition.position.z) > movementThreshold ||
               Mathf.Abs(playerRightHand.position.x - lastRightHandPosition.position.x) > movementThreshold ||
               Mathf.Abs(playerRightHand.position.y - lastRightHandPosition.position.y) > movementThreshold ||
               Mathf.Abs(playerRightHand.position.z - lastRightHandPosition.position.z) > movementThreshold ||
               Mathf.Abs(playerLeftHand.position.x - lastLeftHandPosition.position.x) > movementThreshold ||
               Mathf.Abs(playerLeftHand.position.y - lastLeftHandPosition.position.y) > movementThreshold ||
               Mathf.Abs(playerLeftHand.position.z - lastLeftHandPosition.position.z) > movementThreshold;
    }
}
