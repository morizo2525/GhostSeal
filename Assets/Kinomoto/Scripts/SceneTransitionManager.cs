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
        // �V���O���g���̐ݒ�
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

        // CanvasGroup���ݒ肳��Ă��Ȃ��ꍇ�͍쐬
        if (fadeCanvasGroup == null)
        {
            CreateFadeCanvas();
        }
    }

    private void Start()
    {
        // �J�n���̓t�F�[�h�A�E�g���ăQ�[����ʂ�\��
        StartCoroutine(FadeOut());
    }

    /// <summary>
    /// �V�[�������w�肵�đJ��
    /// </summary>
    public void LoadScene(string sceneName)
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionToScene(sceneName));
        }
    }

    /// <summary>
    /// �V�[���C���f�b�N�X���w�肵�đJ��
    /// </summary>
    public void LoadScene(int sceneIndex)
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionToScene(sceneIndex));
        }
    }

    /// <summary>
    /// ���݂̃V�[���������[�h
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

        // �t�F�[�h�C��(��ʂ��Â�����)
        yield return StartCoroutine(FadeIn());

        // �V�[�������[�h
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // �t�F�[�h�A�E�g(��ʂ𖾂邭����)
        yield return StartCoroutine(FadeOut());

        isTransitioning = false;
    }

    private IEnumerator TransitionToScene(int sceneIndex)
    {
        isTransitioning = true;

        // �t�F�[�h�C��(��ʂ��Â�����)
        yield return StartCoroutine(FadeIn());

        // �V�[�������[�h
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // �t�F�[�h�A�E�g(��ʂ𖾂邭����)
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
        // �t�F�[�h�p��Canvas���쐬
        GameObject canvasObj = new GameObject("FadeCanvas");
        canvasObj.transform.SetParent(transform);

        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999; // �őO�ʂɕ\��

        canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        // �����w�i�p�l�����쐬
        GameObject panelObj = new GameObject("FadePanel");
        panelObj.transform.SetParent(canvasObj.transform);

        UnityEngine.UI.Image image = panelObj.AddComponent<UnityEngine.UI.Image>();
        image.color = Color.black;

        RectTransform rectTransform = panelObj.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;

        // CanvasGroup��ǉ�
        fadeCanvasGroup = panelObj.AddComponent<CanvasGroup>();
        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.blocksRaycasts = false;
    }
}