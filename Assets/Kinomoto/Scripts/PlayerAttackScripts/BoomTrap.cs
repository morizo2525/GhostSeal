using UnityEngine;

public class BoomTrap : MonoBehaviour
{
    //�n���̃X�N���v�g

    [Header("�����̐ݒ�")]
    public float explosionRadius = 5f;      // �����͈̔�
    public int explosionDamage = 50;        // �����_���[�W
    public LayerMask enemyLayer;            // �G�̃��C���[

    [Header("�m�b�N�o�b�N�̐ݒ�")]
    public float knockbackForce = 5f;       // �m�b�N�o�b�N�̗�
    public bool affectPlayer = true;        // �v���C���[���m�b�N�o�b�N���邩
    public float maxKnockbackVelocity = 15f; // �m�b�N�o�b�N��̍ő呬�x
    public float playerKnockbackCooldown = 0.3f; // �v���C���[�̃m�b�N�o�b�N���G����

    [Header("�G�t�F�N�g�i�I�v�V�����j")]
    public GameObject explosionEffect;      // �����G�t�F�N�g��Prefab

    [Header("�N���ݒ�")]
    public float activationDelay = 0.5f;    // �ݒu��̋N���܂ł̒x�����ԁi�딚�h�~�j

    [Header("�T�E���h�ݒ�")]
    public AudioClip explosionSE;           // ����SE
    [Range(0f, 1f)]
    public float seVolume = 1.0f;           // SE�̉���

    private bool isActivated = false;       // �n�����N����Ԃ�
    private bool hasExploded = false;       // ���ɔ����������i�d���h�~�j

    private AnimationController animController;

    // �v���C���[�̍Ō�̃m�b�N�o�b�N�������L�^�istatic�ϐ��őS�n���ŋ��L�j
    private static float lastPlayerKnockbackTime = -999f;

    void Start()
    {
        animController = GetComponent<AnimationController>();

        //Player�Ƃ̓����蔻��𖳂���
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>());
        // �ݒu��A�����҂��Ă���N����Ԃɂ���i�v���C���[�̌딚�h�~�j
        Invoke(nameof(ActivateTrap), activationDelay);
    }

    /// <summary>
    /// �n�����N����Ԃɂ��āA�v���C���[�Ƃ̓����蔻��𕜊�����
    /// </summary>
    void ActivateTrap()
    {
        isActivated = true;
        // �v���C���[�Ƃ̓����蔻��𕜊�
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(), false);
        Debug.Log("�n�����N�����܂���");
    }

    /// <summary>
    /// �G���G�ꂽ�Ƃ��̏���
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        // �܂��N�����Ă��Ȃ��A�܂��͊��ɔ������Ă���ꍇ�͉������Ȃ�
        if (!isActivated || hasExploded)
        {
            return;
        }

        // �G���C���[�̃I�u�W�F�N�g���G�ꂽ���m�F
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            Debug.Log($"{other.gameObject.name} ���n���𓥂݂܂����I");
            Explode();
        }
        // �v���C���[���G�ꂽ�ꍇ������������
        else if (affectPlayer && other.CompareTag("Player"))
        {
            Debug.Log("�v���C���[���n���𓥂݂܂����I");
            Explode();
        }
    }

    /// <summary>
    /// ���������F�͈͓��̓G�Ƀ_���[�W�ƃm�b�N�o�b�N��^����
    /// </summary>
    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        // ����SE���Đ�
        if (explosionSE != null)
        {
            AudioSource.PlayClipAtPoint(explosionSE, transform.position, seVolume);
        }

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
            ApplyKnockback(enemy.gameObject, false);
        }

        // �v���C���[�ւ̃m�b�N�o�b�N�i�����t���j
        if (affectPlayer)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance <= explosionRadius)
                {
                    ApplyKnockback(player, true);
                }
            }
        }

        // �f�o�b�O�p
        Debug.Log($"�n���������I {hitEnemies.Length}�̂̓G�Ƀ_���[�W");

        // �n���I�u�W�F�N�g���폜
        Destroy(gameObject);
    }

    /// <summary>
    /// �m�b�N�o�b�N��K�p����
    /// </summary>
    void ApplyKnockback(GameObject target, bool isPlayer)
    {
        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // �v���C���[�̏ꍇ�A�N�[���_�E�����Ȃ疳��
        if (isPlayer)
        {
            if (Time.time - lastPlayerKnockbackTime < playerKnockbackCooldown)
            {
                Debug.Log("�v���C���[�̃m�b�N�o�b�N�N�[���_�E����");
                return;
            }
            lastPlayerKnockbackTime = Time.time;
        }

        // �������S����^�[�Q�b�g�ւ̕���
        Vector2 direction = (target.transform.position - transform.position).normalized;

        // �����ɉ����ăm�b�N�o�b�N�͂�����
        float distance = Vector2.Distance(transform.position, target.transform.position);
        float forceFalloff = 1f - (distance / explosionRadius); // �����قǎキ�Ȃ�

        // �m�b�N�o�b�N��K�p
        rb.AddForce(direction * knockbackForce * forceFalloff, ForceMode2D.Impulse);

        // ���x�̏����K�p�i�v���C���[�̂݁A�܂��͑S�́j
        if (isPlayer)
        {
            // ���t���[���ő��x������K�p
            StartCoroutine(LimitVelocity(rb));
        }
    }

    /// <summary>
    /// ���x�𐧌�����
    /// </summary>
    private System.Collections.IEnumerator LimitVelocity(Rigidbody2D rb)
    {
        yield return new WaitForFixedUpdate();

        if (rb != null && rb.linearVelocity.magnitude > maxKnockbackVelocity)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxKnockbackVelocity;
            Debug.Log($"�m�b�N�o�b�N���x�𐧌�: {maxKnockbackVelocity}");
        }
    }

    // Gizmo�Ŕ����͈͂�\���i�G�f�B�^��Ŋm�F�p�j
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}