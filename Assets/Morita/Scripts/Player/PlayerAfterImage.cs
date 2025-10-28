using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImage : MonoBehaviour
{
    [Header("残像設定")]
    [SerializeField] private float spawnInterval = 0.05f;   // 残像生成間隔
    [SerializeField] private float fadeOutTime = 0.5f;      // 残像が消えるまでの時間
    [SerializeField] private Color afterImageColor = new Color(1f, 1f, 1f, 0.7f); // 残像の初期色と透明度

    [Header("オブジェクトプーリング")]
    [SerializeField] private int poolSize = 10; // プール内の残像数

    private Queue<GameObject> afterImagePool = new Queue<GameObject>();
    private float spawnTimer = 0f;
    private bool isActive = false;
    private SpriteRenderer playerSpriteRenderer;

    void Start()
    {
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        InitializePool();
    }

    void Update()
    {
        if (!isActive) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnAfterImage();
            spawnTimer = 0f;
        }
    }

    // 残像エフェクトを開始
    public void StartAfterImage()
    {
        isActive = true;
        spawnTimer = 0f;
    }

    // 残像エフェクトを停止
    public void StopAfterImage()
    {
        isActive = false;
    }

    // オブジェクトプールを初期化
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject afterImage = CreateAfterImageObject();
            afterImage.SetActive(false);
            afterImagePool.Enqueue(afterImage);
        }
    }

    // 残像オブジェクトを生成
    private GameObject CreateAfterImageObject()
    {
        GameObject obj = new GameObject("AfterImage");
        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();

        // プレイヤーと同じソートレイヤーで、少し後ろに表示
        sr.sortingLayerName = playerSpriteRenderer.sortingLayerName;
        sr.sortingOrder = playerSpriteRenderer.sortingOrder - 1;

        AfterImageFade fade = obj.AddComponent<AfterImageFade>();
        fade.fadeOutTime = fadeOutTime;
        fade.pool = afterImagePool;

        return obj;
    }

    // 残像を配置(現在のアニメーションフレームをコピー)
    private void SpawnAfterImage()
    {
        if (playerSpriteRenderer == null || playerSpriteRenderer.sprite == null)
            return;

        GameObject afterImage;

        if (afterImagePool.Count > 0)
        {
            afterImage = afterImagePool.Dequeue();
        }
        else
        {
            afterImage = CreateAfterImageObject();
        }

        // 位置・向き・スケールを完全にコピー
        afterImage.transform.position = transform.position;
        afterImage.transform.rotation = transform.rotation;
        afterImage.transform.localScale = transform.localScale;

        SpriteRenderer sr = afterImage.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            // 現在のアニメーションフレーム(スプライト)をコピー
            sr.sprite = playerSpriteRenderer.sprite;
            sr.color = afterImageColor;
            sr.flipX = playerSpriteRenderer.flipX;
            sr.flipY = playerSpriteRenderer.flipY;

            // マテリアルもコピー(シェーダーエフェクトがある場合)
            sr.material = playerSpriteRenderer.material;
        }

        afterImage.SetActive(true);
        afterImage.GetComponent<AfterImageFade>().StartFade();
    }
}

// 残像のフェードアウト処理
public class AfterImageFade : MonoBehaviour
{
    [HideInInspector] public float fadeOutTime = 0.5f;
    [HideInInspector] public Queue<GameObject> pool;

    private SpriteRenderer sr;
    private float fadeTimer;
    private Color startColor;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void StartFade()
    {
        fadeTimer = 0f;
        startColor = sr.color;
    }

    void Update()
    {
        fadeTimer += Time.deltaTime;
        float alpha = Mathf.Lerp(startColor.a, 0f, fadeTimer / fadeOutTime);
        sr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        if (fadeTimer >= fadeOutTime)
        {
            gameObject.SetActive(false);
            if (pool != null)
            {
                pool.Enqueue(gameObject);
            }
        }
    }
}
