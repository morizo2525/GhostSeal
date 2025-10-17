using UnityEngine;

public class PlayerItemManager : MonoBehaviour
{
    [Header("����C���x���g���Q��")]
    [SerializeField] private WeaponInventory weaponInventory;

    [Header("�A�C�e���擾�ݒ�")]
    [SerializeField] private float pickupRange = 2f; // �f�o�b�O�p�i�`��̂݁j
    [SerializeField] private LayerMask itemLayer; // �A�C�e���̃��C���[

    private void Start()
    {
        // WeaponInventory���ݒ肳��Ă��Ȃ��ꍇ�A�����擾�����݂�
        if (weaponInventory == null)
        {
            weaponInventory = GetComponent<WeaponInventory>();
            if (weaponInventory == null)
            {
                Debug.LogError("WeaponInventory��������܂���I");
            }
        }
    }

    // �G�ꂽ�玩���Ŏ擾
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���C���[���A�C�e���w�łȂ���Ζ���
        if (((1 << other.gameObject.layer) & itemLayer) == 0)
            return;

        TryPickupItem(other.gameObject);
    }

    // �A�C�e���̎�ނ𔻒肵�ďE��
    private void TryPickupItem(GameObject item)
    {
        // �|�A�C�e��
        PlayerBowItem bowItem = item.GetComponent<PlayerBowItem>();
        if (bowItem != null)
        {
            PickupBowItem(bowItem);
            return;
        }

        // ���e�A�C�e��
        PlayerBombItem bombItem = item.GetComponent<PlayerBombItem>();
        if (bombItem != null)
        {
            PickupBombItem(bombItem);
            return;
        }

        // 㩃A�C�e��
        PlayerTrapItem trapItem = item.GetComponent<PlayerTrapItem>();
        if (trapItem != null)
        {
            PickupTrapItem(trapItem);
            return;
        }

        Debug.LogWarning($"���m�̃A�C�e���^�C�v�ł�: {item.name}");
    }

    private void PickupBowItem(PlayerBowItem bowItem)
    {
        if (weaponInventory.PikcupWeapon(WeaponInventory.WeaponType.Bow, bowItem.ammoAmount))
        {
            Debug.Log($"�|���擾���܂����I�� +{bowItem.ammoAmount}");
            Destroy(bowItem.gameObject);
        }
        else
        {
            Debug.Log("����X���b�g�����t�ł��B");
        }
    }

    private void PickupBombItem(PlayerBombItem bombItem)
    {
        if (weaponInventory.PikcupWeapon(WeaponInventory.WeaponType.Bomb, bombItem.ammoAmount))
        {
            Debug.Log($"���e���擾���܂����I���e +{bombItem.ammoAmount}");
            Destroy(bombItem.gameObject);
        }
        else
        {
            Debug.Log("����X���b�g�����t�ł��B");
        }
    }

    private void PickupTrapItem(PlayerTrapItem trapItem)
    {
        if (weaponInventory.PikcupWeapon(WeaponInventory.WeaponType.Trap, trapItem.ammoAmount))
        {
            Debug.Log($"㩂��擾���܂����I� +{trapItem.ammoAmount}");
            Destroy(trapItem.gameObject);
        }
        else
        {
            Debug.Log("����X���b�g�����t�ł��B");
        }
    }

    // �f�o�b�O�p�F�E����͈͂�Scene�r���[�ɕ\��
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
