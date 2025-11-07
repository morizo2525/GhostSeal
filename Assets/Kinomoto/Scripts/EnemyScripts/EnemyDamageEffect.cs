using UnityEngine;
using System.Collections;

public class EnemyDamageEffect : MonoBehaviour
{
    [Header("Flash Settings")]
    [Tooltip("点滅時の色")]
    public Color flashColor = Color.red;

    [Tooltip("点滅の持続時間（秒）")]
    public float flashDuration = 0.1f;

    [Tooltip("点滅の回数")]
    public int flashCount = 2;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isFlashing = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogWarning("SpriteRendererが見つかりません。子オブジェクトから検索します。");
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color;
            }
            else
            {
                Debug.LogError("SpriteRendererが見つかりませんでした。");
            }
        }
    }

    // ダメージを受けたときに呼び出すメソッド
    public void Flash()
    {
        if (spriteRenderer == null || isFlashing) return;

        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        isFlashing = true;

        for (int i = 0; i < flashCount; i++)
        {
            // 赤色に変更
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);

            // 元の色に戻す
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }

        isFlashing = false;
    }

    // 元の色を強制的にリセット（必要に応じて使用）
    public void ResetColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }
}