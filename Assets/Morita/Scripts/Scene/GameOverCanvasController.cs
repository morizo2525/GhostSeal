using UnityEngine;
using System.Collections;

public class GameOverCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private CanvasGroup canvasGroup;     // フェード制御用
    [SerializeField] private float fadeInDuration = 1.5f; // フェードイン時間

    private void Awake()
    {
        // GameOver Canvasが未設定の場合、自動検索
        if (gameOverCanvas == null)
        {
            gameOverCanvas = GameObject.Find("GameOverCanvas");
            if (gameOverCanvas == null)
            {
                Debug.LogError("GameOverCanvasが見つかりません。シーンに配置してください。");
            }
        }

        // CanvasGroupが未設定の場合、自動取得または追加
        if (canvasGroup == null && gameOverCanvas != null)
        {
            canvasGroup = gameOverCanvas.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameOverCanvas.AddComponent<CanvasGroup>();
            }
        }

        // 初期状態で非表示
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
    }

   
    // ゲームオーバースプライトをフェードイン表示
    public void ShowGameOver()
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
            StartCoroutine(FadeIn());
        }
    }

    // フェードイン処理
    private IEnumerator FadeIn()
    {
        if (canvasGroup == null) yield break;

        float elapsed = 0f;
        canvasGroup.alpha = 0f;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeInDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}
