using UnityEngine;
using System.Collections;

public class PlayerSwordAttack : MonoBehaviour
{
    [Header("�U���ݒ�")]
    public Animator animator;           // �U���A�j���[�V����
    public float attackRange = 1.0f;    // �U���͈�
    public LayerMask enemyLayer;        // �G���C���[
    public GameObject attackEffect;     // �U���G�t�F�N�g
    public int swordDamage = 20;        // �_���[�W��

    [Header("�m�b�N�o�b�N�ݒ�")]
    public float knockbackForce = 5f;   // �m�b�N�o�b�N�̗�

    [Header("�N�[���_�E���ݒ�")]
    public float attackCooldown = 0.5f; // �U���Ԋu�i�b�j
    private bool isAttacking = false;   // �U�����t���O

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking)
        {
            StartCoroutine(SwordAttack());
        }
    }

    public IEnumerator SwordAttack()
    {
        isAttacking = true;

        // ��������
        float direction = transform.localScale.x < 0 ? -1 : 1;

        // �U���A�j���[�V�����Đ��i�K�v�Ȃ�j
        // animator.SetTrigger("Attack");

        // �U���G�t�F�N�g����
        Vector2 attackPos = (Vector2)transform.position + new Vector2(direction * attackRange, 0);
        if (attackEffect != null)
        {
            Instantiate(attackEffect, attackPos, Quaternion.identity);
        }

        // �U������i�Z���Ԃ����L���j
        yield return new WaitForSeconds(0.1f); // �U�����肪�o��^�C�~���O�𐧌�

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPos, 0.5f, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHPManager enemyHP = enemy.GetComponent<EnemyHPManager>();
            if (enemyHP != null)
            {
                enemyHP.EnemyTakeDamage(swordDamage);
                Debug.Log($"���œG���U���I {swordDamage}�_���[�W");
            }

            Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                Vector2 knockbackDir = (enemy.transform.position - transform.position).normalized;
                enemyRb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
            }
        }

        Debug.DrawLine(transform.position, attackPos, Color.red, 0.5f);

        // �N�[���_�E���ҋ@
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

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
