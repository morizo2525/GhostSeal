using UnityEngine;
using UnityEngine.UI;

public class GameScoreUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Text scoreText; // uGUI Text
    // または TMPro を使用する場合
    // [SerializeField] private TMP_Text scoreText;

    [Header("Display Settings")]
    [SerializeField] private string scorePrefix = "Score: ";
    [SerializeField] private float updateInterval = 0.1f; // 更新頻度（秒）

    private float updateTimer;

    void Start()
    {
        if (scoreText == null)
        {
            Debug.LogError("Score Text reference is not set in GameScoreUI.");
            return;
        }

        UpdateScoreDisplay();
    }

    void Update()
    {
        // 一定間隔でスコア表示を更新
        updateTimer += Time.deltaTime;
        if (updateTimer >= updateInterval)
        {
            UpdateScoreDisplay();
            updateTimer = 0f;
        }
    }

    void UpdateScoreDisplay()
    {
        if (ScoreManager.Instance != null)
        {
            int currentScore = ScoreManager.Instance.GetTotalScore();
            scoreText.text = scorePrefix + currentScore.ToString();
        }
        else
        {
            scoreText.text = scorePrefix + "0";
        }
    }
}