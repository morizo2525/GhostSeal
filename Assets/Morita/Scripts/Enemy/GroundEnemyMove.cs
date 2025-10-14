using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class GroundEnemyMove : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    [SerializeField] private float moveSpeed = 3f;  // �ړ����x

    [Header("�W�����v�ݒ�")]
    [SerializeField] private float jumpForce = 10f;            // �W�����v��
    [SerializeField] private float jumpCooldown = 2f;          // �W�����v�̃N�[���_�E������
    [SerializeField] private float stopDurationAfterJump = 2f; // �W�����v��̒�~����
    [SerializeField] private float randomJumpChance = 0.3f;    // �W�����v�m��(0~1)

    [Header("�n�ʔ���")]
    [SerializeField] private Transform groundCheck;          // �ڒn�`�F�b�N�p�����蔻��̈ʒu
    [SerializeField] private float groundCheckRadius = 0.2f; // �ڒn�`�F�b�N�p�����蔻��̔��a
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Transform player;
    private float jumpTimer;
    private float stopTimer;
    private bool isStopped;
    private bool wasGroundedLastFrame;
    private bool jumpCheckDone; // �N�[���_�E����̃W�����v����ς݃t���O

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // �v���C���[������
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player�^�O�̃I�u�W�F�N�g��������܂���");
        }
    }

    void Update()
    {
        jumpTimer += Time.deltaTime;

        bool currentGrounded = IsGrounded();

        // ���n�����u�Ԃ����m
        if (currentGrounded && !wasGroundedLastFrame)
        {
            // ���n��̒�~�J�n
            isStopped = true;
            stopTimer = 0f;
        }

        // ��~���̏���
        if (isStopped)
        {
            stopTimer += Time.deltaTime;
            if (stopTimer >= stopDurationAfterJump)
            {
                isStopped = false;
                stopTimer = 0f;
            }
        }

        // �N�[���_�E�����I��������A����̐ڒn����1�񂾂��W�����v����
        if (!isStopped && currentGrounded && jumpTimer >= jumpCooldown)
        {
            if (!jumpCheckDone)
            {
                // �W�����v���邩�ǂ�����1�񂾂�����
                if (Random.value < randomJumpChance)
                {
                    Jump();
                }
                jumpCheckDone = true; // ����ς݂ɂ���
            }
        }

        // �N�[���_�E�����͔���t���O�����Z�b�g
        if (jumpTimer < jumpCooldown)
        {
            jumpCheckDone = false;
        }

        wasGroundedLastFrame = currentGrounded;
    }

    void FixedUpdate()
    {
        // �v���C���[�����݂��Ȃ��ꍇ�͈ړ����Ȃ�
        if (player == null)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        // ��~���͉��ړ����Ȃ�
        if (isStopped)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        // �v���C���[�ւ̒Ǐ]
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        // �������̑��x���ێ������܂܃W�����v
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpTimer = 0f;
    }

    bool IsGrounded()
    {
        if (groundCheck == null)
        {
            Debug.LogWarning("groundCheck���ݒ肳��Ă��܂���");
            return false;
        }

        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        // �n�ʔ���̉���
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
