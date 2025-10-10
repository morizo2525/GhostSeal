using UnityEngine;

public class BombBoom : MonoBehaviour
{
    [Header("�����̐ݒ�")]
    public float explosionDelay = 3f;       // �����܂ł̎��ԁi�b�j
    public float explosionRadius = 5f;      // �����͈̔�
    public int explosionDamage = 50;        // �����_���[�W
    public LayerMask enemyLayer;            // �G�̃��C���[

    [Header("�m�b�N�o�b�N�̐ݒ�")]
    public float knockbackForce = 5f;       // �m�b�N�o�b�N�̗�
    public bool affectPlayer = true;        // �v���C���[���m�b�N�o�b�N���邩

    [Header("�G�t�F�N�g�i�I�v�V�����j")]
    public GameObject explosionEffect;      // �����G�t�F�N�g��Prefab

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= explosionDelay)
        {
            Explode();
        }
    }

    /// <summary>
    /// ���������F�͈͓��̓G�Ƀ_���[�W��^����
    /// </summary>
    void Explode()
    {
        // �����G�t�F�N�g�𐶐��i�ݒ肳��Ă���ꍇ�j
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // �͈͓��̓G���������ă_���[�W�{�m�b�N�o�b�N
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            // �_���[�W����
            EnemyHPManager enemyHP = enemy.GetComponent<EnemyHPManager>();
            if (enemyHP != null)
            {
                enemyHP.EnemyTakeDamage(explosionDamage);
            }

            // �m�b�N�o�b�N����
            ApplyKnockback(enemy.gameObject);
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
                    ApplyKnockback(player);
                }
            }
        }

        // �f�o�b�O�p
        Debug.Log($"�����I {hitEnemies.Length}�̂̓G�Ƀ_���[�W");

        // ���e�I�u�W�F�N�g���폜
        Destroy(gameObject);
    }

    /// <summary>
    /// �m�b�N�o�b�N��K�p����
    /// </summary>
    void ApplyKnockback(GameObject target)
    {
        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // �������S����^�[�Q�b�g�ւ̕���
        Vector2 direction = (target.transform.position - transform.position).normalized;

        // �����ɉ����ăm�b�N�o�b�N�͂�����
        float distance = Vector2.Distance(transform.position, target.transform.position);
        float forceFalloff = 1f - (distance / explosionRadius); // �����قǎキ�Ȃ�

        // �m�b�N�o�b�N��K�p
        rb.AddForce(direction * knockbackForce * forceFalloff, ForceMode2D.Impulse);
    }

    // Gizmo�Ŕ����͈͂�\���i�G�f�B�^��Ŋm�F�p�j
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}