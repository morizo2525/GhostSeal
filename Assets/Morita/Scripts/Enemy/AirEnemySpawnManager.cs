using UnityEngine;

public class AirEnemySpawnManager : MonoBehaviour
{
    [Header("スポーン設定")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 2f;

    [Header("スポーン範囲設定")]
    [SerializeField] private Vector2 spawnAreaMin = new Vector2(-10f, 5f);
    [SerializeField] private Vector2 spawnAreaMax = new Vector2(10f, 10f);

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("Enemy Prefabが設定されていません");
            return;
        }

        // 矩形範囲内のランダムな位置を取得
        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector2 spawnPos = new Vector2(randomX, randomY);

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    // Gizmosでスポーン範囲を可視化
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector2 center = (spawnAreaMin + spawnAreaMax) / 2f;
        Vector2 size = spawnAreaMax - spawnAreaMin;

        Gizmos.DrawWireCube(center, size);
    }
}
