using UnityEngine;

public class PlayerHPUI : MonoBehaviour
{
    [Header("ハートのGameObject")]
    [SerializeField] private GameObject[] redHearts;   // 赤いハートの配列
    [SerializeField] private GameObject[] grayHearts;  // 灰色のハートの配列

    [Header("プレイヤーHP管理")]
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
        // PlayerHPManagerから現在のHPを取得して表示を更新
        int newHP = GetCurrentHP();
        if (newHP != currentHP)
        {
            currentHP = newHP;
            UpdateHeartDisplay();
        }
    }

    /// <summary>
    /// PlayerHPManagerから現在のHPを取得
    /// </summary>
    private int GetCurrentHP()
    {
        if (playerHPManager != null)
        {
            // リフレクションで currentHP を取得
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
    /// ハートの表示を更新
    /// </summary>
    private void UpdateHeartDisplay()
    {
        for (int i = 0; i < maxHP; i++)
        {
            if (i < currentHP)
            {
                // HPが残っている部分は赤いハートを表示
                redHearts[i].SetActive(true);
                grayHearts[i].SetActive(false);
            }
            else
            {
                // HPが失われた部分は灰色のハートを表示
                redHearts[i].SetActive(false);
                grayHearts[i].SetActive(true);
            }
        }
    }

    /// <summary>
    /// 外部から直接HP変更を通知する場合に使用
    /// </summary>
    public void OnHPChanged(int newHP)
    {
        currentHP = Mathf.Clamp(newHP, 0, maxHP);
        UpdateHeartDisplay();
    }
}