using UnityEngine;

public class TrapTest : MonoBehaviour
{
    [SerializeField] private PlayerTrapCreator trapCreator;
    [SerializeField] private PlayerTrapBowAttack bowWithTrap;
    [SerializeField] private BoomTrap boomTrap;
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // トラップ矢を発射
            if (bowWithTrap != null)
            {
                bowWithTrap.BowShootTrapArrow();
            }
            else
            {
                Debug.LogError("PlayerAttack_BowWithTrapが設定されていません");
            }
        }

        //地雷トラップを生成
        if (Input.GetKeyDown(KeyCode.G))
        {
            // 足元に地雷トラップを生成
            if (trapCreator != null)
            {
                trapCreator.CreateMineTrap();
            }
            else
            {
                Debug.LogError("PlayerTrapCreatorが設定されていません");
            }
        }

        //確認したいときにコメントを外す
        if (Input.GetKeyDown(KeyCode.T))
        {
            // 足元にトラップを生成
            if (trapCreator != null)
            {
                trapCreator.CreateTrap();
            }
            else
            {
                Debug.LogError("PlayerTrapCreatorが設定されていません");
            }
        }
    }
}