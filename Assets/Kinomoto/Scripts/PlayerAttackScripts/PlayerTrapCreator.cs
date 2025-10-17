using UnityEngine;

public class PlayerTrapCreator : MonoBehaviour
{
    [Header("�g���o�T�~�g���b�v�i㩒P�̍U���j�̐ݒ�")]
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private float trapCooldown = 2f;

    [Header("�n���g���b�v�̐ݒ�")]
    [SerializeField] private GameObject mineTrapPrefab;
    [SerializeField] private float mineTrapCooldown = 5f;

    private float lastTrapTime;

    /// <summary>
    /// �v���C���[�̑����Ƀg���o�T�~�g���b�v�𐶐�
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
        Debug.Log("�g���b�v�𐶐����܂���");
    }

    /// <summary>
    /// �v���C���[�̑����ɒn���g���b�v�𐶐�
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
        Debug.Log("�n���g���b�v�𐶐����܂���");
    }
}