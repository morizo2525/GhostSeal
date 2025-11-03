using UnityEngine;

public class PlayerAttack_Bow : MonoBehaviour
{
    [Header("矢の設定")]
    public GameObject arrowPrefab;       // 発射する矢のPrefab
    public Transform shootPoint;        // 発射位置
    public float shootPower = 10f;  // 発射の初速

    public void BowShoot()
    {
        // マウス位置取得（ワールド座標）
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // 発射方向ベクトル
        Vector2 dir = (mouseWorld - shootPoint.position).normalized;

        // 矢を生成
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);

        // 矢の向きを設定
        arrow.transform.right = dir;

        // ArrowRotationスクリプトを追加（飛行中に回転させる）
        arrow.AddComponent<ArrowRotation>();

        // Rigidbody2Dで物理発射
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = dir * shootPower;
        }
    }
}

// 矢の回転を速度に合わせるスクリプト
public class ArrowRotation : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rb != null && rb.linearVelocity.sqrMagnitude > 0.1f)
        {
            // 速度ベクトルの方向に矢を回転
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}