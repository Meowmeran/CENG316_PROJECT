using UnityEngine;

public class FloatMotion : MonoBehaviour
{
    [Header("Float Settings")]
    public float floatHeight = 10f;
    public float floatSpeed = 1f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        transform.localPosition = new Vector3(
            startPos.x,
            startPos.y + newY,
            startPos.z
        );
    }
}