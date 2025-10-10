using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class GroundEnemyMove : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("�W�����v�ݒ�")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpCooldown = 2f;
    [SerializeField] private float stopDurationAfterJump = 2f;
    [SerializeField] private float randomJumpChance = 0.02f; // ���t���[���̃W�����v�m��

    [Header("�n�ʔ���")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.1f;

    private Rigidbody2D rb;
    private Transform player;
    private float jumpTimer;
    private float stopTimer;
    private bool isStopped;

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

        // ��~���̏���
        if (isStopped)
        {
            stopTimer += Time.deltaTime;
            if (stopTimer >= stopDurationAfterJump)
            {
                isStopped = false;
                stopTimer = 0f;
            }
            return;
        }

        // �����_���W�����v
        if (IsGrounded() && jumpTimer >= jumpCooldown)
        {
            if (Random.value < randomJumpChance)
            {
                Jump();
            }
        }
    }

    void FixedUpdate()
    {
        // ��~���͈ړ����Ȃ�
        if (isStopped || player == null)
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
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpTimer = 0f;
        isStopped = true;
        stopTimer = 0f;
    }

    bool IsGrounded()
    {
        Vector2 position = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, groundCheckDistance, groundLayer);
        return hit.collider != null;
    }

    void OnDrawGizmos()
    {
        // �n�ʔ���̉���
        Gizmos.color = Color.green;
        Vector2 position = transform.position;
        Gizmos.DrawLine(position, position + Vector2.down * groundCheckDistance);
    }
}
