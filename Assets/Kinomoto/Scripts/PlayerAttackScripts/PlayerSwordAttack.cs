using UnityEngine;
using System.Collections;

public class PlayerSwordAttack : MonoBehaviour
{
    [Header("攻撃設定")]
    public Animator animator;           // 攻撃アニメーション
    public float attackRange = 1.0f;    // 攻撃範囲
    public LayerMask enemyLayer;        // 敵レイヤー
    public GameObject attackEffect;     // 攻撃エフェクト
    public int swordDamage = 20;        // ダメージ量

    [Header("ノックバック設定")]
    public float knockbackForce = 5f;   // ノックバックの力

    [Header("クールダウン設定")]
    public float attackCooldown = 0.5f; // 攻撃間隔（秒）
    private bool isAttacking = false;   // 攻撃中フラグ

    [Header("剣アニメーション設定")]
    public GameObject swordAnimPrefab;                    // 剣のアニメーションプレハブ
    public float      swordAnimDuration = 0.5f;           // アニメーション再生時間
    public Vector2    swordOffset = new Vector2(0.5f, 0); // 剣の表示位置オフセット

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

        // 向き判定
        float direction = transform.localScale.x < 0 ? -1 : 1;

        // 攻撃アニメーション再生（必要なら）
        // animator.SetTrigger("Attack");

        if (swordAnimPrefab != null)
        {
            Vector2    swordPos  = (Vector2)transform.position + new Vector2(direction * swordOffset.x, swordOffset.y);
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

        // 攻撃エフェクト生成
        Vector2 attackPos = (Vector2)transform.position + new Vector2(direction * attackRange, 0);
        if (attackEffect != null)
        {
            Instantiate(attackEffect, attackPos, Quaternion.identity);
        }

        // 攻撃判定（短時間だけ有効）
        yield return new WaitForSeconds(0.1f); // 攻撃判定が出るタイミングを制御

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPos, 0.5f, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHPManager enemyHP = enemy.GetComponent<EnemyHPManager>();
            if (enemyHP != null)
            {
                enemyHP.EnemyTakeDamage(swordDamage);
                Debug.Log($"剣で敵を攻撃！ {swordDamage}ダメージ");
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

        Debug.DrawLine(transform.position, attackPos, Color.red, 0.5f);

        // クールダウン待機
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (transform != null)
        {
            float direction = transform.localScale.x < 0 ? -1 : 1;
            Vector2 attackPos = (Vector2)transform.position + new Vector2(direction * attackRange, 0);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPos, 0.5f);
        }
    }
}
