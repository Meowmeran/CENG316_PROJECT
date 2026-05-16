using System.Collections.Generic;
using UnityEngine;

public class FindableGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject findablePrefab;
    [SerializeField] private BoxCollider spawnArea;

    [Header("Generation")]
    [SerializeField] private float minDistance = 2f;

    private Bounds bounds;
    private List<Vector3> usedPositions = new List<Vector3>();
    private List<GameObject> objects = new List<GameObject>();

    void Awake()
    {
        bounds = spawnArea.bounds;

        // IMPORTANT: do NOT destroy spawnArea if you need bounds
        if (spawnArea == null)
        {
            Debug.LogError("Spawn area missing!");
        }

        usedPositions.Clear();
    }

    public void Generate(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 point = GetValidPoint();
            GameObject obj = Instantiate(findablePrefab, point, Quaternion.identity);
            objects.Add(obj);
            usedPositions.Add(point);
        }
    }

    private Vector3 GetValidPoint()
    {
        int attempts = 20;

        while (attempts > 0)
        {
            Vector3 candidate = GetRandomPoint();

            if (IsFarEnough(candidate))
                return candidate;

            attempts--;
        }

        return GetRandomPoint();
    }

    private bool IsFarEnough(Vector3 point)
    {
        foreach (var p in usedPositions)
        {
            if (Vector3.Distance(point, p) < minDistance)
                return false;
        }
        return true;
    }

    private Vector3 GetRandomPoint()
    {
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        float z = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(x, y, z);
    }

    public void DestroyAllFindables()
    {
        foreach (var obj in objects)
            Destroy(obj);

        objects.Clear();
        usedPositions.Clear();
    }
}