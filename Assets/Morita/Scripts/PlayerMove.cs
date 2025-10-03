using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("�ړ��p�����[�^�[")]
    [SerializeField] private float     moveSpeed =  5f; //�ړ����x
    [SerializeField] private float     jumpForce = 10f; //�W�����v��
    [SerializeField] private LayerMask groundLayer;     //�ڒn����p

    private Rigidbody2D rb;
    private bool isGrounded;

    [Header("�ڒn�`�F�b�N")]
    [SerializeField] private Transform groundCheck; �@�@�@�@     //�ڒn�`�F�b�N�p�����蔻��̈ʒu
    [SerializeField] private float     groundCheckRadius = 0.2f; //�ڒn�`�F�b�N�p�����蔻��̔��a

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // ===== ���E�ړ� =====
        float moveInput = 0f;
        if      (Input.GetKey(KeyCode.A)) moveInput = -1f; //���ړ�
        else if (Input.GetKey(KeyCode.D)) moveInput =  1f; //�E�ړ�

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // ===== �ڒn���� =====
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // ===== �W�����v =====
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
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

