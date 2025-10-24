using UnityEngine;

public class PlayerHPUI : MonoBehaviour
{
    [Header("�n�[�g��GameObject")]
    [SerializeField] private GameObject[] redHearts;   // �Ԃ��n�[�g�̔z��
    [SerializeField] private GameObject[] grayHearts;  // �D�F�̃n�[�g�̔z��

    [Header("�v���C���[HP�Ǘ�")]
    [SerializeField] private PlayerHPManager playerHPManager;

    private int maxHP = 3;
    private int currentHP;

    private void Start()
    {
        currentHP = maxHP;
        UpdateHeartDisplay();
    }

    private void Update()
    {
        // PlayerHPManager���猻�݂�HP���擾���ĕ\�����X�V
        int newHP = GetCurrentHP();
        if (newHP != currentHP)
        {
            currentHP = newHP;
            UpdateHeartDisplay();
        }
    }

    /// <summary>
    /// PlayerHPManager���猻�݂�HP���擾
    /// </summary>
    private int GetCurrentHP()
    {
        if (playerHPManager != null)
        {
            // ���t���N�V������ currentHP ���擾
            var field = playerHPManager.GetType().GetField("currentHP",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                return (int)field.GetValue(playerHPManager);
            }
        }
        return currentHP;
    }

    /// <summary>
    /// �n�[�g�̕\�����X�V
    /// </summary>
    private void UpdateHeartDisplay()
    {
        for (int i = 0; i < maxHP; i++)
        {
            if (i < currentHP)
            {
                // HP���c���Ă��镔���͐Ԃ��n�[�g��\��
                redHearts[i].SetActive(true);
                grayHearts[i].SetActive(false);
            }
            else
            {
                // HP������ꂽ�����͊D�F�̃n�[�g��\��
                redHearts[i].SetActive(false);
                grayHearts[i].SetActive(true);
            }
        }
    }

    /// <summary>
    /// �O�����璼��HP�ύX��ʒm����ꍇ�Ɏg�p
    /// </summary>
    public void OnHPChanged(int newHP)
    {
        currentHP = Mathf.Clamp(newHP, 0, maxHP);
        UpdateHeartDisplay();
    }
}