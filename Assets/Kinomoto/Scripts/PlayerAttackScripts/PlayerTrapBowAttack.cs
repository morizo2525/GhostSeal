using UnityEngine;

public class PlayerTrapBowAttack : MonoBehaviour
{
    [Header("��̐ݒ�")]
    [SerializeField] private GameObject trapArrowPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootPower = 10f;

    [Header("�g���b�v��N�[���^�C��")]
    [SerializeField] private float trapArrowCooldown = 3f;
    private float lastTrapArrowTime = -999f;

    /// <summary>
    /// �g���b�v��𔭎�
    /// </summary>
    public void BowShootTrapArrow()
    {
        if (Time.time - lastTrapArrowTime < trapArrowCooldown)
        {
            Debug.Log($"�g���b�v��̃N�[���^�C�����ł��B�c�莞��: {trapArrowCooldown - (Time.time - lastTrapArrowTime):F1}�b");
            return;
        }

        if (trapArrowPrefab == null)
        {
            Debug.LogError("�g���b�v���Prefab���ݒ肳��Ă��܂���");
            return;
        }

        ShootArrow(trapArrowPrefab);
        lastTrapArrowTime = Time.time;
        Debug.Log("�g���b�v��𔭎˂��܂���");
    }

    /// <summary>
    /// ��𔭎˂���������\�b�h
    /// </summary>
    private void ShootArrow(GameObject arrowToUse)
    {
        if (shootPoint == null)
        {
            Debug.LogError("���ˈʒu���ݒ肳��Ă��܂���");
            return;
        }

        // �}�E�X�ʒu�擾�i���[���h���W�j
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // ���˕����x�N�g���i�ˊp�����Ȃ��j
        Vector2 dir = (mouseWorld - shootPoint.position).normalized;

        // ��𐶐�
        GameObject arrow = Instantiate(arrowToUse, shootPoint.position, Quaternion.identity);

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