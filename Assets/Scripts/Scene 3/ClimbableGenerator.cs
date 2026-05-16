using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ClimbableGenerator : MonoBehaviour
{
    
    [SerializeField] private GameObject prefab;
    public Collider[] colliders;
    [SerializeField] private int count = 65;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float angleVariation = 45f;
    [SerializeField] private float minSize = 0.78f;
    [SerializeField] private float maxSize = 1f;
    private List<GameObject> objects;

    void Awake()
    {
        objects = new List<GameObject>();
        generate();
    }
    
    public Quaternion GetRandomHorizontalRotation(float angle = 0)
    {
        return Quaternion.Euler(Random.Range(-angle, angle), Random.Range(0, 360), Random.Range(-angle, angle));
    }
    public Vector3 GetRandomPoint()
    {
        Collider boxCollider = colliders[Random.Range(0, colliders.Length)];
        Bounds bounds = boxCollider.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(randomX, randomY, randomZ);
    }

    public Vector3 GetRandomPointNotCloseToEachOther(int attempts = 10)
    {
        if (attempts <= 0)
        {
            return GetRandomPoint();
        }
        Vector3 point = GetRandomPoint();
        if (Vector3.Distance(point, GetRandomPoint()) < minDistance)
        {
            return GetRandomPointNotCloseToEachOther(attempts - 1);
        }
        return point;
    }

    private void generate()
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 point = GetRandomPointNotCloseToEachOther();
            GameObject obj = Instantiate(prefab, point, GetRandomHorizontalRotation(angleVariation));
            obj.transform.localScale = Vector3.one * Random.Range(minSize, maxSize);
            objects.Add(obj);
        }
    }

    private void DestroyAllClimbables()
    {
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
        objects.Clear();
    }

    [ContextMenu("Regenerate")]
    public void Regenerate()
    {
        DestroyAllClimbables();
        generate();
    }
}
