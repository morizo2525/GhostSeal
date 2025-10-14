using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class AirEnemyMove : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    [SerializeField] private float moveSpeed = 4f;

    private Rigidbody2D rb;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // �d�͂̉e�����󂯂Ȃ��悤�ɐݒ�
        rb.gravityScale = 0f;

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

    void FixedUpdate()
    {
        // �v���C���[�����݂��Ȃ��ꍇ�͈ړ����Ȃ�
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // �v���C���[�ւ̕����x�N�g�����v�Z
        Vector2 direction = (player.position - transform.position).normalized;

        // �v���C���[�Ɍ������Ĉړ�
        rb.linearVelocity = direction * moveSpeed;
    }
}
