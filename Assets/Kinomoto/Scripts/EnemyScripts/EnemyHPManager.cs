using UnityEngine;

public class EnemyHPManager : MonoBehaviour
{
    [Header("HP Settings")]
    [Tooltip("�G��HP�ŏ��l")]
    public int minHealth = 50;

    [Tooltip("�G��HP�ő�l")]
    public int maxHealth = 150;

    [Header("Size Settings")]
    [Tooltip("�ŏ�HP�̎��̃T�C�Y�{��")]
    public float minSizeMultiplier = 0.5f;

    [Tooltip("�ő�HP�̎��̃T�C�Y�{��")]
    public float maxSizeMultiplier = 2.0f;

    private int enemyHealth;
    private int enemyMaxHealth;

    void Start()
    {
        InitializeEnemy();
    }

    // �G�̏������i�X�|�[�����ɌĂяo�����Ƃ��\�j
    public void InitializeEnemy()
    {
        // �����_����HP������
        enemyMaxHealth = Random.Range(minHealth, maxHealth + 1);
        enemyHealth = enemyMaxHealth;

        // HP�ɉ����ăT�C�Y�𒲐�
        AdjustSizeBasedOnHP();

        Debug.Log($"Enemy spawned with HP: {enemyHealth}, Size: {transform.localScale}");
    }

    // HP�Ɋ�Â��ăT�C�Y�𒲐�
    void AdjustSizeBasedOnHP()
    {
        // HP�̊������v�Z�i0.0?1.0�j
        float healthRatio = (float)(enemyMaxHealth - minHealth) / (maxHealth - minHealth);

        // �T�C�Y�{������`��ԂŌv�Z
        float sizeMultiplier = Mathf.Lerp(minSizeMultiplier, maxSizeMultiplier, healthRatio);

        // �T�C�Y��K�p
        transform.localScale = Vector3.one * sizeMultiplier;
    }

    public void EnemyTakeDamage(int damage)
    {
        enemyHealth -= damage;

        // �f�o�b�O�p
        Debug.Log($"Enemy Health: {enemyHealth}/{enemyMaxHealth}");

        if (enemyHealth <= 0)
        {
            EnemyDie();
        }
    }

    void EnemyDie()
    {
        Debug.Log("Enemy died");
        Destroy(gameObject);
    }

    // ���݂�HP���擾�i���̃X�N���v�g����Q�Ɨp�j
    public int GetCurrentHealth()
    {
        return enemyHealth;
    }

    // �ő�HP���擾�i���̃X�N���v�g����Q�Ɨp�j
    public int GetMaxHealth()
    {
        return enemyMaxHealth;
    }
}
