using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class AirEnemyMove : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 4f;

    [Header("揺れ設定")]
    [SerializeField] private float waveAmplitude = 1f;  //揺れ幅(上下の振れ幅)
    [SerializeField] private float waveFrequency = 2f;  //揺れスピード(周波数)

    private Rigidbody2D rb;
    private Transform player;
    private float waveTimer;  //揺れのタイマー

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

        // 揺れのタイマーを更新
        waveTimer += Time.fixedDeltaTime;

        // プレイヤーへの方向ベクトルを計算
        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        // 進行方向に対して垂直なベクトルを計算(上下の揺れ用)
        Vector2 perpendicular = new Vector2(-directionToPlayer.y, directionToPlayer.x);

        // サイン波で滑らかな揺れを生成
        float waveOffset = Mathf.Sin(waveTimer * waveFrequency) * waveAmplitude;

        // 基本の移動方向 + 揺れ
        Vector2 finalDirection = directionToPlayer + perpendicular * waveOffset;

        // プレイヤーに向かって揺れながら移動
        rb.linearVelocity = finalDirection.normalized * moveSpeed;
    }
}
