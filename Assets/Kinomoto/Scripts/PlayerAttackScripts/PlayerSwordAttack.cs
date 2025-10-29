using UnityEngine;
using System.Collections;

public class PlayerSwordAttack : MonoBehaviour
{
    [Header("攻撃設定")]
    public Animator animator;           // 攻撃アニメーション
    public float attackRange = 1.5f;    // 攻撃範囲
    public LayerMask enemyLayer;        // 敵レイヤー
    public int swordDamage = 20;        // ダメージ量

    [Header("剣の軌道設定")]
    public float swingAngle = 180f;     // 振りの角度（180度で半円）
    public int swingCheckPoints = 5;    // 判定ポイント数（多いほど精密）
    public float swingRadius = 0.4f;    // 各判定ポイントの半径

    [Header("サウンド設定")]
    public AudioClip swordSwingSE;      // 剣を振るSE
    public AudioClip swordHitSE;        // 敵に当たった時のSE
    [Range(0f, 1f)]
    public float seVolume = 1.0f;       // SEの音量
    private AudioSource audioSource;

    [Header("ノックバック設定")]
    public float knockbackForce = 5f;   // ノックバックの力

    [Header("クールダウン設定")]
    public float attackCooldown = 0.5f; // 攻撃間隔（秒）
    private bool isAttacking = false;   // 攻撃中フラグ

    [Header("剣アニメーション設定")]
    public GameObject swordAnimPrefab;                    // 剣のアニメーションプレハブ
    public float swordAnimDuration = 0.5f;           // アニメーション再生時間
    public Vector2 swordOffset = new Vector2(0.5f, 0); // 剣の表示位置オフセット

    void Start()
    {
        // AudioSourceコンポーネントを取得または追加
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking)
        {
            StartCoroutine(SwordAttack());
        }
    }

    public IEnumerator SwordAttack()
    {
        isAttacking = true;

        // 剣を振るSEを再生
        if (swordSwingSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(swordSwingSE, seVolume);
        }

        // 向き判定（右向き:1, 左向き:-1）
        float direction = transform.localScale.x > 0 ? 1 : -1;

        // 攻撃アニメーション再生（必要なら）
        // animator.SetTrigger("Attack");

        if (swordAnimPrefab != null)
        {
            Vector2 swordPos = (Vector2)transform.position + new Vector2(direction * swordOffset.x, swordOffset.y);
            GameObject swordAnim = Instantiate(swordAnimPrefab, swordPos, Quaternion.identity);

            // プレイヤーの子オブジェクトにして追従させる
            swordAnim.transform.SetParent(transform);

            // 向きに応じて反転
            if (direction < 0)
            {
                Vector3 scale = swordAnim.transform.localScale;
                scale.x *= -1;
                swordAnim.transform.localScale = scale;
            }

            // アニメーション再生時間後に削除
            Destroy(swordAnim, swordAnimDuration);
        }

        // 攻撃判定（短時間だけ有効）
        yield return new WaitForSeconds(0.1f); // 攻撃判定が出るタイミングを制御

        // 半月型の攻撃判定
        System.Collections.Generic.HashSet<Collider2D> hitEnemiesSet = new System.Collections.Generic.HashSet<Collider2D>();

        // 頭上から正面、足元にかけての扇形判定
        // 右向き: 90度(上) → 0度(右) → -90度(下)
        // 左向き: 90度(上) → 180度(左) → 270度(下) = -90度(下)
        float startAngle = 90f;   // 頭上から開始
        float endAngle = -90f;    // 足元で終了

        for (int i = 0; i < swingCheckPoints; i++)
        {
            float t = (float)i / (swingCheckPoints - 1);
            float angle = Mathf.Lerp(startAngle, endAngle, t) * Mathf.Deg2Rad;

            // directionを使って向きに応じた位置計算
            Vector2 checkPoint = (Vector2)transform.position + new Vector2(
                Mathf.Cos(angle) * attackRange * direction,
                Mathf.Sin(angle) * attackRange
            );

            // 各ポイントで円形判定
            Collider2D[] hits = Physics2D.OverlapCircleAll(checkPoint, swingRadius, enemyLayer);
            foreach (Collider2D hit in hits)
            {
                hitEnemiesSet.Add(hit);
            }

            // デバッグ用の線を描画
            Debug.DrawLine(transform.position, checkPoint, Color.red, 0.5f);
        }

        bool hitAnyEnemy = false;

        // ヒットした敵にダメージとノックバックを適用
        foreach (Collider2D enemy in hitEnemiesSet)
        {
            EnemyHPManager enemyHP = enemy.GetComponent<EnemyHPManager>();
            if (enemyHP != null)
            {
                enemyHP.EnemyTakeDamage(swordDamage);
                Debug.Log($"剣で敵を攻撃！ {swordDamage}ダメージ");
                hitAnyEnemy = true;
            }

            // ノックバック適用
            Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                Vector2 knockbackDir = (enemy.transform.position - transform.position).normalized;
                enemyRb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);

                // 空中の敵の場合、移動を一時停止
                AirEnemyMove airEnemy = enemy.GetComponent<AirEnemyMove>();
                if (airEnemy != null)
                {
                    airEnemy.ApplyKnockback(0.3f); // 0.3秒間ノックバック状態
                }
            }
        }

        // 敵に当たった時のSEを再生
        if (hitAnyEnemy && swordHitSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(swordHitSE, seVolume);
        }

        // クールダウン待機
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (transform != null)
        {
            // 向き判定（右向き:1, 左向き:-1）
            float direction = transform.localScale.x > 0 ? 1 : -1;

            // 半月型の判定範囲を可視化
            float startAngle = 90f;
            float endAngle = -90f;

            Gizmos.color = Color.red;
            Vector3 prevPoint = transform.position;

            for (int i = 0; i < swingCheckPoints; i++)
            {
                float t = (float)i / (swingCheckPoints - 1);
                float angle = Mathf.Lerp(startAngle, endAngle, t) * Mathf.Deg2Rad;

                Vector2 checkPoint = (Vector2)transform.position + new Vector2(
                    Mathf.Cos(angle) * attackRange * direction,
                    Mathf.Sin(angle) * attackRange
                );

                // 判定ポイントを描画
                Gizmos.DrawWireSphere(checkPoint, swingRadius);

                // 軌道線を描画
                if (i > 0)
                {
                    Gizmos.DrawLine(prevPoint, checkPoint);
                }
                prevPoint = checkPoint;
            }
        }
    }
}