using UnityEngine;

public class PlayerBombAttack : MonoBehaviour
{
    [Header("爆弾の設定")]
    public GameObject bombPrefab;           // 投擲する爆弾のPrefab
    public Transform throwPoint;            // 投擲位置
    public float throwPower = 8f;           // 投擲の初速

    [Header("デバッグ設定")]
    public bool showTrajectory = true;      // 軌道予測を表示
    public Color trajectoryColor = Color.yellow;
    public int trajectoryPoints = 30;       // 軌道の点の数

    /// <summary>
    /// 爆弾を投擲する（角度制限なし）
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

        // マウス位置取得
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // 投擲方向
        Vector2 dir = (mouseWorld - throwPoint.position).normalized;

        // 爆弾を生成
        GameObject bomb = Instantiate(bombPrefab, throwPoint.position, Quaternion.identity);

        // 爆弾の向きを設定（任意）
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

        Debug.Log("爆弾を投擲しました");
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