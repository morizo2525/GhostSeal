using UnityEngine;

public class EnemyHPManager : MonoBehaviour
{
    //�G��HP���Ǘ�����X�N���v�g

    public int maxHealth = 100;
    private int enemyHealth;

    void Start()
    {
        enemyHealth = maxHealth;
    }

    public void EnemyTakeDamage(int damage)
    {
        enemyHealth -= damage;
        //�f�o�b�O�p
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
