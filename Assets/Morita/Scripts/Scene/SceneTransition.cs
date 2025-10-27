using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    private static SceneTransition instance;

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float delayBeforeFade = 0f;

    private bool isTransitioning = false;

    private void Awake()
    {
        // シングルトンパターンでシーン間で保持
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // 起動時にフェードイン
            StartCoroutine(FadeIn());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// シーン遷移を実行（フェードアウト → シーンロード → フェードイン）
    /// </summary>
    /// <param name="sceneName">遷移先のシーン名</param>
    /// <param name="delay">フェード開始前の待機時間（秒）</param>
    public static void LoadScene(string sceneName, float delay = -1f)
    {
        if (instance != null && !instance.isTransitioning)
        {
            float actualDelay = delay >= 0f ? delay : instance.delayBeforeFade;
            instance.StartCoroutine(instance.TransitionToScene(sceneName, actualDelay));
        }
    }

    private IEnumerator TransitionToScene(string sceneName, float delay)
    {
        isTransitioning = true;

        // ディレイ
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        // フェードアウト
        yield return StartCoroutine(FadeOut());

        // シーンロード
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // フェードイン
        yield return StartCoroutine(FadeIn());

        isTransitioning = false;
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, 1f);
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, 0f);
    }

    /// <summary>
    /// フェード時間を変更
    /// </summary>
    public static void SetFadeDuration(float duration)
    {
        if (instance != null)
        {
            instance.fadeDuration = duration;
        }
    }

    /// <summary>
    /// フェード色を変更
    /// </summary>
    public static void SetFadeColor(Color color)
    {
        if (instance != null && instance.fadeImage != null)
        {
            Color currentColor = instance.fadeImage.color;
            instance.fadeImage.color = new Color(color.r, color.g, color.b, currentColor.a);
        }
    }

    /// <summary>
    /// ディレイ時間を変更
    /// </summary>
    public static void SetDelay(float delay)
    {
        if (instance != null)
        {
            instance.delayBeforeFade = delay;
        }
    }
}
