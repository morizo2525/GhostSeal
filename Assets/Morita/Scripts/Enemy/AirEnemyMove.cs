using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class AirEnemyMove : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 4f;

    private Rigidbody2D rb;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 重力の影響を受けないように設定
        rb.gravityScale = 0f;

        // プレイヤーを検索
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Playerタグのオブジェクトが見つかりません");
        }
    }

    void FixedUpdate()
    {
        // プレイヤーが存在しない場合は移動しない
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // プレイヤーへの方向ベクトルを計算
        Vector2 direction = (player.position - transform.position).normalized;

        // プレイヤーに向かって移動
        rb.linearVelocity = direction * moveSpeed;
    }
}
