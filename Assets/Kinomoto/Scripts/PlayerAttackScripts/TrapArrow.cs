using UnityEngine;

public class TrapArrow : MonoBehaviour
{
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private float destroyDelay = 0.1f;  // 着弾後の削除遅延時間
    private Rigidbody2D rb;
    private bool hasCollided = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasCollided)
            return;

        hasCollided = true;

        // 着弾位置にトラップを生成
        CreateTrapAtImpact(collision.gameObject);

        // 矢を削除
        Destroy(gameObject, destroyDelay);
    }

    /// <summary>
    /// 着弾位置にトラップを生成
    /// </summary>
    private void CreateTrapAtImpact(GameObject hitObject)
    {
        // トラップPrefabが設定されていない場合は警告を出して終了
        if (trapPrefab == null)
        {
            Debug.LogWarning("トラップPrefabが設定されていません");
            return;
        }

        // 着弾位置でトラップを生成
        Instantiate(trapPrefab, transform.position, Quaternion.identity);
    }
}
