using UnityEngine;

public class PlayerBombItem : MonoBehaviour
{
    [Header("���e�A�C�e���ݒ�")]
    [Tooltip("���̃A�C�e�����擾�����Ƃ��Ɋl���ł��锚�e�̐�")]
    public int ammoAmount = 3;

    [Header("�����ڐݒ�")]
    [SerializeField] private float rotationSpeed = 50f; // ��]���x
    [SerializeField] private float floatAmplitude = 0.3f; // ���V�̐U��
    [SerializeField] private float floatSpeed = 2f; // ���V�̑��x

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // �A�C�e������]������
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // �A�C�e���𕂗V������
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}