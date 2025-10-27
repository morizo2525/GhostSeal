using UnityEngine;

public class PlayerHPManager : MonoBehaviour
{
    [Header("最大HP")]
    [SerializeField] private int maxHP = 5;

    private int currentHP;

    public delegate void OnPlayerDie();  // 死亡イベント
    public event OnPlayerDie PlayerDied; 

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

        PlayerDied?.Invoke(); // プレイヤー死亡イベントを発生
    }

    public int GetCurrentHP() => currentHP;
    public int GetMaxHP() => maxHP;
}
