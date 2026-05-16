using System.Collections.Generic;
using UnityEngine;

public class FindableGenerator : MonoBehaviour
{
    [SerializeField] GameObject findablePrefab;
    List<GameObject> objects = new List<GameObject>();
    [SerializeField] BoxCollider spawnArea;
    private Bounds bounds;

    [Header("Generation")]
    [SerializeField] float minDistance = 2f;

    void Start()
    {
        objects = new List<GameObject>();
        bounds = spawnArea.bounds;
        if (bounds != null)
        {
            Destroy(spawnArea.gameObject);
        }
    }

    private Vector3 GetRandomPointNotCloseToEachOther(int attempts = 10)
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

    Vector3 GetRandomPoint()
    {
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(randomX, randomY, randomZ);
    }

    public void Generate(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 point = GetRandomPointNotCloseToEachOther();
            GameObject obj = Instantiate(findablePrefab, point, Quaternion.identity);
            objects.Add(obj);
        }
    }

    public void DestroyAllFindables()
    {
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
        objects.Clear();
    }

}
