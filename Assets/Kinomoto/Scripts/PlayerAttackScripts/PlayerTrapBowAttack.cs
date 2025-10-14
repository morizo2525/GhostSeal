using UnityEngine;

public class PlayerTrapBowAttack : MonoBehaviour
{
    [Header("矢の設定")]
    [SerializeField] private GameObject trapArrowPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootPower = 10f;

    [Header("トラップ矢クールタイム")]
    [SerializeField] private float trapArrowCooldown = 3f;
    private float lastTrapArrowTime = -999f;

    /// <summary>
    /// トラップ矢を発射
    /// </summary>
    public void BowShootTrapArrow()
    {
        if (Time.time - lastTrapArrowTime < trapArrowCooldown)
        {
            Debug.Log($"トラップ矢のクールタイム中です。残り時間: {trapArrowCooldown - (Time.time - lastTrapArrowTime):F1}秒");
            return;
        }

        if (trapArrowPrefab == null)
        {
            Debug.LogError("トラップ矢のPrefabが設定されていません");
            return;
        }

        ShootArrow(trapArrowPrefab);
        lastTrapArrowTime = Time.time;
        Debug.Log("トラップ矢を発射しました");
    }

    /// <summary>
    /// 矢を発射する内部メソッド
    /// </summary>
    private void ShootArrow(GameObject arrowToUse)
    {
        if (shootPoint == null)
        {
            Debug.LogError("発射位置が設定されていません");
            return;
        }

        // マウス位置取得（ワールド座標）
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // 発射方向ベクトル（射角制限なし）
        Vector2 dir = (mouseWorld - shootPoint.position).normalized;

        // 矢を生成
        GameObject arrow = Instantiate(arrowToUse, shootPoint.position, Quaternion.identity);

        // 矢の向きを設定
        arrow.transform.right = dir;

        // Rigidbody2Dで物理発射
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = dir * shootPower;
        }
    }
}