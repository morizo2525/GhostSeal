using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("移動パラメーター")]
    [SerializeField] private float moveSpeed = 5f; //移動速度
    [SerializeField] private float jumpForce = 10f; //ジャンプ力
    [SerializeField] private LayerMask groundLayer;     //接地判定用
    [SerializeField] private bool enableDoubleJump = true; //二段ジャンプ有効化
    [SerializeField] private float secondJumpForce = 9f;    //二段ジャンプの力(1段目より弱めに設定可能)

    [Header("回避設定")]
    [SerializeField] private float dodgeDistance = 3f;    //回避距離
    [SerializeField] private float dodgeSpeed = 15f;      //回避速度
    [SerializeField] private float dodgeCooldown = 1f;    //回避のクールダウン

    private float dodgeDuration;  //回避時間(距離と速度から自動計算)

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isGrounded;
    private bool hasDoubleJump;       //二段ジャンプが可能かどうか
    private bool isDodging;           //回避中フラグ
    private float dodgeTimer;         //回避の経過時間
    private float dodgeCooldownTimer; //クールダウンタイマー
    private int dodgeDirection;       //回避の方向(1=右, -1=左)

    [Header("接地チェック")]
    [SerializeField] private Transform groundCheck;              //接地チェック用当たり判定の位置
    [SerializeField] private float groundCheckRadius = 0.2f; //接地チェック用当たり判定の半径

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //接地判定
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        //地面に触れたら二段ジャンプをリセット
        if (isGrounded)
        {
            hasDoubleJump = true;
        }

        //クールダウンタイマー更新
        if (dodgeCooldownTimer > 0f)
        {
            dodgeCooldownTimer -= Time.deltaTime;
        }

        //回避中の処理
        if (isDodging)
        {
            dodgeTimer += Time.deltaTime;
            if (dodgeTimer >= dodgeDuration)
            {
                isDodging = false;
                dodgeTimer = 0f;
            }
            return; //回避中は通常の操作を受け付けない
        }

        //回避入力(Eキー)
        if (Input.GetKeyDown(KeyCode.E) && dodgeCooldownTimer <= 0f)
        {
            //回避時間を距離と速度から計算
            dodgeDuration = dodgeDistance / dodgeSpeed;

            //現在向いている方向に回避
            dodgeDirection = transform.localScale.x < 0 ? 1 : -1; //スケールが負なら右、正なら左
            isDodging = true;
            dodgeTimer = 0f;
            dodgeCooldownTimer = dodgeCooldown;
            return;
        }

        //左右移動
        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput -= 1f;  //左移動
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x); //左向きに反転
            transform.localScale = scale;
        }
        if (Input.GetKey(KeyCode.D))
        {
            horizontalInput += 1f; //右移動
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x); //右向きに反転
            transform.localScale = scale;
        }
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        //ジャンプ
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isGrounded)
            {
                //1段目のジャンプ
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else if (enableDoubleJump && hasDoubleJump)
            {
                //2段目のジャンプ
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, secondJumpForce);
                hasDoubleJump = false; //二段ジャンプを使用済みに
            }
        }
    }

    private void FixedUpdate()
    {
        //回避中の移動処理
        if (isDodging)
        {
            rb.linearVelocity = new Vector2(dodgeDirection * dodgeSpeed, rb.linearVelocity.y);
        }
    }

    //接地チェック用の当たり判定を表示(デバッグ用)
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}

