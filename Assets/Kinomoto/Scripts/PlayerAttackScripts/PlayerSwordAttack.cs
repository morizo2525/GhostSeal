using UnityEngine;

public class PlayerSwordAttack : MonoBehaviour
{
    //�v���C���[�̌��U�����Ǘ�����X�N���v�g
    //�U���̃A�j���[�V�����A�U������A�G�t�F�N�g�̐����Ȃ�
    //�U���̔���̓v���C���[�������Ă���O���ɐ��������

    public Animator animator;           //�U���̃A�j���\�V����
    public float attackRange = 1.0f;    //�U���͈�
    public LayerMask enemyLayer;        //�G�̃��C���[
    public GameObject attackEffect;     //�U���G�t�F�N�g
    public int swordDamage = 20;        //���̃_���[�W��

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SwordAttack(); //���U�����\�b�h
        }
    }

    public void SwordAttack()
    {
        // �v���C���[�̌����𔻒�ilocalScale.x�����Ȃ獶�����A���Ȃ�E�����j
        float direction = transform.localScale.x < 0 ? -1 : 1;

        //�U���A�j���[�V�����Đ�
        //animator.SetTrigger("Attack");

        //�v���C���[�̑O���ɍU������𐶐�
        Vector2 attackPos = (Vector2)transform.position + new Vector2(direction * attackRange, 0);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPos, 0.5f, enemyLayer);

        //�U���G�t�F�N�g�̐���
        if (attackEffect != null)
        {
            //Instantiate(attackEffect, attackPos, Quaternion.identity);
        }

        //�G�Ƀ_���[�W��^����
        foreach (Collider2D enemy in hitEnemies)
        {
            //�G�̃_���[�W����
            EnemyHPManager enemyHP = enemy.GetComponent<EnemyHPManager>();
            if (enemyHP != null)
            {
                enemyHP.EnemyTakeDamage(swordDamage);
                Debug.Log($"���œG���U���I {swordDamage}�_���[�W");
            }
        }

        // �f�o�b�O�p�F�U���ʒu������
        Debug.DrawLine(transform.position, attackPos, Color.red, 0.5f);
    }

    // Gizmo�ōU���͈͂������iScene View�Ŋm�F�p�j
    private void OnDrawGizmosSelected()
    {
        if (transform != null)
        {
            float direction = transform.localScale.x < 0 ? -1 : 1;
            Vector2 attackPos = (Vector2)transform.position + new Vector2(direction * attackRange, 0);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPos, 0.5f);
        }
    }
}