using UnityEngine;

public class PlayerBombAttack : MonoBehaviour
{
    //プレイヤーの爆弾攻撃を管理するスクリプト
    //爆弾が爆発する部分はメソッドで作成し、後の組み合わせで再利用できるようにする
    //また、bombを投げる動作は別のメソッドとほぼ同じなので、PlayerAttack_BowのBowShoot()を再利用する

    [Header("爆弾の設定")]
    public GameObject bombPrefab;           // 投げる爆弾のPrefab

    [Header("投擲力の設定")]
    [Range(0.1f, 2.0f)]
    public float throwPowerMultiplier = 0.6f; // 投擲力の倍率（1.0 = 矢と同じ）

    [Header("参照")]
    public PlayerAttack_Bow bowAttackScript; // 弓攻撃スクリプトへの参照

    public void ThrowBomb()
    {
        if (bowAttackScript == null)
        {
            Debug.LogError("PlayerAttack_Bowスクリプトが設定されていません！");
            return;
        }

        // 元の矢のPrefabを一時保存
        GameObject originalArrow = bowAttackScript.arrowPrefab;
        float originalPower = bowAttackScript.shootPower;

        // 矢のPrefabを爆弾に差し替え
        bowAttackScript.arrowPrefab = bombPrefab;

        // 発射力を調整（爆弾は飛びにくくする）
        bowAttackScript.shootPower = originalPower * throwPowerMultiplier;

        // BowShoot()を実行（爆弾が発射される）
        bowAttackScript.BowShoot();

        // 矢のPrefabを元に戻す
        bowAttackScript.arrowPrefab = originalArrow;
        bowAttackScript.shootPower = originalPower;
    }
}
