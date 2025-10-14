using UnityEngine;

public class PlayerTrapCreator : MonoBehaviour
{
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private float trapCooldown = 2f;

    private float lastTrapTime;

    /// <summary>
    /// プレイヤーの足元にトラップを生成
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
}