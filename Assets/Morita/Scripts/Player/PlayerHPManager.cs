using UnityEngine;

public class PlayerHPManager : MonoBehaviour
{
    [Header("最大HP")]
    [SerializeField] private int maxHP = 5;

    private int currentHP;

    private void Start()
    {
        currentHP = maxHP;
    }

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);

        Debug.Log($"プレイヤーがダメージを受けた！ 残りHP: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("敗北");
        // TODO: ゲームオーバー処理など
    }
}
