using UnityEngine;

public class PlayerBombAttack : MonoBehaviour
{
    [Header("爆弾の設定")]
    public GameObject bombPrefab;           // 投擲する爆弾のPrefab
    public Transform throwPoint;            // 投擲位置
    public float throwPower = 8f;           // 投擲の初速

    [Header("投擲角度制限")]
    [Tooltip("この角度範囲内でのみ投擲できる")]
    [Range(0, 120)] public float minAngle = 15f;
    [Range(0, 120)] public float maxAngle = 75f;

    [Header("デバッグ設定")]
    public bool showTrajectory = true;      // 軌道予測を表示
    public Color trajectoryColor = Color.yellow;
    public int trajectoryPoints = 30;       // 軌道の点の数

    /// <summary>
    /// 爆弾を投擲する
    /// </summary>
    public void ThrowBomb()
    {
        if (bombPrefab == null)
        {
            Debug.LogError("爆弾のPrefabが設定されていません！");
            return;
        }

        if (throwPoint == null)
        {
            Debug.LogError("投擲位置（ThrowPoint）が設定されていません！");
            return;
        }

        // マウス位置取得（ワールド座標）
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // 投擲方向ベクトル
        Vector2 dir = (mouseWorld - throwPoint.position).normalized;

        // 下方向の場合は投擲しない
        if (dir.y <= 0)
        {
            Debug.Log("爆弾は下方向に投げられません");
            return;
        }

        // マウスが左右どちら側にあるかを判定
        bool isLeftSide = dir.x < 0;

        // 基準方向（マウス側の水平方向）
        Vector2 baseDir = isLeftSide ? Vector2.left : Vector2.right;

        // 投擲角度を計算（水平方向から上向きの角度）
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

        // 爆弾を生成
        GameObject bomb = Instantiate(bombPrefab, throwPoint.position, Quaternion.identity);

        // 爆弾の向きを設定（回転させたい場合）
        bomb.transform.right = dir;

        // Rigidbody2Dで物理投擲
        Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = dir * throwPower;
        }
        else
        {
            Debug.LogWarning("爆弾PrefabにRigidbody2Dがアタッチされていません");
        }

        Debug.Log($"爆弾を投擲しました。角度: {angle:F1}度");
    }

    /// <summary>
    /// 軌道予測を描画（デバッグ用）
    /// </summary>
    void OnDrawGizmos()
    {
        if (!showTrajectory || throwPoint == null) return;

        // マウス位置取得
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // 投擲方向ベクトル
        Vector2 dir = (mouseWorld - throwPoint.position).normalized;

        // 下方向の場合は描画しない
        if (dir.y <= 0) return;

        // マウスが左右どちら側にあるかを判定
        bool isLeftSide = dir.x < 0;
        Vector2 baseDir = isLeftSide ? Vector2.left : Vector2.right;

        // 角度計算とクランプ
        float angle = Vector2.Angle(baseDir, dir);
        angle = Mathf.Clamp(angle, minAngle, maxAngle);

        // クランプされた方向ベクトル
        if (isLeftSide)
        {
            dir = Quaternion.Euler(0, 0, 180 - angle) * Vector2.right;
        }
        else
        {
            dir = Quaternion.Euler(0, 0, angle) * Vector2.right;
        }

        // 軌道を描画
        Gizmos.color = trajectoryColor;
        Vector2 velocity = dir * throwPower;
        Vector2 position = throwPoint.position;
        float timeStep = 0.1f;

        for (int i = 0; i < trajectoryPoints; i++)
        {
            Vector2 nextPosition = position + velocity * timeStep;
            velocity += Physics2D.gravity * timeStep;

            Gizmos.DrawLine(position, nextPosition);
            position = nextPosition;
        }
    }
}