using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]

public class PlayerBlink : MonoBehaviour
{
    [SerializeField] private float blinkInterval = 0.1f; // 点滅間隔
    private SpriteRenderer spriteRenderer;
    private Coroutine blinkCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 点滅を開始するメソッド
    public void StartBlink(float duration)
    {
        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);

        blinkCoroutine = StartCoroutine(BlinkRoutine(duration));
    }

    // 点滅処理のコルーチン
    private IEnumerator BlinkRoutine(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        spriteRenderer.enabled = true; // 最後は表示状態に戻す
        blinkCoroutine = null;
    }
}
