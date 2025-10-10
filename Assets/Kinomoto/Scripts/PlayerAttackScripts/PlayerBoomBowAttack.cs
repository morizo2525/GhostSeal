using UnityEngine;

public class PlayerBoomBowAttack : MonoBehaviour
{
    [Header("���e��̐ݒ�")]
    public GameObject boomArrowPrefab;   // ����������Prefab�i�ʏ�̖�ɔ����@�\��ǉ��������́j
    public GameObject explosionEffect;   // �����G�t�F�N�g
    public float explosionRadius = 4f;   // �����͈�
    public int explosionDamage = 40;     // �����_���[�W
    public LayerMask enemyLayer;         // �G���C���[

    [Header("�Q��")]
    public PlayerAttack_Bow bowAttackScript;  // �|�X�N���v�g�̎Q��

    /// <summary>
    /// �������������
    /// </summary>
    public void ShootBoomArrow()
    {
        if (bowAttackScript == null)
        {
            Debug.LogError("PlayerAttack_Bow�X�N���v�g���ݒ肳��Ă��܂���I");
            return;
        }

        // ���̖�Prefab��ۑ�
        GameObject originalArrow = bowAttackScript.arrowPrefab;

        // �ꎞ�I�ɔ�����Prefab���Z�b�g
        bowAttackScript.arrowPrefab = boomArrowPrefab;

        // ���ˏ������ė��p
        bowAttackScript.BowShoot();

        // ���ɖ߂�
        bowAttackScript.arrowPrefab = originalArrow;
    }
}