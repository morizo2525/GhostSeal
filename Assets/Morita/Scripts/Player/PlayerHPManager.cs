using UnityEngine;

public class PlayerHPManager : MonoBehaviour
{
    [Header("�ő�HP")]
    [SerializeField] private int maxHP = 5;

    private int currentHP;

    private void Start()
    {
        currentHP = maxHP;
    }

    /// <summary>
    /// �_���[�W���󂯂�
    /// </summary>
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);

        Debug.Log($"�v���C���[���_���[�W���󂯂��I �c��HP: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("�s�k");
        // TODO: �Q�[���I�[�o�[�����Ȃ�
    }
}
