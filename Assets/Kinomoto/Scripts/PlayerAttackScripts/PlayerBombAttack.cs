using UnityEngine;

public class PlayerBombAttack : MonoBehaviour
{
    [Header("���e�̐ݒ�")]
    public GameObject bombPrefab;           // �������锚�e��Prefab
    public Transform throwPoint;            // �����ʒu
    public float throwPower = 8f;           // �����̏���

    [Header("�����p�x����")]
    [Tooltip("���̊p�x�͈͓��ł̂ݓ����ł���")]
    [Range(0, 120)] public float minAngle = 15f;
    [Range(0, 120)] public float maxAngle = 75f;

    [Header("�f�o�b�O�ݒ�")]
    public bool showTrajectory = true;      // �O���\����\��
    public Color trajectoryColor = Color.yellow;
    public int trajectoryPoints = 30;       // �O���̓_�̐�

    /// <summary>
    /// ���e�𓊝�����
    /// </summary>
    public void ThrowBomb()
    {
        if (bombPrefab == null)
        {
            Debug.LogError("���e��Prefab���ݒ肳��Ă��܂���I");
            return;
        }

        if (throwPoint == null)
        {
            Debug.LogError("�����ʒu�iThrowPoint�j���ݒ肳��Ă��܂���I");
            return;
        }

        // �}�E�X�ʒu�擾�i���[���h���W�j
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // ���������x�N�g��
        Vector2 dir = (mouseWorld - throwPoint.position).normalized;

        // �������̏ꍇ�͓������Ȃ�
        if (dir.y <= 0)
        {
            Debug.Log("���e�͉������ɓ������܂���");
            return;
        }

        // �}�E�X�����E�ǂ��瑤�ɂ��邩�𔻒�
        bool isLeftSide = dir.x < 0;

        // ������i�}�E�X���̐��������j
        Vector2 baseDir = isLeftSide ? Vector2.left : Vector2.right;

        // �����p�x���v�Z�i�����������������̊p�x�j
        float angle = Vector2.Angle(baseDir, dir);

        // �p�x��������ɃN�����v�i�����j
        angle = Mathf.Clamp(angle, minAngle, maxAngle);

        // �N�����v���ꂽ�p�x�ŕ����x�N�g�����Čv�Z
        if (isLeftSide)
        {
            // ����: 180�x����p�x������
            dir = Quaternion.Euler(0, 0, 180 - angle) * Vector2.right;
        }
        else
        {
            // �E��: ���̂܂܊p�x���g��
            dir = Quaternion.Euler(0, 0, angle) * Vector2.right;
        }

        // ���e�𐶐�
        GameObject bomb = Instantiate(bombPrefab, throwPoint.position, Quaternion.identity);

        // ���e�̌�����ݒ�i��]���������ꍇ�j
        bomb.transform.right = dir;

        // Rigidbody2D�ŕ�������
        Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = dir * throwPower;
        }
        else
        {
            Debug.LogWarning("���ePrefab��Rigidbody2D���A�^�b�`����Ă��܂���");
        }

        Debug.Log($"���e�𓊝����܂����B�p�x: {angle:F1}�x");
    }

    /// <summary>
    /// �O���\����`��i�f�o�b�O�p�j
    /// </summary>
    void OnDrawGizmos()
    {
        if (!showTrajectory || throwPoint == null) return;

        // �}�E�X�ʒu�擾
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // ���������x�N�g��
        Vector2 dir = (mouseWorld - throwPoint.position).normalized;

        // �������̏ꍇ�͕`�悵�Ȃ�
        if (dir.y <= 0) return;

        // �}�E�X�����E�ǂ��瑤�ɂ��邩�𔻒�
        bool isLeftSide = dir.x < 0;
        Vector2 baseDir = isLeftSide ? Vector2.left : Vector2.right;

        // �p�x�v�Z�ƃN�����v
        float angle = Vector2.Angle(baseDir, dir);
        angle = Mathf.Clamp(angle, minAngle, maxAngle);

        // �N�����v���ꂽ�����x�N�g��
        if (isLeftSide)
        {
            dir = Quaternion.Euler(0, 0, 180 - angle) * Vector2.right;
        }
        else
        {
            dir = Quaternion.Euler(0, 0, angle) * Vector2.right;
        }

        // �O����`��
        Gizmos.color = trajectoryColor;
        Vector2 velocity = dir * throwPower;
        Vector2 position = throwPoint.position;
        float timeStep = 0.1f;

        for (int i = 0; i < trajectoryPoints; i++)
        {
            Vector2 nextPosition = position + velocity * timeStep;
            velocity += Physics2D.gravity * timeStep;

            Gizmos.DrawLine(position, nextPosition);
            position = nextPosition;
        }
    }
}