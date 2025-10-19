using UnityEngine;

public class PlayerTrapBowAttack : MonoBehaviour
{
    [Header("��̐ݒ�")]
    [SerializeField] private GameObject trapArrowPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootPower = 10f;

    /// <summary>
    /// �g���b�v��𔭎�
    /// </summary>
    public void BowShootTrapArrow()
    {
        if (trapArrowPrefab == null)
        {
            Debug.LogError("�g���b�v���Prefab���ݒ肳��Ă��܂���");
            return;
        }

        ShootArrow(trapArrowPrefab);
        Debug.Log("�g���b�v��𔭎˂��܂���");
    }

    /// <summary>
    /// ��𔭎˂��郁�\�b�h
    /// </summary>
    private void ShootArrow(GameObject arrowToUse)
    {
        if (shootPoint == null)
        {
            Debug.LogError("���ˈʒu���ݒ肳��Ă��܂���");
            return;
        }

        // �}�E�X�̈ʒu�擾
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Vector2 dir = (mouseWorld - shootPoint.position).normalized;

        // ��𐶐�
        GameObject arrow = Instantiate(arrowToUse, shootPoint.position, Quaternion.identity);

        // ��̌�����ݒ�
        arrow.transform.right = dir;

        //����
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = dir * shootPower;
        }
    }
}