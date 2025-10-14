using UnityEngine;

public class PlayerTrapCreator : MonoBehaviour
{
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private float trapCooldown = 2f;

    private float lastTrapTime;

    /// <summary>
    /// �v���C���[�̑����Ƀg���b�v�𐶐�
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
}