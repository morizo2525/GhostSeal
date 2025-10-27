using UnityEngine;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] private Button startButton;   // UnityのInspectorでボタンをアサイン
    [SerializeField] private string gameSceneName = "GameScene";  // 遷移先のシーン名

    private void Start()
    {
        // ボタンが押されたときの処理を登録
        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        // SceneTransitionを使って遷移
        SceneTransition.LoadScene("Main");
    }
}
