using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("移動パラメーター")]
    [SerializeField] private float     moveSpeed = 5f;           // 移動速度
    [SerializeField] private float     jumpForce = 10f;          // 1段目のジャンプ力
    [SerializeField] private LayerMask groundLayer;              // 地面として認識するレイヤー
    [SerializeField] private bool      enableDoubleJump = true;  // 2段ジャンプを有効にするか
    [SerializeField] private float     secondJumpForce = 9f;     // 2段目のジャンプ力

    [Header("回避設定")]
    [SerializeField] private float dodgeDistance = 3f; 　　　　// 回避距離
    [SerializeField] private float dodgeSpeed = 15f; 　　　　　// 回避速度
    [SerializeField] private float dodgeCooldown = 1f; 　　　　// 回避クールダウン
    [SerializeField] private float dodgeInvincibleTime = 0.5f; // 回避の無敵時間

    private float dodgeDuration; // 回避動作全体の時間

    private Rigidbody2D rb; 
    private AnimationController animController;

    private bool  isGrounded;         // 接地しているか
    private bool  wasGrounded;        // 前フレームで接地していたか
    private bool  hasDoubleJump;      // 2段ジャンプが使用可能か
    private bool  isDodging;          // 現在回避中か
    private float dodgeTimer;         // 回避開始からの経過時間
    private float dodgeCooldownTimer; // 回避のクールダウン残り時間
    private int   dodgeDirection;     // 回避の方向(1:右 / -1:左)

    [Header("接地チェック")]
    [SerializeField] private Transform groundCheck;          // 接地判定用のオブジェクト位置
    [SerializeField] private float groundCheckRadius = 0.2f; // 接地判定の半径

    [HideInInspector] public bool isInvincible = false; // 回避無敵フラグ(他スクリプトから参照)

    void Start()
    {
        rb             = GetComponent<Rigidbody2D>();
        animController = GetComponent<AnimationController>();
    }

    private void Update()
    {
        // 接地判定
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && !wasGrounded)
        {
            animController.PlayerLandAnim();
        }

        if (isGrounded) hasDoubleJump = true; // 着地したら2段ジャンプを回復

        // クールダウンタイマー更新
        if (dodgeCooldownTimer > 0f) dodgeCooldownTimer -= Time.deltaTime;

        // 回避中の処理
        if (isDodging)
        {
            dodgeTimer += Time.deltaTime;

            // 無敵時間終了チェック
            if (dodgeTimer >= dodgeInvincibleTime)
                isInvincible = false; // 無敵時間が終了したら無敵解除

            // 回避動作終了チェック
            if (dodgeTimer >= dodgeDuration)
            {
                isDodging    = false;  // 回避終了
                dodgeTimer   = 0f; 　　// タイマーリセット
                isInvincible = false;　// 確実に無敵解除
            }
            return; // 回避中は通常操作を受け付けない
        }

        // 回避入力
        if (Input.GetKeyDown(KeyCode.E) && dodgeCooldownTimer <= 0f)
        {
            dodgeDuration      = dodgeDistance / dodgeSpeed; 　　　　 // 回避時間を計算
            dodgeDirection     = transform.localScale.x < 0 ? 1 : -1; // 向きの逆方向に回避
            isDodging          = true; 　                             // 回避開始
            dodgeTimer         = 0f;                                  // タイマーリセット
            dodgeCooldownTimer = dodgeCooldown;                       // クールダウン開始
            isInvincible       = true;                                // 無敵状態開始
            return;
        }

        // 左右移動
        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput -= 1f; // 左移動
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x); // スプライトを左向きに
            transform.localScale = scale;
            animController.PlayerBesideMoveAnim();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput += 1f; // 右移動
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x); // スプライトを右向きに
            transform.localScale = scale;
            animController.PlayerBesideMoveAnim();
        }
        else
        {
            animController.PlayerIdleAnim();
        }
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // ジャンプ
        if (Input.GetKeyDown(KeyCode.W))
        {
            animController.PlayerJumpAnim();

            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // 1段目ジャンプ
                
            }
            else if (enableDoubleJump && hasDoubleJump)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, secondJumpForce); // 2段目ジャンプ
                hasDoubleJump = false; // 2段ジャンプを消費
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
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius); // 接地判定範囲を可視化
        }
    }
}

