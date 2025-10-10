using UnityEngine;

public class BoomArrow : MonoBehaviour
{
    //着弾時に爆発する矢の挙動
    public GameObject explosionEffect;
    public float explosionRadius = 4f;
    public int explosionDamage = 40;
    public LayerMask enemyLayer;
    public float knockbackForce = 5f;
    public bool affectPlayer = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 着弾した瞬間に爆発
        Explode();
    }

    void Explode()
    {
        // 爆発エフェクト生成
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // 範囲内の敵を取得
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHPManager enemyHP = enemy.GetComponent<EnemyHPManager>();
            if (enemyHP != null)
            {
                enemyHP.EnemyTakeDamage(explosionDamage);
            }

            // ノックバック
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 dir = (enemy.transform.position - transform.position).normalized;
                rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
            }
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
                    Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 dir = (player.transform.position - transform.position).normalized;
                        rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
                    }
                }
            }
        }

        // デバッグ出力
        Debug.Log($"爆弾矢が爆発！ 敵数: {hitEnemies.Length}");

        // 矢を削除
        Destroy(gameObject);
    }

    // エディタ上で範囲確認
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
