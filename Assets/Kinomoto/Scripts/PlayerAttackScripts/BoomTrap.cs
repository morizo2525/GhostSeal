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
    public float maxKnockbackVelocity = 15f; // ノックバック後の最大速度
    public float playerKnockbackCooldown = 0.3f; // プレイヤーのノックバック無敵時間

    [Header("エフェクト（オプション）")]
    public GameObject explosionEffect;      // 爆発エフェクトのPrefab

    [Header("起動設定")]
    public float activationDelay = 0.5f;    // 設置後の起動までの遅延時間（誤爆防止）

    [Header("サウンド設定")]
    public AudioClip explosionSE;           // 爆発SE
    [Range(0f, 1f)]
    public float seVolume = 1.0f;           // SEの音量

    private bool isActivated = false;       // 地雷が起動状態か
    private bool hasExploded = false;       // 既に爆発したか（重複防止）

    // プレイヤーの最後のノックバック時刻を記録（static変数で全地雷で共有）
    private static float lastPlayerKnockbackTime = -999f;

    void Start()
    {
        //Playerとの当たり判定を無くす
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>());
        // 設置後、少し待ってから起動状態にする（プレイヤーの誤爆防止）
        Invoke(nameof(ActivateTrap), activationDelay);
    }

    /// <summary>
    /// 地雷を起動状態にして、プレイヤーとの当たり判定を復活する
    /// </summary>
    void ActivateTrap()
    {
        isActivated = true;
        // プレイヤーとの当たり判定を復活
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(), false);
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
        // プレイヤーが触れた場合も爆発させる
        else if (affectPlayer && other.CompareTag("Player"))
        {
            Debug.Log("プレイヤーが地雷を踏みました！");
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

        // 爆発SEを再生
        if (explosionSE != null)
        {
            AudioSource.PlayClipAtPoint(explosionSE, transform.position, seVolume);
        }

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
            ApplyKnockback(enemy.gameObject, false);
        }

        // プレイヤーへのノックバック（制限付き）
        if (affectPlayer)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance <= explosionRadius)
                {
                    ApplyKnockback(player, true);
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
    void ApplyKnockback(GameObject target, bool isPlayer)
    {
        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // プレイヤーの場合、クールダウン中なら無視
        if (isPlayer)
        {
            if (Time.time - lastPlayerKnockbackTime < playerKnockbackCooldown)
            {
                Debug.Log("プレイヤーのノックバッククールダウン中");
                return;
            }
            lastPlayerKnockbackTime = Time.time;
        }

        // 爆発中心からターゲットへの方向
        Vector2 direction = (target.transform.position - transform.position).normalized;

        // 距離に応じてノックバック力を減衰
        float distance = Vector2.Distance(transform.position, target.transform.position);
        float forceFalloff = 1f - (distance / explosionRadius); // 遠いほど弱くなる

        // ノックバックを適用
        rb.AddForce(direction * knockbackForce * forceFalloff, ForceMode2D.Impulse);

        // 速度の上限を適用（プレイヤーのみ、または全体）
        if (isPlayer)
        {
            // 次フレームで速度制限を適用
            StartCoroutine(LimitVelocity(rb));
        }
    }

    /// <summary>
    /// 速度を制限する
    /// </summary>
    private System.Collections.IEnumerator LimitVelocity(Rigidbody2D rb)
    {
        yield return new WaitForFixedUpdate();

        if (rb != null && rb.velocity.magnitude > maxKnockbackVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxKnockbackVelocity;
            Debug.Log($"ノックバック速度を制限: {maxKnockbackVelocity}");
        }
    }

    // Gizmoで爆発範囲を表示（エディタ上で確認用）
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}