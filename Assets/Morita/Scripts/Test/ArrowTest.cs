using UnityEngine;

public class ArrowTest : MonoBehaviour
{
    private PlayerAttack_Bow arrowShooter;

    void Start()
    {
        // ����GameObject�ɃA�^�b�`���ꂽArrowShooter���擾
        arrowShooter = GetComponent<PlayerAttack_Bow>();

        if (arrowShooter == null)
        {
            Debug.LogError("ArrowShooter��������܂���I����GameObject��ArrowShooter���A�^�b�`���Ă��������B");
        }
    }

    void Update()
    {
        // �E�N���b�N�i�}�E�X�{�^��1�j�Ŕ���
        if (Input.GetMouseButtonDown(1))
        {
            if (arrowShooter != null)
            {
                arrowShooter.BowShoot();
            }
        }
    }
}
