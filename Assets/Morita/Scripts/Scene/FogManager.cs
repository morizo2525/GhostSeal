using UnityEngine;

[System.Serializable]
public class FogLayer
{
    public Transform fogTransform; // 霧オブジェクト
    public float minSpeed = 0.1f;
    public float maxSpeed = 0.5f;
    [HideInInspector] public float speed;
}

public class FogManager : MonoBehaviour
{
    [Header("霧レイヤー")]
    public FogLayer[] fogLayers;

    [Header("X座標リセット")]
    public float resetOffset = 1f; // 左端に出たら右端に戻す余白

    private Camera cam;
    private float camHeight;
    private float camWidth;
    private float startX;
    private float resetX;
    private float minY;
    private float maxY;

    void Start()
    {
        cam = Camera.main;
        camHeight = 2f * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        startX = cam.transform.position.x + camWidth / 2f + resetOffset;
        resetX = cam.transform.position.x - camWidth / 2f - resetOffset;

        minY = cam.transform.position.y - camHeight / 2f;
        maxY = cam.transform.position.y + camHeight / 2f;

        foreach (var layer in fogLayers)
        {
            // 初期速度をランダム化
            layer.speed = Random.Range(layer.minSpeed, layer.maxSpeed);

            // 初期位置をランダム化
            Vector3 pos = layer.fogTransform.position;
            pos.x = Random.Range(resetX, startX);
            pos.y = Random.Range(minY, maxY);
            layer.fogTransform.position = pos;

            // 奥行き感を出すために透明度を変える（optional）
            SpriteRenderer sr = layer.fogTransform.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                float alpha = Mathf.Lerp(0.3f, 0.8f, (layer.speed - layer.minSpeed) / (layer.maxSpeed - layer.minSpeed));
                Color c = sr.color;
                c.a = alpha;
                sr.color = c;
            }
        }
    }

    void Update()
    {
        foreach (var layer in fogLayers)
        {
            layer.fogTransform.Translate(Vector2.right * layer.speed * Time.deltaTime);

            // 右端を超えたら左端に戻す
            if (layer.fogTransform.position.x >= startX)
            {
                Vector3 pos = layer.fogTransform.position;
                pos.x = resetX;
                pos.y = Random.Range(minY, maxY);
                layer.fogTransform.position = pos;

                // 速度をランダム化
                layer.speed = Random.Range(layer.minSpeed, layer.maxSpeed);
            }
        }
    }
}
