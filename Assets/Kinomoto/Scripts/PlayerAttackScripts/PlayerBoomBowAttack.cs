using UnityEngine;

public class PlayerBoomBowAttack : MonoBehaviour
{
    [Header("爆弾矢の設定")]
    public GameObject boomArrowPrefab;   // 爆発する矢のPrefab（通常の矢に爆発機能を追加したもの）
    public GameObject explosionEffect;   // 爆発エフェクト
    public float explosionRadius = 4f;   // 爆発範囲
    public int explosionDamage = 40;     // 爆発ダメージ
    public LayerMask enemyLayer;         // 敵レイヤー

    [Header("参照")]
    public PlayerAttack_Bow bowAttackScript;  // 弓スクリプトの参照

    /// <summary>
    /// 爆発する矢を放つ
    /// </summary>
    public void ShootBoomArrow()
    {
        if (bowAttackScript == null)
        {
            Debug.LogError("PlayerAttack_Bowスクリプトが設定されていません！");
            return;
        }

        // 元の矢Prefabを保存
        GameObject originalArrow = bowAttackScript.arrowPrefab;

        // 一時的に爆発矢Prefabをセット
        bowAttackScript.arrowPrefab = boomArrowPrefab;

        // 発射処理を再利用
        bowAttackScript.BowShoot();

        // 元に戻す
        bowAttackScript.arrowPrefab = originalArrow;
    }
}