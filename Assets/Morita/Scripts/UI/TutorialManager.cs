using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("チュートリアルUI")]
    [SerializeField] private GameObject tutorialPanel;   // チュートリアル全体のパネル
    [SerializeField] private Button openTutorialButton;  // タイトル画面の「チュートリアル」ボタン
    [SerializeField] private Button closeTutorialButton; // ×ボタン

    [Header("チュートリアル画像")]
    [SerializeField] private Image tutorialImage;   // チュートリアル画像
    [SerializeField] private Sprite tutorialSprite; // 表示する画像

    private void Start()
    {
        // 初期設定
        if (tutorialImage != null && tutorialSprite != null)
        {
            tutorialImage.sprite = tutorialSprite;
        }

        // チュートリアルパネルを非表示にする
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }

        // ボタンのクリックイベントを設定
        if (openTutorialButton != null)
        {
            openTutorialButton.onClick.AddListener(OpenTutorial);
        }

        if (closeTutorialButton != null)
        {
            closeTutorialButton.onClick.AddListener(CloseTutorial);
        }
    }

    // チュートリアルを表示
    private void OpenTutorial()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
        }
    }

    // チュートリアルを非表示
    private void CloseTutorial()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        // メモリリーク防止のためリスナーを削除
        if (openTutorialButton != null)
        {
            openTutorialButton.onClick.RemoveListener(OpenTutorial);
        }

        if (closeTutorialButton != null)
        {
            closeTutorialButton.onClick.RemoveListener(CloseTutorial);
        }
    }
}
