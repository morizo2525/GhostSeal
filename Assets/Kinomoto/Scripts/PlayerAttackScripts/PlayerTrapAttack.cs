using UnityEngine;

public class PlayerTrapAttack : MonoBehaviour
{
    //トラップ用のスクリプト
    //トラップを投げた後、地面に触れた場所へオブジェクトが設置される
    //設置後、一定時間経過で消える
    //トラップに触れた敵を1体拘束する

    [SerializeField] private float trapDuration = 10f;           // トラップが消えるまでの時間
    [SerializeField] private float enemyStunDuration = 3f;       // 敵が拘束される時間
    [SerializeField] private string enemyTag = "Enemy";          // 敵のタグ
    [SerializeField] private Collider2D trapCollider;            // トラップのコライダー

    private float elapsedTime = 0f;
    private bool hasTriggered = false;                           // 既に敵を捕捉したか
    private GameObject capturedEnemy;                            // 現在捕捉中の敵
    private GameObject capturedAirEnemy;                         // 空中の捕捉中の敵

    private void Start()
    {
        if (trapCollider == null)
            trapCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        // トラップの経過時間を管理
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= trapDuration)
        {
            DestroyTrap();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 敵のタグをチェック
        if (collision.CompareTag(enemyTag) && !hasTriggered)
        {
            CaptureEnemy(collision.gameObject);
        }
    }

    /// <summary>
    /// 敵を拘束するメソッド
    /// 他の機能からも呼び出し可能
    /// </summary>
    public void CaptureEnemy(GameObject enemy)
    {
        if (enemy == null)
            return;

        hasTriggered = true;
        capturedEnemy = enemy;
        capturedAirEnemy = enemy;


        // 敵のRigidbody2Dを取得して動きを止める
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
        }

        // 敵のスクリプトから制御を奪う（例：移動スクリプトの無効化）
        GroundEnemyMove enemyController = enemy.GetComponent<GroundEnemyMove>();
        AirEnemyMove airEnemyConreoller = enemy.GetComponent<AirEnemyMove>();
        if (enemyController != null)
        {
            enemyController.enabled = false;
        }
        if (airEnemyConreoller != null)
        {
            airEnemyConreoller.enabled = false;
        }

        // 拘束時間経過後に敵を解放
        Invoke(nameof(ReleaseEnemy), enemyStunDuration);

        //トラップも削除
        Invoke(nameof(DestroyTrap), enemyStunDuration);
    }

    /// <summary>
    /// 拘束された敵を解放するメソッド
    /// </summary>
    private void ReleaseEnemy()
    {
        if (capturedEnemy == null)
            return;

        // 敵のRigidbody2Dを動かせるようにする
        Rigidbody2D rb = capturedEnemy.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        // 敵のスクリプトを再度有効にする
        GroundEnemyMove enemyController = capturedEnemy.GetComponent<GroundEnemyMove>();
        AirEnemyMove airEnemyMove = capturedEnemy.GetComponent<AirEnemyMove>();
        if (enemyController != null)
        {
            enemyController.enabled = true;
        }
        if (airEnemyMove != null)
        {
            airEnemyMove.enabled = true;
        }

        capturedEnemy = null;
        hasTriggered = false;
    }

    /// <summary>
    /// トラップを消滅させるメソッド
    /// </summary>
    private void DestroyTrap()
    {
        // 拘束中の敵がいれば解放
        if (capturedEnemy != null)
        {
            ReleaseEnemy();
        }

        Destroy(gameObject);
    }
}