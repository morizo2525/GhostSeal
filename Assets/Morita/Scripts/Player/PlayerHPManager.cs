using UnityEngine;

public class PlayerHPManager : MonoBehaviour
{
    [Header("最大HP")]
    [SerializeField] private int maxHP = 5;
    private int currentHP;
    private bool isDead = false; // 死亡フラグ

    [Header("サウンド設定")]
    [SerializeField] private AudioClip damageSE;    // ダメージを受けた時のSE
    [SerializeField] private AudioClip deathSE;     // 死亡時のSE
    [Range(0f, 1f)]
    [SerializeField] private float seVolume = 1.0f; // SEの音量
    private AudioSource audioSource;

    public delegate void OnPlayerDie();  // 死亡イベント
    public event OnPlayerDie PlayerDied;

    private void Start()
    {
        currentHP = maxHP;

        // AudioSourceコンポーネントを取得または追加
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);

        Debug.Log($"プレイヤーがダメージを受けた！ 残りHP: {currentHP}");

        // ダメージSEを再生
        if (damageSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(damageSE, seVolume);
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return; // 二重呼び出し防止
        isDead = true; // 死亡フラグON

        Debug.Log("敗北");

        // 死亡SEを再生
        if (deathSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSE, seVolume);
        }

        PlayerDied?.Invoke(); // プレイヤー死亡イベントを発生
    }

    public int GetCurrentHP() => currentHP;
    public int GetMaxHP() => maxHP;
    public bool IsDead => isDead; // 外部参照用
}