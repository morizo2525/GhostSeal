using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class AirEnemyMove : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    [SerializeField] private float moveSpeed = 4f;

    [Header("�h��ݒ�")]
    [SerializeField] private float waveAmplitude = 1f;  //�h�ꕝ(�㉺�̐U�ꕝ)
    [SerializeField] private float waveFrequency = 2f;  //�h��X�s�[�h(���g��)

    private Rigidbody2D rb;
    private Transform player;
    private float waveTimer;  //�h��̃^�C�}�[

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

        // �h��̃^�C�}�[���X�V
        waveTimer += Time.fixedDeltaTime;

        // �v���C���[�ւ̕����x�N�g�����v�Z
        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        // �i�s�����ɑ΂��Đ����ȃx�N�g�����v�Z(�㉺�̗h��p)
        Vector2 perpendicular = new Vector2(-directionToPlayer.y, directionToPlayer.x);

        // �T�C���g�Ŋ��炩�ȗh��𐶐�
        float waveOffset = Mathf.Sin(waveTimer * waveFrequency) * waveAmplitude;

        // ��{�̈ړ����� + �h��
        Vector2 finalDirection = directionToPlayer + perpendicular * waveOffset;

        // �v���C���[�Ɍ������ėh��Ȃ���ړ�
        rb.linearVelocity = finalDirection.normalized * moveSpeed;
    }
}
