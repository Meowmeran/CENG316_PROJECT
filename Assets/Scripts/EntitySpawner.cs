using System.Collections;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    public GameObject[] entityPrefabs;
    public float[] spawnRates;
    public Bounds spawnArea;
    public float spawnCheckRadius = 1f;
    public float minDistanceToPlayer = 5f;
    public int maxEnemiesAlive = 10;
    public string enemyTag = "Enemy";
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i < entityPrefabs.Length; i++)
        {
            StartCoroutine(SpawnEntity(i));
        }
    }

    IEnumerator SpawnEntity(int index)
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRates[index]);
            if (GetAliveEnemyCount() >= maxEnemiesAlive)
            {
                continue;
            }

            Vector3 spawnPos = GetValidSpawnPosition();
            if (spawnPos != Vector3.zero)
            {
                Instantiate(entityPrefabs[index], spawnPos, Quaternion.identity);
            }
        }
    }

    int GetAliveEnemyCount()
    {
        return GameObject.FindGameObjectsWithTag(enemyTag).Length;
    }

    Vector3 GetValidSpawnPosition()
    {
        for (int attempts = 0; attempts < 10; attempts++)
        {
            Vector3 pos = new Vector3(
                Random.Range(spawnArea.min.x, spawnArea.max.x),
                Random.Range(spawnArea.min.y, spawnArea.max.y),
                Random.Range(spawnArea.min.z, spawnArea.max.z)
            );
            if (!Physics.CheckSphere(pos, spawnCheckRadius) && Vector3.Distance(pos, player.position) > minDistanceToPlayer)
            {
                return pos;
            }
        }
        return Vector3.zero;
    }
}
