using UnityEngine;

public class PlayerTrapCreator : MonoBehaviour
{
    [Header("トラバサミトラップ（罠単体攻撃）の設定")]
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private float trapCooldown = 2f;

    [Header("地雷トラップの設定")]
    [SerializeField] private GameObject mineTrapPrefab;
    [SerializeField] private float mineTrapCooldown = 5f;

    private float lastTrapTime;

    /// <summary>
    /// プレイヤーの足元にトラバサミトラップを生成
    /// </summary>
    public void CreateTrap()
    {
        if (Time.time - lastTrapTime < trapCooldown)
        {
            return;
        }

        Vector3 trapPosition = transform.position;
        Instantiate(trapPrefab, trapPosition, Quaternion.identity);
        lastTrapTime = Time.time;
        Debug.Log("トラップを生成しました");
    }

    /// <summary>
    /// プレイヤーの足元に地雷トラップを生成
    /// </summary>
    
    public void CreateMineTrap()
    {
        if (Time.time - lastTrapTime < mineTrapCooldown)
        {
            return;
        }

        Vector3 trapPosition = transform.position;
        Instantiate(mineTrapPrefab, trapPosition, Quaternion.identity);
        lastTrapTime = Time.time;
        Debug.Log("地雷トラップを生成しました");
    }
}