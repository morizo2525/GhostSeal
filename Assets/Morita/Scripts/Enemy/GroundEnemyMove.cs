using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class GroundEnemyMove : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    [SerializeField] private float horizontalSpeed = 3f;  // �W�����v���̉��ړ����x

    [Header("�ʏ�W�����v�ݒ�(�X���C���ړ�)")]
    [SerializeField] private float normalJumpForce = 5f;          // �ʏ�W�����v��
    [SerializeField] private float normalJumpInterval = 0.5f;     // �ʏ�W�����v�̊Ԋu
    [SerializeField] private float normalStopDuration = 0.1f;     // �ʏ�W�����v��̒�~����
    [SerializeField][Range(0f, 1f)] private float normalJumpChance = 0.7f; // �ʏ�W�����v�̊m��(0~1)

    [Header("��W�����v�ݒ�")]
    [SerializeField] private float bigJumpForce = 10f;            // ��W�����v��
    [SerializeField] private float bigJumpCooldown = 2f;          // �W�����v����̊Ԋu
    [SerializeField] private float bigStopDuration = 2f;          // ��W�����v��̒�~����
    [SerializeField][Range(0f, 1f)] private float bigJumpChance = 0.3f; // ��W�����v�̊m��(0~1)
                                                                        // ����: normalJumpChance + bigJumpChance �̍��v��1.0(100%)�ɂȂ�悤�ɒ������Ă�������

    [Header("�n�ʔ���")]
    [SerializeField] private Transform groundCheck;          // �ڒn�`�F�b�N�p�����蔻��̈ʒu
    [SerializeField] private float groundCheckRadius = 0.2f; // �ڒn�`�F�b�N�p�����蔻��̔��a
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Transform player;
    private float jumpIntervalTimer;  // �W�����v����̊Ԋu�^�C�}�[
    private float stopTimer;
    private bool isStopped;
    private bool wasGroundedLastFrame;
    private bool jumpCheckDone;       // �W�����v����ς݃t���O
    private bool isNormalJumpActive;  // �ʏ�W�����v���ǂ����̃t���O

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
        jumpIntervalTimer += Time.deltaTime;

        bool currentGrounded = IsGrounded();

        // ���n�����u�Ԃ����m
        if (currentGrounded && !wasGroundedLastFrame)
        {
            // ���n��̒�~�J�n(���ړ�����~)
            isStopped = true;
            stopTimer = 0f;
            rb.linearVelocity = Vector2.zero; // ���S�ɒ�~
        }

        // ��~���̏���
        if (isStopped)
        {
            stopTimer += Time.deltaTime;

            // ��~���Ԃ𔻒�(��W�����v�̕���������~����)
            float currentStopDuration = isNormalJumpActive ? normalStopDuration : bigStopDuration;

            if (stopTimer >= currentStopDuration)
            {
                isStopped = false;
                stopTimer = 0f;
            }
        }

        // �W�����v����̃^�C�~���O(�ʏ�W�����v�̊Ԋu�Ŕ���)
        if (!isStopped && currentGrounded && jumpIntervalTimer >= normalJumpInterval && !jumpCheckDone)
        {
            // �������擾
            float randomValue = Random.value;

            // �m���Ɋ�Â��ăW�����v�̎�ނ�����
            if (randomValue < bigJumpChance)
            {
                // ��W�����v
                BigJump();
            }
            else if (randomValue < bigJumpChance + normalJumpChance)
            {
                // �ʏ�W�����v
                NormalJump();
            }
            // ����ȊO(�c��̊m��)�̓W�����v���Ȃ�

            jumpCheckDone = true; // ����ς݂ɂ���
            jumpIntervalTimer = 0f; // �^�C�}�[���Z�b�g
        }

        // �^�C�}�[�����Z�b�g���ꂽ�画��t���O�����Z�b�g
        if (jumpIntervalTimer < normalJumpInterval)
        {
            jumpCheckDone = false;
        }

        wasGroundedLastFrame = currentGrounded;
    }

    void NormalJump()
    {
        if (player == null) return;

        // �v���C���[�ւ̕������v�Z
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        // �ʏ�̏��W�����v + �������̗�
        rb.linearVelocity = new Vector2(direction * horizontalSpeed, normalJumpForce);
        isNormalJumpActive = true;
    }

    void BigJump()
    {
        if (player == null) return;

        // �v���C���[�ւ̕������v�Z
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        // ��W�����v + �������̗�
        rb.linearVelocity = new Vector2(direction * horizontalSpeed, bigJumpForce);
        isNormalJumpActive = false;
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
