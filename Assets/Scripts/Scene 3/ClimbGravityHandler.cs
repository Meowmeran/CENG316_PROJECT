using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Gravity;

public class ClimbGravityHandler : MonoBehaviour
{
    [SerializeField] private GravityProvider gp;
    int climbCount = 0;

    void Start()
    {
        climbCount = 0;
        if (gp == null)
        {
            gp = FindFirstObjectByType<GravityProvider>();

            if (gp == null)
            {
                Debug.LogError("GravityProvider not found in scene.");
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
        gp.useGravity = climbCount <= 0;
    }
}
