using UnityEngine;

public class JumpscareHandlerTouch : MonoBehaviour
{
    public GameObject jumpscare;
    public void Jumpscare()
    {
        jumpscare.SetActive(true);
    }
}
