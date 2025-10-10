using UnityEngine;

public class PlayerAttack_Bow : MonoBehaviour
{
    [Header("��̐ݒ�")]
    public GameObject arrowPrefab;      // ���˂�����Prefab
    public Transform shootPoint;        // ���ˈʒu
    public float shootPower = 10f;      // ���˂̏���

    [Header("�ˊp�����i������̉���j")]
    [Tooltip("���̊p�x�͈͓��i������j�ł̂ݔ��˂ł���")]
    [Range(0, 120)] public float minAngle = 15f;
    [Range(0, 120)] public float maxAngle = 75f;

    public void BowShoot()
    {
        // �}�E�X�ʒu�擾�i���[���h���W�j
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // ���˕����x�N�g��
        Vector2 dir = (mouseWorld - shootPoint.position).normalized;

        // �������̏ꍇ�͔��˂��Ȃ�
        if (dir.y <= 0)
            return;

        // �}�E�X�����E�ǂ��瑤�ɂ��邩�𔻒�
        bool isLeftSide = dir.x < 0;

        // ������i�}�E�X���̐��������j
        Vector2 baseDir = isLeftSide ? Vector2.left : Vector2.right;

        // ���ˊp�x���v�Z�i�����������������̊p�x�j
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

        // ��𐶐�
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);

        // ��̌�����ݒ�
        arrow.transform.right = dir;

        // Rigidbody2D�ŕ�������
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = dir * shootPower;
        }
    }
}
