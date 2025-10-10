using UnityEngine;

public class BombBoom : MonoBehaviour
{
    [Header("爆発の設定")]
    public float explosionDelay = 3f;       // 爆発までの時間（秒）
    public float explosionRadius = 5f;      // 爆発の範囲
    public int explosionDamage = 50;        // 爆発ダメージ
    public LayerMask enemyLayer;            // 敵のレイヤー

    [Header("ノックバックの設定")]
    public float knockbackForce = 5f;       // ノックバックの力
    public bool affectPlayer = true;        // プレイヤーもノックバックするか

    [Header("エフェクト（オプション）")]
    public GameObject explosionEffect;      // 爆発エフェクトのPrefab

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= explosionDelay)
        {
            Explode();
        }
    }

    /// <summary>
    /// 爆発処理：範囲内の敵にダメージを与える
    /// </summary>
    void Explode()
    {
        // 爆発エフェクトを生成（設定されている場合）
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // 範囲内の敵を検索してダメージ＋ノックバック
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            // ダメージ処理
            EnemyHPManager enemyHP = enemy.GetComponent<EnemyHPManager>();
            if (enemyHP != null)
            {
                enemyHP.EnemyTakeDamage(explosionDamage);
            }

            // ノックバック処理
            ApplyKnockback(enemy.gameObject);
        }

        // プレイヤーへのノックバック
        if (affectPlayer)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance <= explosionRadius)
                {
                    ApplyKnockback(player);
                }
            }
        }

        // デバッグ用
        Debug.Log($"爆発！ {hitEnemies.Length}体の敵にダメージ");

        // 爆弾オブジェクトを削除
        Destroy(gameObject);
    }

    /// <summary>
    /// ノックバックを適用する
    /// </summary>
    void ApplyKnockback(GameObject target)
    {
        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // 爆発中心からターゲットへの方向
        Vector2 direction = (target.transform.position - transform.position).normalized;

        // 距離に応じてノックバック力を減衰
        float distance = Vector2.Distance(transform.position, target.transform.position);
        float forceFalloff = 1f - (distance / explosionRadius); // 遠いほど弱くなる

        // ノックバックを適用
        rb.AddForce(direction * knockbackForce * forceFalloff, ForceMode2D.Impulse);
    }

    // Gizmoで爆発範囲を表示（エディタ上で確認用）
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}