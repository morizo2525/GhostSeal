using UnityEngine;

public class BoomTrap : MonoBehaviour
{
    //地雷のスクリプト

    [Header("爆発の設定")]
    public float explosionRadius = 5f;      // 爆発の範囲
    public int explosionDamage = 50;        // 爆発ダメージ
    public LayerMask enemyLayer;            // 敵のレイヤー

    [Header("ノックバックの設定")]
    public float knockbackForce = 5f;       // ノックバックの力
    public bool affectPlayer = true;        // プレイヤーもノックバックするか

    [Header("エフェクト（オプション）")]
    public GameObject explosionEffect;      // 爆発エフェクトのPrefab

    [Header("起動設定")]
    public float activationDelay = 0.5f;    // 設置後の起動までの遅延時間（誤爆防止）

    private bool isActivated = false;       // 地雷が起動状態か
    private bool hasExploded = false;       // 既に爆発したか（重複防止）

    void Start()
    {
        // 設置後、少し待ってから起動状態にする（プレイヤーの誤爆防止）
        Invoke(nameof(ActivateTrap), activationDelay);
    }

    /// <summary>
    /// 地雷を起動状態にする
    /// </summary>
    void ActivateTrap()
    {
        isActivated = true;
        Debug.Log("地雷が起動しました");
    }

    /// <summary>
    /// 敵が触れたときの処理
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        // まだ起動していない、または既に爆発している場合は何もしない
        if (!isActivated || hasExploded)
        {
            return;
        }

        // 敵レイヤーのオブジェクトが触れたか確認
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            Debug.Log($"{other.gameObject.name} が地雷を踏みました！");
            Explode();
        }
    }

    /// <summary>
    /// 爆発処理：範囲内の敵にダメージとノックバックを与える
    /// </summary>
    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

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
        Debug.Log($"地雷が爆発！ {hitEnemies.Length}体の敵にダメージ");

        // 地雷オブジェクトを削除
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