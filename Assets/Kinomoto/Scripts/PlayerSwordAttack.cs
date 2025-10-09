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

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            SwordAttack(); //���U�����\�b�h
        }
    }

    public void SwordAttack()
    {
        //�U���A�j���[�V�����Đ�
        animator.SetTrigger("Attack");

        //�v���C���[�̑O���ɍU������𐶐�
        Vector2 attackPos = (Vector2)transform.position + (Vector2)(transform.right * attackRange);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPos, 0.5f, enemyLayer);

        //�U���G�t�F�N�g�̐���
        if (attackEffect != null)
        {
            Instantiate(attackEffect, attackPos, Quaternion.identity);
        }

        //�G�Ƀ_���[�W��^����
        foreach (Collider2D enemy in hitEnemies)
        {
            //�G�̃_���[�W����
        }
    }
}
