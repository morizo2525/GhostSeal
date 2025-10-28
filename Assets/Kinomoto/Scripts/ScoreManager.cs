using UnityEngine;


//EnemyHPManagerを参照して、敵の最大HPに基づいてスコアを計算・管理するスクリプト
public class ScoreManager : MonoBehaviour
{
    [Header("敵撃破時のスコア設定")]
    [SerializeField]
    private EnemyHPManager enemyHPManager;　//EnemyHPManagerへの参照
    private int score;
    private int enemyMaxHealth;
    private const int scorePerHealthPoint = 10; // HP1ポイントあたりのスコア
    private const int scorePerEnemyPoint = 20; // 敵1体あたりの追加スコア

    void Start()
    {
        if (enemyHPManager == null)
        {
            Debug.LogError("EnemyHPManager reference is not set in ScoreManager.");
            return;
        }

        // 敵の最大HPを取得
        enemyMaxHealth = enemyHPManager.GetMaxHealth();

        // スコアを計算
        CalculateScore();

        // デバッグ用にスコアを表示
        Debug.Log($"Calculated Score: {score}");
    }

    void CalculateScore()
    {
        score = (enemyMaxHealth * scorePerHealthPoint) + scorePerEnemyPoint;
    }
}
