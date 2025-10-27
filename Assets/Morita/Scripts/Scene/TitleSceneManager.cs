using UnityEngine;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] private Button startButton;   // UnityのInspectorでボタンをアサイン
    [SerializeField] private string gameSceneName = "GameScene";  // 遷移先のシーン名
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite newBackgroundSprite;

    private void Start()
    {
        // ボタンが押されたときの処理を登録
        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        // 背景画像を切り替え
        if (backgroundImage != null && newBackgroundSprite != null)
        {
            backgroundImage.sprite = newBackgroundSprite;
        }

        // SceneTransitionを使って遷移
        SceneTransition.LoadScene("Main");
    }
}
