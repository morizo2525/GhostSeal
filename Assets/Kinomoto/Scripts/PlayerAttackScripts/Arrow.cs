using UnityEngine;

public class Arrow : MonoBehaviour
{
    //矢のダメージ判定スクリプト（2D用）
    //矢が当たったときに敵にダメージを与える

    [Header("ダメージ設定")]
    [Tooltip("矢が与えるダメージ量")]
    public int damage = 10;

    [Header("破壊設定")]
    [Tooltip("オブジェクトに当たった後に矢を破壊するか")]
    public bool destroyOnHit = true;

    [Tooltip("当たってから破壊するまでの遅延時間（秒）")]
    public float destroyDelay = 0.1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleHit(collision.gameObject);
    }

    // 当たり判定の処理
    private void HandleHit(GameObject hitObject)
    {
        Debug.Log($"矢が {hitObject.name} に当たりました！");

        // 敵タグを持つオブジェクトの場合、ダメージを与える
        if (hitObject.CompareTag("Enemy"))
        {
            EnemyHPManager enemyHP = hitObject.GetComponent<EnemyHPManager>();

            if (enemyHP != null)
            {
                enemyHP.EnemyTakeDamage(damage);
                Debug.Log($"敵にダメージ！ {damage}ダメージ");
            }
            else
            {
                Debug.LogWarning($"{hitObject.name} にEnemyHPManagerがありません！");
            }
        }

        // どんなオブジェクトに当たっても矢を破壊
        if (destroyOnHit)
        {
            Destroy(gameObject, destroyDelay);
        }
    }
}