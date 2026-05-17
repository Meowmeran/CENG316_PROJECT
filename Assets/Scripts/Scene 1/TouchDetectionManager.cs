using UnityEngine;

public class TouchDetectionManager : MonoBehaviour
{
    public TouchDetection[] touchDetection;
    public bool IsTouching()
    {
        foreach (TouchDetection detection in touchDetection)
        {
            if (detection.IsTouching())
            {
                return true;
            }
        }
        return false;
    }

}
