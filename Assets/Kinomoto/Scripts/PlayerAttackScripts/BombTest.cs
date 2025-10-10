using UnityEngine;

public class BombTest : MonoBehaviour
{
    [Header("テスト対象")]
    public PlayerBombAttack bombAttack;     // 爆弾攻撃スクリプト
    public PlayerAttack_Bow bowAttack;      // 弓攻撃スクリプト
    public PlayerBoomBowAttack boomBowAttack;   // 爆発矢攻撃スクリプト

    [Header("テスト設定")]
    public KeyCode bombKey = KeyCode.B;     // 爆弾投擲キー
    public KeyCode arrowKey = KeyCode.A;    // 矢発射キー
    public KeyCode boomBowKey = KeyCode.X;      // 爆発矢発射キー

    [Header("デバッグ情報")]
    public bool showDebugInfo = true;       // デバッグ情報を表示

    void Update()
    {
        // 爆弾投擲テスト
        if (Input.GetKeyDown(bombKey))
        {
            TestBombThrow();
        }

        // 矢発射テスト（比較用）
        if (Input.GetKeyDown(arrowKey))
        {
            TestArrowShoot();
        }

        // 爆発矢発射テスト
        if (Input.GetKeyDown(boomBowKey))
        {
            TestBoomBowShoot();
        }
    }

    void TestBombThrow()
    {
        if (bombAttack == null)
        {
            Debug.LogError("PlayerBombAttackが設定されていません！");
            return;
        }

        if (showDebugInfo)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log($"[爆弾テスト] マウス位置: {mousePos}, 時刻: {Time.time:F2}秒");
        }

        bombAttack.ThrowBomb();
    }

    void TestArrowShoot()
    {
        if (bowAttack == null)
        {
            Debug.LogWarning("PlayerAttack_Bowが設定されていません");
            return;
        }

        if (showDebugInfo)
        {
            Debug.Log($"[矢テスト] 矢を発射, 時刻: {Time.time:F2}秒");
        }

        bowAttack.BowShoot();
    }

    void TestBoomBowShoot()
    {
        if (boomBowAttack == null)
        {
            Debug.LogError("PlayerBoomBowAttackが設定されていません！");
            return;
        }

        if (showDebugInfo)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log($"[爆発矢テスト] マウス位置: {mousePos}, 時刻: {Time.time:F2}秒");
        }

        boomBowAttack.ShootBoomArrow();
    }
}