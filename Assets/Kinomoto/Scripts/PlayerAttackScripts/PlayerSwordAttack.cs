using UnityEngine;

public class PlayerSwordAttack : MonoBehaviour
{
    //プレイヤーの剣攻撃を管理するスクリプト
    //攻撃のアニメーション、攻撃判定、エフェクトの生成など
    //攻撃の判定はプレイヤーが向いている前方に生成される

    public Animator animator;           //攻撃のアニメ―ション
    public float attackRange = 1.0f;    //攻撃範囲
    public LayerMask enemyLayer;        //敵のレイヤー
    public GameObject attackEffect;     //攻撃エフェクト
    public int swordDamage = 20;        //剣のダメージ量

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SwordAttack(); //剣攻撃メソッド
        }
    }

    public void SwordAttack()
    {
        // プレイヤーの向きを判定（localScale.xが負なら左向き、正なら右向き）
        float direction = transform.localScale.x < 0 ? -1 : 1;

        //攻撃アニメーション再生
        //animator.SetTrigger("Attack");

        //プレイヤーの前方に攻撃判定を生成
        Vector2 attackPos = (Vector2)transform.position + new Vector2(direction * attackRange, 0);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPos, 0.5f, enemyLayer);

        //攻撃エフェクトの生成
        if (attackEffect != null)
        {
            //Instantiate(attackEffect, attackPos, Quaternion.identity);
        }

        //敵にダメージを与える
        foreach (Collider2D enemy in hitEnemies)
        {
            //敵のダメージ処理
            EnemyHPManager enemyHP = enemy.GetComponent<EnemyHPManager>();
            if (enemyHP != null)
            {
                enemyHP.EnemyTakeDamage(swordDamage);
                Debug.Log($"剣で敵を攻撃！ {swordDamage}ダメージ");
            }
        }

        // デバッグ用：攻撃位置を可視化
        Debug.DrawLine(transform.position, attackPos, Color.red, 0.5f);
    }

    // Gizmoで攻撃範囲を可視化（Scene Viewで確認用）
    private void OnDrawGizmosSelected()
    {
        if (transform != null)
        {
            float direction = transform.localScale.x < 0 ? -1 : 1;
            Vector2 attackPos = (Vector2)transform.position + new Vector2(direction * attackRange, 0);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPos, 0.5f);
        }
    }
}