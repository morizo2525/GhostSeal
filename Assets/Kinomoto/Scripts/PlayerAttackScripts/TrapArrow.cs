using UnityEngine;

public class TrapArrow : MonoBehaviour
{
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private float destroyDelay = 0.1f;  // ���e��̍폜�x������
    private Rigidbody2D rb;
    private bool hasCollided = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasCollided)
            return;

        hasCollided = true;

        // ���e�ʒu�Ƀg���b�v�𐶐�
        CreateTrapAtImpact(collision.gameObject);

        // ����폜
        Destroy(gameObject, destroyDelay);
    }

    /// <summary>
    /// ���e�ʒu�Ƀg���b�v�𐶐�
    /// </summary>
    private void CreateTrapAtImpact(GameObject hitObject)
    {
        // �g���b�vPrefab���ݒ肳��Ă��Ȃ��ꍇ�͌x�����o���ďI��
        if (trapPrefab == null)
        {
            Debug.LogWarning("�g���b�vPrefab���ݒ肳��Ă��܂���");
            return;
        }

        // ���e�ʒu�Ńg���b�v�𐶐�
        Instantiate(trapPrefab, transform.position, Quaternion.identity);
    }
}
