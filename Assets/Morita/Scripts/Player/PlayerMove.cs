using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("移動パラメーター")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool enableDoubleJump = true;
    [SerializeField] private float secondJumpForce = 9f;

    [Header("回避設定")]
    [SerializeField] private float dodgeDistance = 3f;
    [SerializeField] private float dodgeSpeed = 15f;
    [SerializeField] private float dodgeCooldown = 1f;
    [SerializeField] private float dodgeInvincibleTime = 0.5f; // 回避中無敵時間

    private float dodgeDuration;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isGrounded;
    private bool hasDoubleJump;
    private bool isDodging;
    private float dodgeTimer;
    private float dodgeCooldownTimer;
    private int dodgeDirection;

    [Header("接地チェック")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;

    [HideInInspector] public bool isInvincible = false; // 回避無敵フラグ

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // 接地判定
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded) hasDoubleJump = true;

        // クールダウンタイマー更新
        if (dodgeCooldownTimer > 0f) dodgeCooldownTimer -= Time.deltaTime;

        // 回避中の処理
        if (isDodging)
        {
            dodgeTimer += Time.deltaTime;

            // 無敵時間終了チェック
            if (dodgeTimer >= dodgeInvincibleTime)
                isInvincible = false;

            if (dodgeTimer >= dodgeDuration)
            {
                isDodging = false;
                dodgeTimer = 0f;
            }
            return; // 回避中は通常操作を受け付けない
        }

        // 回避入力(Eキー)
        if (Input.GetKeyDown(KeyCode.E) && dodgeCooldownTimer <= 0f)
        {
            dodgeDuration = dodgeDistance / dodgeSpeed;
            dodgeDirection = transform.localScale.x < 0 ? 1 : -1;
            isDodging = true;
            dodgeTimer = 0f;
            dodgeCooldownTimer = dodgeCooldown;
            isInvincible = true;
            return;
        }

        // 左右移動
        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput -= 1f;
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        if (Input.GetKey(KeyCode.D))
        {
            horizontalInput += 1f;
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // ジャンプ
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isGrounded)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            else if (enableDoubleJump && hasDoubleJump)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, secondJumpForce);
                hasDoubleJump = false;
            }
        }
    }

    private void FixedUpdate()
    {
        // 回避中の移動
        if (isDodging)
        {
            rb.linearVelocity = new Vector2(dodgeDirection * dodgeSpeed, rb.linearVelocity.y);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}

