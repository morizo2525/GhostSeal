using UnityEngine;

public class PlayerItemManager : MonoBehaviour
{
    [Header("武器インベントリ参照")]
    [SerializeField] private WeaponInventory weaponInventory;

    [Header("アイテム取得設定")]
    [SerializeField] private float pickupRange = 2f; // デバッグ用（描画のみ）
    [SerializeField] private LayerMask itemLayer; // アイテムのレイヤー

    private void Start()
    {
        // WeaponInventoryが設定されていない場合、自動取得を試みる
        if (weaponInventory == null)
        {
            weaponInventory = GetComponent<WeaponInventory>();
            if (weaponInventory == null)
            {
                Debug.LogError("WeaponInventoryが見つかりません！");
            }
        }
    }

    // 触れたら自動で取得
    private void OnTriggerEnter2D(Collider2D other)
    {
        // レイヤーがアイテム層でなければ無視
        if (((1 << other.gameObject.layer) & itemLayer) == 0)
            return;

        TryPickupItem(other.gameObject);
    }

    // アイテムの種類を判定して拾う
    private void TryPickupItem(GameObject item)
    {
        // 弓アイテム
        PlayerBowItem bowItem = item.GetComponent<PlayerBowItem>();
        if (bowItem != null)
        {
            PickupBowItem(bowItem);
            return;
        }

        // 爆弾アイテム
        PlayerBombItem bombItem = item.GetComponent<PlayerBombItem>();
        if (bombItem != null)
        {
            PickupBombItem(bombItem);
            return;
        }

        // 罠アイテム
        PlayerTrapItem trapItem = item.GetComponent<PlayerTrapItem>();
        if (trapItem != null)
        {
            PickupTrapItem(trapItem);
            return;
        }

        Debug.LogWarning($"未知のアイテムタイプです: {item.name}");
    }

    private void PickupBowItem(PlayerBowItem bowItem)
    {
        if (weaponInventory.PikcupWeapon(WeaponInventory.WeaponType.Bow, bowItem.ammoAmount))
        {
            Debug.Log($"弓を取得しました！矢 +{bowItem.ammoAmount}");
            Destroy(bowItem.gameObject);
        }
        else
        {
            Debug.Log("武器スロットが満杯です。");
        }
    }

    private void PickupBombItem(PlayerBombItem bombItem)
    {
        if (weaponInventory.PikcupWeapon(WeaponInventory.WeaponType.Bomb, bombItem.ammoAmount))
        {
            Debug.Log($"爆弾を取得しました！爆弾 +{bombItem.ammoAmount}");
            Destroy(bombItem.gameObject);
        }
        else
        {
            Debug.Log("武器スロットが満杯です。");
        }
    }

    private void PickupTrapItem(PlayerTrapItem trapItem)
    {
        if (weaponInventory.PikcupWeapon(WeaponInventory.WeaponType.Trap, trapItem.ammoAmount))
        {
            Debug.Log($"罠を取得しました！罠 +{trapItem.ammoAmount}");
            Destroy(trapItem.gameObject);
        }
        else
        {
            Debug.Log("武器スロットが満杯です。");
        }
    }

    // デバッグ用：拾える範囲をSceneビューに表示
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
