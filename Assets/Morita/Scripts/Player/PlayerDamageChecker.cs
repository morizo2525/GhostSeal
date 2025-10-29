using UnityEngine;

public class PlayerDamageChecker : MonoBehaviour
{
    [Header("敵Layer")]
    [SerializeField] private LayerMask enemyLayer;

    [Header("ダメージ量")]
    [SerializeField] private int damage = 1;

    [Header("連続ダメージ防止")]
    [SerializeField] private float invincibleTime = 0.5f;

    [Header("Box判定")]
    [SerializeField] private Vector2 boxSize = new Vector2(1f, 1f);
    [SerializeField] private Vector2 boxOffset = Vector2.zero;

    [Header("上下Circle判定")]
    [SerializeField] private float circleRadius = 0.5f;
    [SerializeField] private float circleYOffset = 0.5f;

    [Header("プレイヤー移動スクリプト")]
    [SerializeField] private PlayerMove playerMove;

    private PlayerHPManager playerHPManager;
    private PlayerBlink     playerBlink;
    private float invincibleTimer = 0f;
    private bool  isTouchingEnemy = false;

    private void Start()
    {
        playerHPManager = GetComponent<PlayerHPManager>();
        playerBlink     = GetComponent<PlayerBlink>();
        if (playerMove == null) playerMove = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        // プレイヤーが死亡していたら判定をやめる
        if (playerHPManager.IsDead) return;

        // 無敵タイマーを減らす
        if (invincibleTimer > 0f) invincibleTimer -= Time.deltaTime;

        // プレイヤーの位置と向きの取得
        Vector2 pos = (Vector2)transform.position;
        float horizontalSign = 1f;
        if (playerMove != null) horizontalSign = Mathf.Sign(playerMove.transform.localScale.x);

        // 判定位置の計算
        Vector2 currentBoxOffset = new Vector2(boxOffset.x * horizontalSign, boxOffset.y);
        Vector2 topCirclePos = pos + currentBoxOffset + Vector2.up * circleYOffset;
        Vector2 bottomCirclePos = pos + currentBoxOffset + Vector2.down * circleYOffset;

        // 敵との接触判定
        bool hitDetected = Physics2D.OverlapBox(pos + currentBoxOffset, boxSize, 0f, enemyLayer);
        hitDetected |= Physics2D.OverlapCircle(topCirclePos, circleRadius, enemyLayer);
        hitDetected |= Physics2D.OverlapCircle(bottomCirclePos, circleRadius, enemyLayer);

        // 接触判定とダメージ処理
        if (hitDetected)
        {
            if (!isTouchingEnemy) 
            { 
                Debug.Log("敵と接触開始！"); 
                isTouchingEnemy = true; 
            }

            // ダメージ処理
            if ((playerMove == null || !playerMove.isInvincible) && invincibleTimer <= 0f)
            {
                playerHPManager.TakeDamage(damage);
                invincibleTimer = invincibleTime;

                // 点滅開始
                if (playerBlink != null)
                {
                    playerBlink.StartBlink(invincibleTime);
                }

                Debug.Log("→ ダメージ発生！ 残りHP更新");
            }
        }
        else
        {
            if (isTouchingEnemy) 
            { 
                Debug.Log("敵との接触が解除された。"); 
                isTouchingEnemy = false; 
            }
        }
    }


#if UNITY_EDITOR
    // Gizmosで判定範囲を可視化
    private void OnDrawGizmosSelected()
    {
        Vector2 pos = (Vector2)transform.position;
        float horizontalSign = 1f;
        if (playerMove != null) horizontalSign = Mathf.Sign(playerMove.transform.localScale.x);

        Vector2 currentBoxOffset = new Vector2(boxOffset.x * horizontalSign, boxOffset.y);
        Vector2 topCirclePos = pos + currentBoxOffset + Vector2.up * circleYOffset;
        Vector2 bottomCirclePos = pos + currentBoxOffset + Vector2.down * circleYOffset;

        Gizmos.color = isTouchingEnemy ? Color.red : Color.green;
        Gizmos.DrawWireCube(pos + currentBoxOffset, boxSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(topCirclePos, circleRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(bottomCirclePos, circleRadius);
    }
#endif
}
