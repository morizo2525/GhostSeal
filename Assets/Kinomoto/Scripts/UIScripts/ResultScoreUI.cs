using UnityEngine;
using UnityEngine.UI;

public class ResultScoreUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Text finalScoreText; // uGUI Text
    // Ç‹ÇΩÇÕ TMPro ÇégópÇ∑ÇÈèÍçá
    // [SerializeField] private TMP_Text finalScoreText;

    [Header("Display Settings")]
    [SerializeField] private string scorePrefix = "Final Score: ";

    void Start()
    {
        if (finalScoreText == null)
        {
            Debug.LogError("Final Score Text reference is not set in ResultScoreUI.");
            return;
        }

        DisplayFinalScore();
    }

    void DisplayFinalScore()
    {
        if (ScoreManager.Instance != null)
        {
            int finalScore = ScoreManager.Instance.GetTotalScore();
            finalScoreText.text = scorePrefix + finalScore.ToString();
            Debug.Log($"Final Score displayed: {finalScore}");
        }
        else
        {
            finalScoreText.text = scorePrefix + "0";
            Debug.LogWarning("ScoreManager instance not found in Result scene.");
        }
    }
}