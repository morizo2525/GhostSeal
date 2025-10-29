using UnityEngine;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] private Button startButton;   // UnityのInspectorでボタンをアサイン
    [SerializeField] private string gameSceneName = "Main";  // 遷移先のシーン名
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite newBackgroundSprite;

    [Header("サウンド設定")]
    [SerializeField] private AudioClip buttonClickSE;  // ボタンクリック時のSE
    [Range(0f, 1f)]
    [SerializeField] private float seVolume = 1.0f;    // SEの音量
    private AudioSource audioSource;

    private void Start()
    {
        // AudioSourceコンポーネントを取得または追加
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ボタンが押されたときの処理を登録
        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        // SEを再生
        if (buttonClickSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSE, seVolume);
        }

        // 背景画像を切り替え
        if (backgroundImage != null && newBackgroundSprite != null)
        {
            backgroundImage.sprite = newBackgroundSprite;
        }

        // SEの長さ分待ってからシーン遷移（最低0.1秒）
        float delay = buttonClickSE != null ? Mathf.Max(buttonClickSE.length, 0.1f) : 0f;

        // SceneTransitionを使って遷移
        SceneTransition.LoadScene(gameSceneName, delay);
    }
}