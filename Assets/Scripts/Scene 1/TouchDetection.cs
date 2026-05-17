using UnityEngine;

public class TouchDetection : MonoBehaviour
{
    [SerializeField] float allowance = 0.15f;

    private float touchTimer = 0f;
    private bool touching = false;
    private bool detectedTouch = false;

    public bool IsTouching()
    {
        return detectedTouch;
    }

    void Update()
    {
        if (touching)
        {
            touchTimer += Time.deltaTime;

            if (touchTimer >= allowance && !detectedTouch)
            {
                detectedTouch = true;
            }
        }
        else
        {
            touchTimer = 0f;
            detectedTouch = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Handle"))
        {
            touching = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Handle"))
        {
            touching = false;
        }
    }
}