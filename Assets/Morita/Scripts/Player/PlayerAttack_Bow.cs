using UnityEngine;

public class PlayerAttack_Bow : MonoBehaviour
{
    [Header("矢の設定")]
    public GameObject arrowPrefab;       // 発射する矢のPrefab
    public Transform  shootPoint;        // 発射位置
    public float      shootPower = 10f;  // 発射の初速

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

        // Rigidbody2Dで物理発射
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = dir * shootPower;
        }
    }
}