using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class AirEnemyMove : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 4f;      // 移動速度
    [Header("揺れ設定")]
    [SerializeField] private float waveAmplitude = 1f;  //揺れ幅(上下の振れ幅)
    [SerializeField] private float waveFrequency = 2f;  //揺れスピード(周波数)

    private Rigidbody2D rb;
    private Transform player;
    private float waveTimer;         //揺れのタイマー
    private float initialScaleX;
    private int   currentFacing = 1; // 1:右向き, -1:左向き

    // ノックバック制御用
    private bool isKnockedBack = false;
    private float knockbackEndTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialScaleX = transform.localScale.x; // 初期スケール保存

        // 重力の影響を受けないように設定
        rb.gravityScale = 0f;
        // プレイヤーを検索
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Playerタグのオブジェクトが見つかりません");
        }
    }

    void FixedUpdate()
    {
        // ノックバック中かチェック
        if (isKnockedBack)
        {
            if (Time.time >= knockbackEndTime)
            {
                isKnockedBack = false;
            }
            else
            {
                // ノックバック中は移動処理をスキップ
                return;
            }
        }

        // プレイヤーが存在しない場合は移動しない
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // 揺れのタイマーを更新
        waveTimer += Time.fixedDeltaTime;
        // プレイヤーへの方向ベクトルを計算
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        // 進行方向に対して垂直なベクトルを計算(上下の揺れ用)
        Vector2 perpendicular = new Vector2(-directionToPlayer.y, directionToPlayer.x);
        // サイン波で滑らかな揺れを生成
        float waveOffset = Mathf.Sin(waveTimer * waveFrequency) * waveAmplitude;
        // 基本の移動方向 + 揺れ
        Vector2 finalDirection = directionToPlayer + perpendicular * waveOffset;
        // プレイヤーに向かって揺れながら移動
        rb.linearVelocity = finalDirection.normalized * moveSpeed;

        // 向きの調整
        float horizontalDiff = player.position.x - transform.position.x;
        if (Mathf.Abs(horizontalDiff) > 0.1f)
        {
            int desiredFacing = horizontalDiff > 0 ? 1 : -1;
            if (desiredFacing != currentFacing) // 向きが変わった場合のみ更新
            {
                currentFacing = desiredFacing;
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(initialScaleX) * -currentFacing * Mathf.Sign(initialScaleX);
                transform.localScale = scale;
            }
        }

    }

    // 外部から呼び出してノックバック状態にする
    public void ApplyKnockback(float duration)
    {
        isKnockedBack = true;
        knockbackEndTime = Time.time + duration;
    }
}