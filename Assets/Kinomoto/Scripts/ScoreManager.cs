using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("敵撃破時のスコア設定")]
    private int totalScore;
    private const int scorePerHealthPoint = 10;
    private const int scorePerEnemyPoint = 20;

    [Header("スコアリセット設定")]
    [Tooltip("このシーンに遷移した時にスコアをリセット")]
    [SerializeField] private string gameSceneName = "GameScene"; // ゲームシーンの名前を設定

    void Awake()
    {
        // シングルトンパターン + シーン間で永続化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // シーン遷移時のイベントを登録
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        totalScore = 0;
        Debug.Log($"Score initialized: {totalScore}");
    }

    void OnDestroy()
    {
        // イベントの登録解除
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // シーンが読み込まれた時に呼ばれる
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ゲームシーンに遷移したらスコアをリセット
        if (scene.name == gameSceneName)
        {
            ResetScore();
        }
    }

    // 敵が倒された時に呼び出されるメソッド
    public void OnEnemyDefeated(int enemyMaxHealth)
    {
        int earnedScore = (enemyMaxHealth * scorePerHealthPoint) + scorePerEnemyPoint;
        totalScore += earnedScore;
        Debug.Log($"Enemy defeated! Earned: {earnedScore}, Total Score: {totalScore}");
    }

    public int GetTotalScore()
    {
        return totalScore;
    }

    // ゲーム再開時などにスコアをリセット
    public void ResetScore()
    {
        totalScore = 0;
        Debug.Log("Score reset");
    }
}