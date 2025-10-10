using UnityEngine;

public class GroundEnemySpawnManager : MonoBehaviour
{
    [Header("スポーン設定")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 3f;

    [Header("左側スポーン範囲")]
    [SerializeField] private Vector2 leftSpawnAreaMin = new Vector2(-12f, -3f);
    [SerializeField] private Vector2 leftSpawnAreaMax = new Vector2(-10f, -2f);

    [Header("右側スポーン範囲")]
    [SerializeField] private Vector2 rightSpawnAreaMin = new Vector2(10f, -3f);
    [SerializeField] private Vector2 rightSpawnAreaMax = new Vector2(12f, -2f);

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

        // 左右どちらかをランダムに選択
        bool spawnLeft = Random.value < 0.5f;

        Vector2 spawnPos;
        if (spawnLeft)
        {
            spawnPos = GetRandomPositionInArea(leftSpawnAreaMin, leftSpawnAreaMax);
        }
        else
        {
            spawnPos = GetRandomPositionInArea(rightSpawnAreaMin, rightSpawnAreaMax);
        }

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    Vector2 GetRandomPositionInArea(Vector2 min, Vector2 max)
    {
        float randomX = Random.Range(min.x, max.x);
        float randomY = Random.Range(min.y, max.y);
        return new Vector2(randomX, randomY);
    }

    // Gizmosで左右のスポーン範囲を可視化
    void OnDrawGizmos()
    {
        // 左側の範囲
        Gizmos.color = Color.red;
        Vector2 leftCenter = (leftSpawnAreaMin + leftSpawnAreaMax) / 2f;
        Vector2 leftSize = leftSpawnAreaMax - leftSpawnAreaMin;
        Gizmos.DrawWireCube(leftCenter, leftSize);

        // 右側の範囲
        Gizmos.color = Color.red;
        Vector2 rightCenter = (rightSpawnAreaMin + rightSpawnAreaMax) / 2f;
        Vector2 rightSize = rightSpawnAreaMax - rightSpawnAreaMin;
        Gizmos.DrawWireCube(rightCenter, rightSize);
    }
}
