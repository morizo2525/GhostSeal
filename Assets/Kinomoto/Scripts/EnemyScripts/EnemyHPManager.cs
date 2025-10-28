using UnityEngine;
using System.Collections;

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

    [Tooltip("地上エネミーの死亡アニメーション再生時間")]
    public float groundDeathDuration = 1.0f;

    [Tooltip("空中エネミーの死亡アニメーション再生時間")]
    public float airDeathDuration = 1.0f;

    private AirEnemyMove airEnemy;       // 空中エネミー用のコンポーネント
    private GroundEnemyMove groundEnemy; // 地上エネミー用のコンポーネント
    private int enemyHealth;
    private int enemyMaxHealth;
    private AnimationController animController;
    private bool isDead = false;

    void Start()
    {
        groundEnemy = GetComponent<GroundEnemyMove>();
        airEnemy = GetComponent<AirEnemyMove>();
        animController = GetComponent<AnimationController>();
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
        if (isDead) return; // 既に死亡している場合は無視

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
        if (isDead) return;
        isDead = true;
        Debug.Log("Enemy died");

        // スコアを加算
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnEnemyDefeated(enemyMaxHealth);
        }

        // 移動を停止
        if (groundEnemy != null) groundEnemy.enabled = false;
        if (airEnemy != null) airEnemy.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // 敵タイプに応じて死亡アニメ再生と待機時間設定
        float deathDuration = groundDeathDuration;
        if (groundEnemy != null)
        {
            animController?.GroundEnemyDeathAnim();
            deathDuration = groundDeathDuration;
        }

        StartCoroutine(DestroyAfterAnimation(deathDuration));
    }

    IEnumerator DestroyAfterAnimation(float duration)
    {
        yield return new WaitForSeconds(duration);
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
