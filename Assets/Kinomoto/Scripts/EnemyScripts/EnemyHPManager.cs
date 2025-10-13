using UnityEngine;

public class EnemyHPManager : MonoBehaviour
{
    [Header("HP Settings")]
    [Tooltip("敵のHP最小値")]
    public int minHealth = 50;

    [Tooltip("敵のHP最大値")]
    public int maxHealth = 150;

    [Header("Size Settings")]
    [Tooltip("最小HPの時のサイズ倍率")]
    public float minSizeMultiplier = 0.5f;

    [Tooltip("最大HPの時のサイズ倍率")]
    public float maxSizeMultiplier = 2.0f;

    private int enemyHealth;
    private int enemyMaxHealth;

    void Start()
    {
        InitializeEnemy();
    }

    // 敵の初期化（スポーン時に呼び出すことも可能）
    public void InitializeEnemy()
    {
        // ランダムでHPを決定
        enemyMaxHealth = Random.Range(minHealth, maxHealth + 1);
        enemyHealth = enemyMaxHealth;

        // HPに応じてサイズを調整
        AdjustSizeBasedOnHP();

        Debug.Log($"Enemy spawned with HP: {enemyHealth}, Size: {transform.localScale}");
    }

    // HPに基づいてサイズを調整
    void AdjustSizeBasedOnHP()
    {
        // HPの割合を計算（0.0?1.0）
        float healthRatio = (float)(enemyMaxHealth - minHealth) / (maxHealth - minHealth);

        // サイズ倍率を線形補間で計算
        float sizeMultiplier = Mathf.Lerp(minSizeMultiplier, maxSizeMultiplier, healthRatio);

        // サイズを適用
        transform.localScale = Vector3.one * sizeMultiplier;
    }

    public void EnemyTakeDamage(int damage)
    {
        enemyHealth -= damage;

        // デバッグ用
        Debug.Log($"Enemy Health: {enemyHealth}/{enemyMaxHealth}");

        if (enemyHealth <= 0)
        {
            EnemyDie();
        }
    }

    void EnemyDie()
    {
        Debug.Log("Enemy died");
        Destroy(gameObject);
    }

    // 現在のHPを取得（他のスクリプトから参照用）
    public int GetCurrentHealth()
    {
        return enemyHealth;
    }

    // 最大HPを取得（他のスクリプトから参照用）
    public int GetMaxHealth()
    {
        return enemyMaxHealth;
    }
}
