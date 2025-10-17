using UnityEngine;

public class Arrow : MonoBehaviour
{
    //��̃_���[�W����X�N���v�g�i2D�p�j
    //����������Ƃ��ɓG�Ƀ_���[�W��^����

    [Header("�_���[�W�ݒ�")]
    [Tooltip("��^����_���[�W��")]
    public int damage = 10;

    [Header("�j��ݒ�")]
    [Tooltip("�I�u�W�F�N�g�ɓ���������ɖ��j�󂷂邩")]
    public bool destroyOnHit = true;

    [Tooltip("�������Ă���j�󂷂�܂ł̒x�����ԁi�b�j")]
    public float destroyDelay = 0.1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleHit(collision.gameObject);
    }

    // �����蔻��̏���
    private void HandleHit(GameObject hitObject)
    {
        Debug.Log($"� {hitObject.name} �ɓ�����܂����I");

        // �G�^�O�����I�u�W�F�N�g�̏ꍇ�A�_���[�W��^����
        if (hitObject.CompareTag("Enemy"))
        {
            EnemyHPManager enemyHP = hitObject.GetComponent<EnemyHPManager>();

            if (enemyHP != null)
            {
                enemyHP.EnemyTakeDamage(damage);
                Debug.Log($"�G�Ƀ_���[�W�I {damage}�_���[�W");
            }
            else
            {
                Debug.LogWarning($"{hitObject.name} ��EnemyHPManager������܂���I");
            }
        }

        // �ǂ�ȃI�u�W�F�N�g�ɓ������Ă����j��
        if (destroyOnHit)
        {
            Destroy(gameObject, destroyDelay);
        }
    }
}