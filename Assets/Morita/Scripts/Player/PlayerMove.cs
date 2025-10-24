using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("�ړ��p�����[�^�[")]
    [SerializeField] private float     moveSpeed = 5f;           // �ړ����x
    [SerializeField] private float     jumpForce = 10f;          // 1�i�ڂ̃W�����v��
    [SerializeField] private LayerMask groundLayer;              // �n�ʂƂ��ĔF�����郌�C���[
    [SerializeField] private bool      enableDoubleJump = true;  // 2�i�W�����v��L���ɂ��邩
    [SerializeField] private float     secondJumpForce = 9f;     // 2�i�ڂ̃W�����v��

    [Header("���ݒ�")]
    [SerializeField] private float dodgeDistance = 3f; �@�@�@�@// �������
    [SerializeField] private float dodgeSpeed = 15f; �@�@�@�@�@// ��𑬓x
    [SerializeField] private float dodgeCooldown = 1f; �@�@�@�@// ����N�[���_�E��
    [SerializeField] private float dodgeInvincibleTime = 0.5f; // ����̖��G����

    private float dodgeDuration; // ��𓮍�S�̂̎���

    private Rigidbody2D rb; 
    private AnimationController animController;

    private bool  isGrounded;         // �ڒn���Ă��邩
    private bool  wasGrounded;        // �O�t���[���Őڒn���Ă�����
    private bool  hasDoubleJump;      // 2�i�W�����v���g�p�\��
    private bool  isDodging;          // ���݉�𒆂�
    private float dodgeTimer;         // ����J�n����̌o�ߎ���
    private float dodgeCooldownTimer; // ����̃N�[���_�E���c�莞��
    private int   dodgeDirection;     // ����̕���(1:�E / -1:��)

    [Header("�ڒn�`�F�b�N")]
    [SerializeField] private Transform groundCheck;          // �ڒn����p�̃I�u�W�F�N�g�ʒu
    [SerializeField] private float groundCheckRadius = 0.2f; // �ڒn����̔��a

    [HideInInspector] public bool isInvincible = false; // ��𖳓G�t���O(���X�N���v�g����Q��)

    void Start()
    {
        rb             = GetComponent<Rigidbody2D>();
        animController = GetComponent<AnimationController>();
    }

    private void Update()
    {
        // �ڒn����
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && !wasGrounded)
        {
            animController.PlayerLandAnim();
        }

        if (isGrounded) hasDoubleJump = true; // ���n������2�i�W�����v����

        // �N�[���_�E���^�C�}�[�X�V
        if (dodgeCooldownTimer > 0f) dodgeCooldownTimer -= Time.deltaTime;

        // ��𒆂̏���
        if (isDodging)
        {
            dodgeTimer += Time.deltaTime;

            // ���G���ԏI���`�F�b�N
            if (dodgeTimer >= dodgeInvincibleTime)
                isInvincible = false; // ���G���Ԃ��I�������疳�G����

            // ��𓮍�I���`�F�b�N
            if (dodgeTimer >= dodgeDuration)
            {
                isDodging    = false;  // ����I��
                dodgeTimer   = 0f; �@�@// �^�C�}�[���Z�b�g
                isInvincible = false;�@// �m���ɖ��G����
            }
            return; // ��𒆂͒ʏ푀����󂯕t���Ȃ�
        }

        // ������
        if (Input.GetKeyDown(KeyCode.E) && dodgeCooldownTimer <= 0f)
        {
            dodgeDuration      = dodgeDistance / dodgeSpeed; �@�@�@�@ // ������Ԃ��v�Z
            dodgeDirection     = transform.localScale.x < 0 ? 1 : -1; // �����̋t�����ɉ��
            isDodging          = true; �@                             // ����J�n
            dodgeTimer         = 0f;                                  // �^�C�}�[���Z�b�g
            dodgeCooldownTimer = dodgeCooldown;                       // �N�[���_�E���J�n
            isInvincible       = true;                                // ���G��ԊJ�n
            return;
        }

        // ���E�ړ�
        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput -= 1f; // ���ړ�
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x); // �X�v���C�g����������
            transform.localScale = scale;
            animController.PlayerBesideMoveAnim();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput += 1f; // �E�ړ�
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x); // �X�v���C�g���E������
            transform.localScale = scale;
            animController.PlayerBesideMoveAnim();
        }
        else
        {
            animController.PlayerIdleAnim();
        }
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // �W�����v
        if (Input.GetKeyDown(KeyCode.W))
        {
            animController.PlayerJumpAnim();

            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // 1�i�ڃW�����v
                
            }
            else if (enableDoubleJump && hasDoubleJump)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, secondJumpForce); // 2�i�ڃW�����v
                hasDoubleJump = false; // 2�i�W�����v������
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
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius); // �ڒn����͈͂�����
        }
    }
}

