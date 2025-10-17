using UnityEngine;

public class PlayerBombItem : MonoBehaviour
{
    [Header("爆弾アイテム設定")]
    [Tooltip("このアイテムを取得したときに獲得できる爆弾の数")]
    public int ammoAmount = 3;

    [Header("見た目設定")]
    [SerializeField] private float rotationSpeed = 50f; // 回転速度
    [SerializeField] private float floatAmplitude = 0.3f; // 浮遊の振幅
    [SerializeField] private float floatSpeed = 2f; // 浮遊の速度

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // アイテムを回転させる
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // アイテムを浮遊させる
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}