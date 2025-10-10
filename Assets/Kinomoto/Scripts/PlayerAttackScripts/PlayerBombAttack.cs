using UnityEngine;

public class PlayerBombAttack : MonoBehaviour
{
    //�v���C���[�̔��e�U�����Ǘ�����X�N���v�g
    //���e���������镔���̓��\�b�h�ō쐬���A��̑g�ݍ��킹�ōė��p�ł���悤�ɂ���
    //�܂��Abomb�𓊂��铮��͕ʂ̃��\�b�h�Ƃقړ����Ȃ̂ŁAPlayerAttack_Bow��BowShoot()���ė��p����

    [Header("���e�̐ݒ�")]
    public GameObject bombPrefab;           // �����锚�e��Prefab

    [Header("�����͂̐ݒ�")]
    [Range(0.1f, 2.0f)]
    public float throwPowerMultiplier = 0.6f; // �����͂̔{���i1.0 = ��Ɠ����j

    [Header("�Q��")]
    public PlayerAttack_Bow bowAttackScript; // �|�U���X�N���v�g�ւ̎Q��

    public void ThrowBomb()
    {
        if (bowAttackScript == null)
        {
            Debug.LogError("PlayerAttack_Bow�X�N���v�g���ݒ肳��Ă��܂���I");
            return;
        }

        // ���̖��Prefab���ꎞ�ۑ�
        GameObject originalArrow = bowAttackScript.arrowPrefab;
        float originalPower = bowAttackScript.shootPower;

        // ���Prefab�𔚒e�ɍ����ւ�
        bowAttackScript.arrowPrefab = bombPrefab;

        // ���˗͂𒲐��i���e�͔�тɂ�������j
        bowAttackScript.shootPower = originalPower * throwPowerMultiplier;

        // BowShoot()�����s�i���e�����˂����j
        bowAttackScript.BowShoot();

        // ���Prefab�����ɖ߂�
        bowAttackScript.arrowPrefab = originalArrow;
        bowAttackScript.shootPower = originalPower;
    }
}
