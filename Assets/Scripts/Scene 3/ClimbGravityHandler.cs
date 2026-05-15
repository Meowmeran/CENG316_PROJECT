using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class ClimbGravityHandler : MonoBehaviour
{
    [SerializeField] private DynamicMoveProvider dynamicMoveProvider;
    int climbCount = 0;

    void Start()
    {
        climbCount = 0;
        if (dynamicMoveProvider == null)
        {
            dynamicMoveProvider = FindFirstObjectByType<DynamicMoveProvider>();

            if (dynamicMoveProvider == null)
            {
                Debug.LogError("DynamicMoveProvider not found in scene.");
            }
        }
    }

    public void OnClimb()
    {
        climbCount++;
        UpdateLogic();
    }
    public void OnRelease()
    {
        climbCount--;
        UpdateLogic();
    }

    private void UpdateLogic()
    {
        dynamicMoveProvider.useGravity = climbCount <= 0;
    }
}
