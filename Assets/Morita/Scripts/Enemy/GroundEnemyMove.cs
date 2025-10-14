using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class GroundEnemyMove : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 3f;  // 移動速度

    [Header("ジャンプ設定")]
    [SerializeField] private float jumpForce = 10f;            // ジャンプ力
    [SerializeField] private float jumpCooldown = 2f;          // ジャンプのクールダウン時間
    [SerializeField] private float stopDurationAfterJump = 2f; // ジャンプ後の停止時間
    [SerializeField] private float randomJumpChance = 0.3f;    // ジャンプ確率(0~1)

    [Header("地面判定")]
    [SerializeField] private Transform groundCheck;          // 接地チェック用当たり判定の位置
    [SerializeField] private float groundCheckRadius = 0.2f; // 接地チェック用当たり判定の半径
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Transform player;
    private float jumpTimer;
    private float stopTimer;
    private bool isStopped;
    private bool wasGroundedLastFrame;
    private bool jumpCheckDone; // クールダウン後のジャンプ判定済みフラグ

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
        jumpTimer += Time.deltaTime;

        bool currentGrounded = IsGrounded();

        // 着地した瞬間を検知
        if (currentGrounded && !wasGroundedLastFrame)
        {
            // 着地後の停止開始
            isStopped = true;
            stopTimer = 0f;
        }

        // 停止中の処理
        if (isStopped)
        {
            stopTimer += Time.deltaTime;
            if (stopTimer >= stopDurationAfterJump)
            {
                isStopped = false;
                stopTimer = 0f;
            }
        }

        // クールダウンが終了したら、次回の接地時に1回だけジャンプ判定
        if (!isStopped && currentGrounded && jumpTimer >= jumpCooldown)
        {
            if (!jumpCheckDone)
            {
                // ジャンプするかどうかを1回だけ判定
                if (Random.value < randomJumpChance)
                {
                    Jump();
                }
                jumpCheckDone = true; // 判定済みにする
            }
        }

        // クールダウン中は判定フラグをリセット
        if (jumpTimer < jumpCooldown)
        {
            jumpCheckDone = false;
        }

        wasGroundedLastFrame = currentGrounded;
    }

    void FixedUpdate()
    {
        // プレイヤーが存在しない場合は移動しない
        if (player == null)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        // 停止中は横移動しない
        if (isStopped)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        // プレイヤーへの追従
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        // 横方向の速度を維持したままジャンプ
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpTimer = 0f;
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
