using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("�ړ��p�����[�^�[")]
    [SerializeField] private float moveSpeed = 5f; //�ړ����x
    [SerializeField] private float jumpForce = 10f; //�W�����v��
    [SerializeField] private LayerMask groundLayer;     //�ڒn����p
    [SerializeField] private bool enableDoubleJump = true; //��i�W�����v�L����
    [SerializeField] private float secondJumpForce = 9f;    //��i�W�����v�̗�(1�i�ڂ���߂ɐݒ�\)

    [Header("���ݒ�")]
    [SerializeField] private float dodgeDistance = 3f;    //�������
    [SerializeField] private float dodgeSpeed = 15f;      //��𑬓x
    [SerializeField] private float dodgeCooldown = 1f;    //����̃N�[���_�E��

    private float dodgeDuration;  //�������(�����Ƒ��x���玩���v�Z)

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isGrounded;
    private bool hasDoubleJump;       //��i�W�����v���\���ǂ���
    private bool isDodging;           //��𒆃t���O
    private float dodgeTimer;         //����̌o�ߎ���
    private float dodgeCooldownTimer; //�N�[���_�E���^�C�}�[
    private int dodgeDirection;       //����̕���(1=�E, -1=��)

    [Header("�ڒn�`�F�b�N")]
    [SerializeField] private Transform groundCheck;              //�ڒn�`�F�b�N�p�����蔻��̈ʒu
    [SerializeField] private float groundCheckRadius = 0.2f; //�ڒn�`�F�b�N�p�����蔻��̔��a

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //�ڒn����
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        //�n�ʂɐG�ꂽ���i�W�����v�����Z�b�g
        if (isGrounded)
        {
            hasDoubleJump = true;
        }

        //�N�[���_�E���^�C�}�[�X�V
        if (dodgeCooldownTimer > 0f)
        {
            dodgeCooldownTimer -= Time.deltaTime;
        }

        //��𒆂̏���
        if (isDodging)
        {
            dodgeTimer += Time.deltaTime;
            if (dodgeTimer >= dodgeDuration)
            {
                isDodging = false;
                dodgeTimer = 0f;
            }
            return; //��𒆂͒ʏ�̑�����󂯕t���Ȃ�
        }

        //������(E�L�[)
        if (Input.GetKeyDown(KeyCode.E) && dodgeCooldownTimer <= 0f)
        {
            //������Ԃ������Ƒ��x����v�Z
            dodgeDuration = dodgeDistance / dodgeSpeed;

            //���݌����Ă�������ɉ��
            dodgeDirection = transform.localScale.x < 0 ? 1 : -1; //�X�P�[�������Ȃ�E�A���Ȃ獶
            isDodging = true;
            dodgeTimer = 0f;
            dodgeCooldownTimer = dodgeCooldown;
            return;
        }

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
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isGrounded)
            {
                //1�i�ڂ̃W�����v
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else if (enableDoubleJump && hasDoubleJump)
            {
                //2�i�ڂ̃W�����v
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, secondJumpForce);
                hasDoubleJump = false; //��i�W�����v���g�p�ς݂�
            }
        }
    }

    private void FixedUpdate()
    {
        //��𒆂̈ړ�����
        if (isDodging)
        {
            rb.linearVelocity = new Vector2(dodgeDirection * dodgeSpeed, rb.linearVelocity.y);
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

