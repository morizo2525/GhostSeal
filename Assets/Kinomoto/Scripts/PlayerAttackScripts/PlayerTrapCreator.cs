using UnityEngine;

public class PlayerTrapCreator : MonoBehaviour
{
    [Header("トラバサミトラップ（罠単体攻撃）の設定")]
    [SerializeField] private GameObject trapPrefab;

    [Header("地雷トラップの設定")]
    [SerializeField] private GameObject mineTrapPrefab;

    [Header("投擲設定")]
    [SerializeField] private float maxThrowDistance = 3f; // 最大投擲距離
    [SerializeField] private float downwardThreshold = 1f; // この距離以下で足元配置（Y軸）
    [SerializeField] private Camera mainCamera; // メインカメラ参照

    [Header("デバッグ")]
    [SerializeField] private bool showThrowPreview = true; // 投擲予測の表示

    private void Start()
    {
        // カメラが設定されていない場合、自動取得
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    /// <summary>
    /// プレイヤーの足元、またはマウス方向にトラバサミトラップを生成
    /// </summary>
    public void CreateTrap()
    {
        ThrowTrap(trapPrefab);
    }

    /// <summary>
    /// プレイヤーの足元、またはマウス方向に地雷トラップを生成
    /// </summary>
    public void CreateMineTrap()
    {
        ThrowTrap(mineTrapPrefab);
    }

    /// <summary>
    /// トラップを投擲する
    /// </summary>
    private void ThrowTrap(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("トラッププレハブが設定されていません");
            return;
        }

        // トラップを生成（プレイヤーの位置から）
        GameObject trap = Instantiate(prefab, transform.position, Quaternion.identity);

        // 目標位置を計算
        Vector3 targetPosition = CalculateTrapPosition();

        // 足元配置の場合は即座に設置
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            trap.transform.position = targetPosition;
            Debug.Log($"トラップを足元に配置しました: {targetPosition}");
            return;
        }

        // Rigidbody2Dを取得または追加
        Rigidbody2D rb = trap.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = trap.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1f;
        }

        // 投擲方向と力を計算
        Vector2 direction = (targetPosition - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, targetPosition);

        // 投擲力を計算（距離に応じて調整）
        float throwForce = Mathf.Lerp(3f, 8f, distance / maxThrowDistance);
        Vector2 throwVelocity = direction * throwForce;

        // 上方向の力を追加（放物線を描くため）
        throwVelocity.y += 3f;

        // 速度を設定
        rb.linearVelocity = throwVelocity;

        // 回転を追加（投げられている感）
        rb.angularVelocity = -200f;

        Debug.Log($"トラップを投擲しました: 目標 {targetPosition}, 力 {throwVelocity}");
    }

    /// <summary>
    /// マウス位置に基づいてトラップの配置位置を計算
    /// </summary>
    private Vector3 CalculateTrapPosition()
    {
        if (mainCamera == null)
        {
            Debug.LogWarning("カメラが設定されていません。足元に配置します。");
            return transform.position;
        }

        // マウスのワールド座標を取得
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; // 2D用にZ座標を0に

        // プレイヤーからマウスへの方向ベクトル
        Vector3 directionToMouse = mouseWorldPos - transform.position;

        // マウスがプレイヤーの下方にある場合は足元に配置
        if (Mathf.Abs(directionToMouse.y) < downwardThreshold && directionToMouse.y < 0)
        {
            return transform.position;
        }

        // 投擲距離を計算（最大距離でクランプ）
        float distance = Mathf.Min(directionToMouse.magnitude, maxThrowDistance);

        // 方向を正規化
        Vector3 direction = directionToMouse.normalized;

        // 投擲先の位置を計算
        Vector3 targetPosition = transform.position + direction * distance;

        return targetPosition;
    }

    /// <summary>
    /// デバッグ用：投擲予測を表示
    /// </summary>
    private void OnDrawGizmos()
    {
        if (!showThrowPreview || mainCamera == null) return;

        // マウスのワールド座標を取得
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // プレイヤーからマウスへの方向
        Vector3 directionToMouse = mouseWorldPos - transform.position;

        // 足元配置の判定
        if (Mathf.Abs(directionToMouse.y) < downwardThreshold && directionToMouse.y < 0)
        {
            // 足元配置の場合は青い円
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 0.3f);
        }
        else
        {
            // 投擲の場合
            float distance = Mathf.Min(directionToMouse.magnitude, maxThrowDistance);
            Vector3 direction = directionToMouse.normalized;
            Vector3 targetPosition = transform.position + direction * distance;

            // 投擲先を黄色の円で表示
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(targetPosition, 0.3f);

            // プレイヤーから投擲先への線
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, targetPosition);
        }

        // 最大投擲範囲を表示
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, maxThrowDistance);
    }
}