using UnityEngine;

public class BoomArrow : MonoBehaviour
{
    //���e���ɔ��������̋���
    public GameObject explosionEffect;
    public float explosionRadius = 4f;
    public int explosionDamage = 40;
    public LayerMask enemyLayer;
    public float knockbackForce = 5f;
    public bool affectPlayer = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���e�����u�Ԃɔ���
        Explode();
    }

    void Explode()
    {
        // �����G�t�F�N�g����
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // �͈͓��̓G���擾
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHPManager enemyHP = enemy.GetComponent<EnemyHPManager>();
            if (enemyHP != null)
            {
                enemyHP.EnemyTakeDamage(explosionDamage);
            }

            // �m�b�N�o�b�N
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 dir = (enemy.transform.position - transform.position).normalized;
                rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
            }
        }

        // �v���C���[�ւ̃m�b�N�o�b�N
        if (affectPlayer)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance <= explosionRadius)
                {
                    Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 dir = (player.transform.position - transform.position).normalized;
                        rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
                    }
                }
            }
        }

        // �f�o�b�O�o��
        Debug.Log($"���e������I �G��: {hitEnemies.Length}");

        // ����폜
        Destroy(gameObject);
    }

    // �G�f�B�^��Ŕ͈͊m�F
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
