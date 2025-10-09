using UnityEngine;

public class EnemyHPManager : MonoBehaviour
{
    //敵のHPを管理するスクリプト

    public int maxHealth = 100;
    private int enemyHealth;

    void Start()
    {
        enemyHealth = maxHealth;
    }

    public void EnemyTakeDamage(int damage)
    {
        enemyHealth -= damage;
        //デバッグ用
        Debug.Log("Enemy Health: " + enemyHealth);
        if (enemyHealth <= 0)
        {
            EnemyDie();
        }
    }

    void EnemyDie()
    {
        Destroy(gameObject);
    }
}
