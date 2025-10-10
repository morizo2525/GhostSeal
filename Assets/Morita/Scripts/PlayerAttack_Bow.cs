using UnityEngine;

public class PlayerAttack_Bow : MonoBehaviour
{
    [Header("矢の設定")]
    public GameObject arrowPrefab;      // 発射する矢のPrefab
    public Transform shootPoint;        // 発射位置
    public float shootPower = 10f;      // 発射の初速

    [Header("射角制限（上方向の可動域）")]
    [Tooltip("この角度範囲内（上方向）でのみ発射できる")]
    [Range(0, 120)] public float minAngle = 15f;
    [Range(0, 120)] public float maxAngle = 75f;

    public void BowShoot()
    {
        // マウス位置取得（ワールド座標）
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // 発射方向ベクトル
        Vector2 dir = (mouseWorld - shootPoint.position).normalized;

        // 下方向の場合は発射しない
        if (dir.y <= 0)
            return;

        // マウスが左右どちら側にあるかを判定
        bool isLeftSide = dir.x < 0;

        // 基準方向（マウス側の水平方向）
        Vector2 baseDir = isLeftSide ? Vector2.left : Vector2.right;

        // 発射角度を計算（水平方向から上向きの角度）
        float angle = Vector2.Angle(baseDir, dir);

        // 角度を可動域内にクランプ（制限）
        angle = Mathf.Clamp(angle, minAngle, maxAngle);

        // クランプされた角度で方向ベクトルを再計算
        if (isLeftSide)
        {
            // 左側: 180度から角度を引く
            dir = Quaternion.Euler(0, 0, 180 - angle) * Vector2.right;
        }
        else
        {
            // 右側: そのまま角度を使う
            dir = Quaternion.Euler(0, 0, angle) * Vector2.right;
        }

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
