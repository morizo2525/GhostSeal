using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [Header("Fade Settings")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private bool isTransitioning = false;

    private void Awake()
    {
        // シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // CanvasGroupが設定されていない場合は作成
        if (fadeCanvasGroup == null)
        {
            CreateFadeCanvas();
        }
    }

    private void Start()
    {
        // 開始時はフェードアウトしてゲーム画面を表示
        StartCoroutine(FadeOut());
    }

    /// <summary>
    /// シーン名を指定して遷移
    /// </summary>
    public void LoadScene(string sceneName)
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionToScene(sceneName));
        }
    }

    /// <summary>
    /// シーンインデックスを指定して遷移
    /// </summary>
    public void LoadScene(int sceneIndex)
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionToScene(sceneIndex));
        }
    }

    /// <summary>
    /// 現在のシーンをリロード
    /// </summary>
    public void ReloadCurrentScene()
    {
        if (!isTransitioning)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine(TransitionToScene(currentSceneIndex));
        }
    }

    private IEnumerator TransitionToScene(string sceneName)
    {
        isTransitioning = true;

        // フェードイン(画面を暗くする)
        yield return StartCoroutine(FadeIn());

        // シーンをロード
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // フェードアウト(画面を明るくする)
        yield return StartCoroutine(FadeOut());

        isTransitioning = false;
    }

    private IEnumerator TransitionToScene(int sceneIndex)
    {
        isTransitioning = true;

        // フェードイン(画面を暗くする)
        yield return StartCoroutine(FadeIn());

        // シーンをロード
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // フェードアウト(画面を明るくする)
        yield return StartCoroutine(FadeOut());

        isTransitioning = false;
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 0f;
    }

    private void CreateFadeCanvas()
    {
        // フェード用のCanvasを作成
        GameObject canvasObj = new GameObject("FadeCanvas");
        canvasObj.transform.SetParent(transform);

        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999; // 最前面に表示

        canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        // 黒い背景パネルを作成
        GameObject panelObj = new GameObject("FadePanel");
        panelObj.transform.SetParent(canvasObj.transform);

        UnityEngine.UI.Image image = panelObj.AddComponent<UnityEngine.UI.Image>();
        image.color = Color.black;

        RectTransform rectTransform = panelObj.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;

        // CanvasGroupを追加
        fadeCanvasGroup = panelObj.AddComponent<CanvasGroup>();
        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.blocksRaycasts = false;
    }
}