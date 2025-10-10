using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class GroundEnemyMove : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("ジャンプ設定")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpCooldown = 2f;
    [SerializeField] private float stopDurationAfterJump = 2f;
    [SerializeField] private float randomJumpChance = 0.02f; // 毎フレームのジャンプ確率

    [Header("地面判定")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.1f;

    private Rigidbody2D rb;
    private Transform player;
    private float jumpTimer;
    private float stopTimer;
    private bool isStopped;

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

        // 停止中の処理
        if (isStopped)
        {
            stopTimer += Time.deltaTime;
            if (stopTimer >= stopDurationAfterJump)
            {
                isStopped = false;
                stopTimer = 0f;
            }
            return;
        }

        // ランダムジャンプ
        if (IsGrounded() && jumpTimer >= jumpCooldown)
        {
            if (Random.value < randomJumpChance)
            {
                Jump();
            }
        }
    }

    void FixedUpdate()
    {
        // 停止中は移動しない
        if (isStopped || player == null)
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
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpTimer = 0f;
        isStopped = true;
        stopTimer = 0f;
    }

    bool IsGrounded()
    {
        Vector2 position = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, groundCheckDistance, groundLayer);
        return hit.collider != null;
    }

    void OnDrawGizmos()
    {
        // 地面判定の可視化
        Gizmos.color = Color.green;
        Vector2 position = transform.position;
        Gizmos.DrawLine(position, position + Vector2.down * groundCheckDistance);
    }
}
