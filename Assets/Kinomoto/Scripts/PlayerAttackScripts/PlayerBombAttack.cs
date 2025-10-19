using UnityEngine;

public class PlayerBombAttack : MonoBehaviour
{
    [Header("���e�̐ݒ�")]
    public GameObject bombPrefab;           // �������锚�e��Prefab
    public Transform throwPoint;            // �����ʒu
    public float throwPower = 8f;           // �����̏���

    [Header("�f�o�b�O�ݒ�")]
    public bool showTrajectory = true;      // �O���\����\��
    public Color trajectoryColor = Color.yellow;
    public int trajectoryPoints = 30;       // �O���̓_�̐�

    /// <summary>
    /// ���e�𓊝�����i�p�x�����Ȃ��j
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

        // �}�E�X�ʒu�擾
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // ��������
        Vector2 dir = (mouseWorld - throwPoint.position).normalized;

        // ���e�𐶐�
        GameObject bomb = Instantiate(bombPrefab, throwPoint.position, Quaternion.identity);

        // ���e�̌�����ݒ�i�C�Ӂj
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

        Debug.Log("���e�𓊝����܂���");
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