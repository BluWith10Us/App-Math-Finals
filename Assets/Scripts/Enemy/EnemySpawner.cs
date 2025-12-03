using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyInstanced enemyPrefab;
    public float spawnInterval = 2f;
    public Vector2 spawnRangeX = new Vector2(-30, 30);
    public float spawnY = 10f;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > spawnInterval)
        {
            SpawnEnemy();
            timer = 0;
        }
    }

    void SpawnEnemy()
    {
        Vector3 pos = new Vector3(Random.Range(spawnRangeX.x, spawnRangeX.y), spawnY, 0);
        Instantiate(enemyPrefab, pos, Quaternion.identity);
    }
}
