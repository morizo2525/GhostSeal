using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("�ړ��p�����[�^�[")]
    [SerializeField] private float     moveSpeed =  5f; //�ړ����x
    [SerializeField] private float     jumpForce = 10f; //�W�����v��
    [SerializeField] private LayerMask groundLayer;     //�ڒn����p

    private Rigidbody2D    rb;
    private SpriteRenderer sr;
    private bool isGrounded;
    
    [Header("�ڒn�`�F�b�N")]
    [SerializeField] private Transform groundCheck; �@�@�@�@     //�ڒn�`�F�b�N�p�����蔻��̈ʒu
    [SerializeField] private float     groundCheckRadius = 0.2f; //�ڒn�`�F�b�N�p�����蔻��̔��a

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //�ڒn����
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        //���E�ړ�
        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput -= 1f;  //���ړ�
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x); //�������ɔ��]
            transform.localScale = scale;
        }
        if (Input.GetKey(KeyCode.D))
        {
            horizontalInput += 1f; //�E�ړ�
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x); //�E�����ɔ��]
            transform.localScale = scale;
        }

        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        //�W�����v
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    //�ڒn�`�F�b�N�p�̓����蔻���\��(�f�o�b�O�p)
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}

