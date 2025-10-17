using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class GroundEnemyMove : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float horizontalSpeed = 3f;  // ジャンプ時の横移動速度

    [Header("通常ジャンプ設定(スライム移動)")]
    [SerializeField] private float normalJumpForce = 5f;          // 通常ジャンプ力
    [SerializeField] private float normalJumpInterval = 0.5f;     // 通常ジャンプの間隔
    [SerializeField] private float normalStopDuration = 0.1f;     // 通常ジャンプ後の停止時間
    [SerializeField][Range(0f, 1f)] private float normalJumpChance = 0.7f; // 通常ジャンプの確率(0~1)

    [Header("大ジャンプ設定")]
    [SerializeField] private float bigJumpForce = 10f;            // 大ジャンプ力
    [SerializeField] private float bigJumpCooldown = 2f;          // ジャンプ判定の間隔
    [SerializeField] private float bigStopDuration = 2f;          // 大ジャンプ後の停止時間
    [SerializeField][Range(0f, 1f)] private float bigJumpChance = 0.3f; // 大ジャンプの確率(0~1)
                                                                        // 注意: normalJumpChance + bigJumpChance の合計が1.0(100%)になるように調整してください

    [Header("地面判定")]
    [SerializeField] private Transform groundCheck;          // 接地チェック用当たり判定の位置
    [SerializeField] private float groundCheckRadius = 0.2f; // 接地チェック用当たり判定の半径
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Transform player;
    private float jumpIntervalTimer;  // ジャンプ判定の間隔タイマー
    private float stopTimer;
    private bool isStopped;
    private bool wasGroundedLastFrame;
    private bool jumpCheckDone;       // ジャンプ判定済みフラグ
    private bool isNormalJumpActive;  // 通常ジャンプかどうかのフラグ

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

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

    void Update()
    {
        jumpIntervalTimer += Time.deltaTime;

        bool currentGrounded = IsGrounded();

        // 着地した瞬間を検知
        if (currentGrounded && !wasGroundedLastFrame)
        {
            // 着地後の停止開始(横移動も停止)
            isStopped = true;
            stopTimer = 0f;
            rb.linearVelocity = Vector2.zero; // 完全に停止
        }

        // 停止中の処理
        if (isStopped)
        {
            stopTimer += Time.deltaTime;

            // 停止時間を判定(大ジャンプの方が長い停止時間)
            float currentStopDuration = isNormalJumpActive ? normalStopDuration : bigStopDuration;

            if (stopTimer >= currentStopDuration)
            {
                isStopped = false;
                stopTimer = 0f;
            }
        }

        // ジャンプ判定のタイミング(通常ジャンプの間隔で判定)
        if (!isStopped && currentGrounded && jumpIntervalTimer >= normalJumpInterval && !jumpCheckDone)
        {
            // 乱数を取得
            float randomValue = Random.value;

            // 確率に基づいてジャンプの種類を決定
            if (randomValue < bigJumpChance)
            {
                // 大ジャンプ
                BigJump();
            }
            else if (randomValue < bigJumpChance + normalJumpChance)
            {
                // 通常ジャンプ
                NormalJump();
            }
            // それ以外(残りの確率)はジャンプしない

            jumpCheckDone = true; // 判定済みにする
            jumpIntervalTimer = 0f; // タイマーリセット
        }

        // タイマーがリセットされたら判定フラグもリセット
        if (jumpIntervalTimer < normalJumpInterval)
        {
            jumpCheckDone = false;
        }

        wasGroundedLastFrame = currentGrounded;
    }

    void NormalJump()
    {
        if (player == null) return;

        // プレイヤーへの方向を計算
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        // 通常の小ジャンプ + 横方向の力
        rb.linearVelocity = new Vector2(direction * horizontalSpeed, normalJumpForce);
        isNormalJumpActive = true;
    }

    void BigJump()
    {
        if (player == null) return;

        // プレイヤーへの方向を計算
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        // 大ジャンプ + 横方向の力
        rb.linearVelocity = new Vector2(direction * horizontalSpeed, bigJumpForce);
        isNormalJumpActive = false;
    }

    bool IsGrounded()
    {
        if (groundCheck == null)
        {
            Debug.LogWarning("groundCheckが設定されていません");
            return false;
        }

        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        // 地面判定の可視化
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
