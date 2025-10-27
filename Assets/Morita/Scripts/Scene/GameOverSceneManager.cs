using UnityEngine;
using UnityEngine.UI;

public class GameOverSceneManager : MonoBehaviour
{
    [SerializeField] private Button retryButton;      // リトライボタン
    [SerializeField] private Button titleButton;      // タイトルボタン
    [SerializeField] private string gameSceneName = "GameScene";   // ゲームシーン名
    [SerializeField] private string titleSceneName = "TitleScene"; // タイトルシーン名

    private void Start()
    {
        // ボタンが押されたときの処理を登録
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(OnRetryButtonClicked);
        }

        if (titleButton != null)
        {
            titleButton.onClick.AddListener(OnTitleButtonClicked);
        }
    }

    /// <summary>
    /// リトライボタンが押されたときの処理
    /// </summary>
    private void OnRetryButtonClicked()
    {
        // SceneTransitionを使ってゲームシーンへ遷移
        SceneTransition.LoadScene("Main");
    }

    /// <summary>
    /// タイトルボタンが押されたときの処理
    /// </summary>
    private void OnTitleButtonClicked()
    {
        // SceneTransitionを使ってタイトルシーンへ遷移
        SceneTransition.LoadScene("TitleScene");
    }
}
