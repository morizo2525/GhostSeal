using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("チュートリアルUI")]
    [SerializeField] private GameObject tutorialPanel;   // チュートリアル全体のパネル
    [SerializeField] private Button openTutorialButton;  // タイトル画面のチュートリアルボタン
    [SerializeField] private Button closeTutorialButton; // ×ボタン

    [Header("チュートリアル画像")]
    [SerializeField] private Image tutorialImage;        // チュートリアル画像
    [SerializeField] private Sprite[] tutorialSprites;   // 表示する画像の配列

    [Header("ナビゲーションボタン")]
    [SerializeField] private Button nextButton;          // 右ボタン（次へ）
    [SerializeField] private Button prevButton;          // 左ボタン（前へ）

    private int currentIndex = 0; // 現在表示中の画像インデックス

    private void Start()
    {
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
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(ShowNextImage);
        }
        if (prevButton != null)
        {
            prevButton.onClick.AddListener(ShowPrevImage);
        }

        // 初期画像を設定
        UpdateTutorialImage();
    }

    // チュートリアルを表示
    private void OpenTutorial()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
            currentIndex = 0; // 最初の画像にリセット
            UpdateTutorialImage();
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

    // 次の画像を表示
    private void ShowNextImage()
    {
        if (tutorialSprites != null && tutorialSprites.Length > 0)
        {
            currentIndex = (currentIndex + 1) % tutorialSprites.Length;
            UpdateTutorialImage();
        }
    }

    // 前の画像を表示
    private void ShowPrevImage()
    {
        if (tutorialSprites != null && tutorialSprites.Length > 0)
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = tutorialSprites.Length - 1;
            }
            UpdateTutorialImage();
        }
    }

    // 画像とボタンの状態を更新
    private void UpdateTutorialImage()
    {
        if (tutorialImage != null && tutorialSprites != null && tutorialSprites.Length > 0)
        {
            tutorialImage.sprite = tutorialSprites[currentIndex];
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
        if (nextButton != null)
        {
            nextButton.onClick.RemoveListener(ShowNextImage);
        }
        if (prevButton != null)
        {
            prevButton.onClick.RemoveListener(ShowPrevImage);
        }
    }
}
