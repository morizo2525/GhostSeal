using UnityEngine;

public class ArrowTest : MonoBehaviour
{
    private PlayerAttack_Bow arrowShooter;

    void Start()
    {
        // 同じGameObjectにアタッチされたArrowShooterを取得
        arrowShooter = GetComponent<PlayerAttack_Bow>();

        if (arrowShooter == null)
        {
            Debug.LogError("ArrowShooterが見つかりません！同じGameObjectにArrowShooterをアタッチしてください。");
        }
    }

    void Update()
    {
        // 右クリック（マウスボタン1）で発射
        if (Input.GetMouseButtonDown(1))
        {
            if (arrowShooter != null)
            {
                arrowShooter.BowShoot();
            }
        }
    }
}
