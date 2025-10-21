using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSpawn : MonoBehaviour
{
    [Header("スポーン設定")]
    [SerializeField] private int maxItemsOnMap = 5; // マップ上の最大アイテム数
    [SerializeField] private float spawnIntervalMin = 3f; // 最小スポーン間隔（秒）
    [SerializeField] private float spawnIntervalMax = 8f; // 最大スポーン間隔（秒）

    [Header("スポーン範囲")]
    [SerializeField] private float spawnAreaMinX = -10f; // X座標の最小値
    [SerializeField] private float spawnAreaMaxX = 10f; // X座標の最大値
    [SerializeField] private float spawnAreaMinY = -5f; // Y座標の最小値（低さの限界）
    [SerializeField] private float spawnAreaMaxY = 5f; // Y座標の最大値（高さの限界）

    [Header("アイテムプレハブ")]
    [SerializeField] private GameObject bowItemPrefab; // 弓アイテムのプレハブ
    [SerializeField] private GameObject bombItemPrefab; // 爆弾アイテムのプレハブ
    [SerializeField] private GameObject trapItemPrefab; // 罠アイテムのプレハブ

    [Header("プレイヤー参照")]
    [SerializeField] private WeaponInventory playerInventory; // プレイヤーのインベントリ参照

    [Header("デバッグ")]
    [SerializeField] private bool showSpawnArea = true; // スポーン範囲の表示
    [SerializeField] private bool enableSpawning = true; // スポーンの有効/無効

    private List<GameObject> spawnedItems = new List<GameObject>(); // 生成されたアイテムのリスト
    private Coroutine spawnCoroutine;

    private void Start()
    {
        // プレイヤーのインベントリが設定されていない場合、自動検索
        if (playerInventory == null)
        {
            playerInventory = FindObjectOfType<WeaponInventory>();
            if (playerInventory == null)
            {
                Debug.LogWarning("WeaponInventoryが見つかりません。確率調整機能が無効になります。");
            }
        }

        // スポーンコルーチンを開始
        if (enableSpawning)
        {
            spawnCoroutine = StartCoroutine(SpawnRoutine());
        }
    }

    private void OnDestroy()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
    }

    // スポーンのメインルーチン
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // ランダムな間隔で待機
            float waitTime = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(waitTime);

            // アイテム数が最大に達していなければスポーン
            if (spawnedItems.Count < maxItemsOnMap)
            {
                SpawnItem();
            }
        }
    }

    // アイテムをスポーン
    private void SpawnItem()
    {
        // ランダムな位置を取得
        Vector2 spawnPosition = GetRandomPosition();

        // スポーンするアイテムの種類を決定
        WeaponInventory.WeaponType weaponType = DetermineWeaponType();
        GameObject itemPrefab = GetItemPrefab(weaponType);

        if (itemPrefab == null)
        {
            Debug.LogError($"アイテムプレハブが設定されていません: {weaponType}");
            return;
        }

        // アイテムを生成
        GameObject spawnedItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        spawnedItems.Add(spawnedItem);

        // アイテムが破壊されたときにリストから削除
        StartCoroutine(MonitorItemDestruction(spawnedItem));

        Debug.Log($"{weaponType}をスポーンしました。位置: {spawnPosition}");
    }

    // スポーン範囲内のランダムな位置を取得
    private Vector2 GetRandomPosition()
    {
        float randomX = Random.Range(spawnAreaMinX, spawnAreaMaxX);
        float randomY = Random.Range(spawnAreaMinY, spawnAreaMaxY);
        return new Vector2(randomX, randomY);
    }

    // インベントリの状況に応じてスポーンするアイテムの種類を決定
    private WeaponInventory.WeaponType DetermineWeaponType()
    {
        if (playerInventory == null)
        {
            // インベントリ参照がない場合は完全ランダム
            return GetRandomWeaponType();
        }

        // インベントリの状況を確認
        bool hasBow = playerInventory.HasWeapon(WeaponInventory.WeaponType.Bow);
        bool hasBomb = playerInventory.HasWeapon(WeaponInventory.WeaponType.Bomb);
        bool hasTrap = playerInventory.HasWeapon(WeaponInventory.WeaponType.Trap);

        // 各武器の弾薬量を確認
        int bowAmmo = playerInventory.GetAmmo(WeaponInventory.WeaponType.Bow);
        int bombAmmo = playerInventory.GetAmmo(WeaponInventory.WeaponType.Bomb);
        int trapAmmo = playerInventory.GetAmmo(WeaponInventory.WeaponType.Trap);

        // 重み付けを計算（持っていない、または弾薬が少ないほど高確率）
        float bowWeight = CalculateWeight(hasBow, bowAmmo);
        float bombWeight = CalculateWeight(hasBomb, bombAmmo);
        float trapWeight = CalculateWeight(hasTrap, trapAmmo);

        float totalWeight = bowWeight + bombWeight + trapWeight;
        float randomValue = Random.Range(0f, totalWeight);

        // 重み付けに基づいて選択
        if (randomValue < bowWeight)
        {
            return WeaponInventory.WeaponType.Bow;
        }
        else if (randomValue < bowWeight + bombWeight)
        {
            return WeaponInventory.WeaponType.Bomb;
        }
        else
        {
            return WeaponInventory.WeaponType.Trap;
        }
    }

    // 武器の重み付けを計算
    private float CalculateWeight(bool hasWeapon, int ammo)
    {
        if (!hasWeapon)
        {
            // 持っていない武器は高確率（基本重み3倍）
            return 3f;
        }
        else
        {
            // 弾薬が少ないほど高確率
            // maxAmmoの半分以下なら重み1.5倍、それ以上なら1倍
            float ammoRatio = (float)ammo / playerInventory.maxAmmo;
            if (ammoRatio < 0.5f)
            {
                return 1.5f;
            }
            else
            {
                return 1f;
            }
        }
    }

    // ランダムな武器タイプを取得
    private WeaponInventory.WeaponType GetRandomWeaponType()
    {
        int random = Random.Range(0, 3);
        switch (random)
        {
            case 0:
                return WeaponInventory.WeaponType.Bow;
            case 1:
                return WeaponInventory.WeaponType.Bomb;
            case 2:
                return WeaponInventory.WeaponType.Trap;
            default:
                return WeaponInventory.WeaponType.Bow;
        }
    }

    // 武器タイプに対応するプレハブを取得
    private GameObject GetItemPrefab(WeaponInventory.WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponInventory.WeaponType.Bow:
                return bowItemPrefab;
            case WeaponInventory.WeaponType.Bomb:
                return bombItemPrefab;
            case WeaponInventory.WeaponType.Trap:
                return trapItemPrefab;
            default:
                return null;
        }
    }

    // アイテムの破壊を監視
    private IEnumerator MonitorItemDestruction(GameObject item)
    {
        while (item != null)
        {
            yield return null;
        }

        // アイテムが破壊されたらリストから削除
        spawnedItems.Remove(item);
    }

    // 手動でアイテムをスポーン（デバッグ用）
    public void SpawnItemManually()
    {
        if (spawnedItems.Count < maxItemsOnMap)
        {
            SpawnItem();
        }
        else
        {
            Debug.Log("アイテム数が最大に達しています。");
        }
    }

    // スポーンの有効/無効を切り替え
    public void ToggleSpawning(bool enable)
    {
        enableSpawning = enable;

        if (enable && spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnRoutine());
        }
        else if (!enable && spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    // 現在のアイテム数を取得
    public int GetCurrentItemCount()
    {
        return spawnedItems.Count;
    }

    // デバッグ用：スポーン範囲を表示
    private void OnDrawGizmos()
    {
        if (!showSpawnArea) return;

        Gizmos.color = Color.green;
        Vector3 center = new Vector3(
            (spawnAreaMinX + spawnAreaMaxX) / 2f,
            (spawnAreaMinY + spawnAreaMaxY) / 2f,
            0f
        );
        Vector3 size = new Vector3(
            spawnAreaMaxX - spawnAreaMinX,
            spawnAreaMaxY - spawnAreaMinY,
            0f
        );
        Gizmos.DrawWireCube(center, size);

        // スポーン済みアイテムの位置を表示
        Gizmos.color = Color.yellow;
        foreach (GameObject item in spawnedItems)
        {
            if (item != null)
            {
                Gizmos.DrawWireSphere(item.transform.position, 0.3f);
            }
        }
    }
}