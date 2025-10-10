using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("移動パラメーター")]
    [SerializeField] private float     moveSpeed =  5f; //移動速度
    [SerializeField] private float     jumpForce = 10f; //ジャンプ力
    [SerializeField] private LayerMask groundLayer;     //接地判定用

    private Rigidbody2D    rb;
    private SpriteRenderer sr;
    private bool isGrounded;
    
    [Header("接地チェック")]
    [SerializeField] private Transform groundCheck; 　　　　     //接地チェック用当たり判定の位置
    [SerializeField] private float     groundCheckRadius = 0.2f; //接地チェック用当たり判定の半径

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //接地判定
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

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
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
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

