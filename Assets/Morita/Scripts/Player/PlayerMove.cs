using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("�ړ��p�����[�^�[")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool enableDoubleJump = true;
    [SerializeField] private float secondJumpForce = 9f;

    [Header("���ݒ�")]
    [SerializeField] private float dodgeDistance = 3f;
    [SerializeField] private float dodgeSpeed = 15f;
    [SerializeField] private float dodgeCooldown = 1f;
    [SerializeField] private float dodgeInvincibleTime = 0.5f; // ��𒆖��G����

    private float dodgeDuration;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isGrounded;
    private bool hasDoubleJump;
    private bool isDodging;
    private float dodgeTimer;
    private float dodgeCooldownTimer;
    private int dodgeDirection;

    [Header("�ڒn�`�F�b�N")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;

    [HideInInspector] public bool isInvincible = false; // ��𖳓G�t���O

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // �ڒn����
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded) hasDoubleJump = true;

        // �N�[���_�E���^�C�}�[�X�V
        if (dodgeCooldownTimer > 0f) dodgeCooldownTimer -= Time.deltaTime;

        // ��𒆂̏���
        if (isDodging)
        {
            dodgeTimer += Time.deltaTime;

            // ���G���ԏI���`�F�b�N
            if (dodgeTimer >= dodgeInvincibleTime)
                isInvincible = false;

            if (dodgeTimer >= dodgeDuration)
            {
                isDodging = false;
                dodgeTimer = 0f;
            }
            return; // ��𒆂͒ʏ푀����󂯕t���Ȃ�
        }

        // ������(E�L�[)
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

        // ���E�ړ�
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

        // �W�����v
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
        // ��𒆂̈ړ�
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

