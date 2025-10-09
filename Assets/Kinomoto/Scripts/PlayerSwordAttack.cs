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

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            SwordAttack(); //剣攻撃メソッド
        }
    }

    public void SwordAttack()
    {
        //攻撃アニメーション再生
        animator.SetTrigger("Attack");

        //プレイヤーの前方に攻撃判定を生成
        Vector2 attackPos = (Vector2)transform.position + (Vector2)(transform.right * attackRange);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPos, 0.5f, enemyLayer);

        //攻撃エフェクトの生成
        if (attackEffect != null)
        {
            Instantiate(attackEffect, attackPos, Quaternion.identity);
        }

        //敵にダメージを与える
        foreach (Collider2D enemy in hitEnemies)
        {
            //敵のダメージ処理
        }
    }
}
