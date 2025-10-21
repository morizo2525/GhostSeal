using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSpawn : MonoBehaviour
{
    [Header("�X�|�[���ݒ�")]
    [SerializeField] private int maxItemsOnMap = 5; // �}�b�v��̍ő�A�C�e����
    [SerializeField] private float spawnIntervalMin = 3f; // �ŏ��X�|�[���Ԋu�i�b�j
    [SerializeField] private float spawnIntervalMax = 8f; // �ő�X�|�[���Ԋu�i�b�j

    [Header("�X�|�[���͈�")]
    [SerializeField] private float spawnAreaMinX = -10f; // X���W�̍ŏ��l
    [SerializeField] private float spawnAreaMaxX = 10f; // X���W�̍ő�l
    [SerializeField] private float spawnAreaMinY = -5f; // Y���W�̍ŏ��l�i�Ⴓ�̌��E�j
    [SerializeField] private float spawnAreaMaxY = 5f; // Y���W�̍ő�l�i�����̌��E�j

    [Header("�A�C�e���v���n�u")]
    [SerializeField] private GameObject bowItemPrefab; // �|�A�C�e���̃v���n�u
    [SerializeField] private GameObject bombItemPrefab; // ���e�A�C�e���̃v���n�u
    [SerializeField] private GameObject trapItemPrefab; // 㩃A�C�e���̃v���n�u

    [Header("�v���C���[�Q��")]
    [SerializeField] private WeaponInventory playerInventory; // �v���C���[�̃C���x���g���Q��

    [Header("�f�o�b�O")]
    [SerializeField] private bool showSpawnArea = true; // �X�|�[���͈͂̕\��
    [SerializeField] private bool enableSpawning = true; // �X�|�[���̗L��/����

    private List<GameObject> spawnedItems = new List<GameObject>(); // �������ꂽ�A�C�e���̃��X�g
    private Coroutine spawnCoroutine;

    private void Start()
    {
        // �v���C���[�̃C���x���g�����ݒ肳��Ă��Ȃ��ꍇ�A��������
        if (playerInventory == null)
        {
            playerInventory = FindObjectOfType<WeaponInventory>();
            if (playerInventory == null)
            {
                Debug.LogWarning("WeaponInventory��������܂���B�m�������@�\�������ɂȂ�܂��B");
            }
        }

        // �X�|�[���R���[�`�����J�n
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

    // �X�|�[���̃��C�����[�`��
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // �����_���ȊԊu�őҋ@
            float waitTime = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(waitTime);

            // �A�C�e�������ő�ɒB���Ă��Ȃ���΃X�|�[��
            if (spawnedItems.Count < maxItemsOnMap)
            {
                SpawnItem();
            }
        }
    }

    // �A�C�e�����X�|�[��
    private void SpawnItem()
    {
        // �����_���Ȉʒu���擾
        Vector2 spawnPosition = GetRandomPosition();

        // �X�|�[������A�C�e���̎�ނ�����
        WeaponInventory.WeaponType weaponType = DetermineWeaponType();
        GameObject itemPrefab = GetItemPrefab(weaponType);

        if (itemPrefab == null)
        {
            Debug.LogError($"�A�C�e���v���n�u���ݒ肳��Ă��܂���: {weaponType}");
            return;
        }

        // �A�C�e���𐶐�
        GameObject spawnedItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        spawnedItems.Add(spawnedItem);

        // �A�C�e�����j�󂳂ꂽ�Ƃ��Ƀ��X�g����폜
        StartCoroutine(MonitorItemDestruction(spawnedItem));

        Debug.Log($"{weaponType}���X�|�[�����܂����B�ʒu: {spawnPosition}");
    }

    // �X�|�[���͈͓��̃����_���Ȉʒu���擾
    private Vector2 GetRandomPosition()
    {
        float randomX = Random.Range(spawnAreaMinX, spawnAreaMaxX);
        float randomY = Random.Range(spawnAreaMinY, spawnAreaMaxY);
        return new Vector2(randomX, randomY);
    }

    // �C���x���g���̏󋵂ɉ����ăX�|�[������A�C�e���̎�ނ�����
    private WeaponInventory.WeaponType DetermineWeaponType()
    {
        if (playerInventory == null)
        {
            // �C���x���g���Q�Ƃ��Ȃ��ꍇ�͊��S�����_��
            return GetRandomWeaponType();
        }

        // �C���x���g���̏󋵂��m�F
        bool hasBow = playerInventory.HasWeapon(WeaponInventory.WeaponType.Bow);
        bool hasBomb = playerInventory.HasWeapon(WeaponInventory.WeaponType.Bomb);
        bool hasTrap = playerInventory.HasWeapon(WeaponInventory.WeaponType.Trap);

        // �e����̒e��ʂ��m�F
        int bowAmmo = playerInventory.GetAmmo(WeaponInventory.WeaponType.Bow);
        int bombAmmo = playerInventory.GetAmmo(WeaponInventory.WeaponType.Bomb);
        int trapAmmo = playerInventory.GetAmmo(WeaponInventory.WeaponType.Trap);

        // �d�ݕt�����v�Z�i�����Ă��Ȃ��A�܂��͒e�򂪏��Ȃ��قǍ��m���j
        float bowWeight = CalculateWeight(hasBow, bowAmmo);
        float bombWeight = CalculateWeight(hasBomb, bombAmmo);
        float trapWeight = CalculateWeight(hasTrap, trapAmmo);

        float totalWeight = bowWeight + bombWeight + trapWeight;
        float randomValue = Random.Range(0f, totalWeight);

        // �d�ݕt���Ɋ�Â��đI��
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

    // ����̏d�ݕt�����v�Z
    private float CalculateWeight(bool hasWeapon, int ammo)
    {
        if (!hasWeapon)
        {
            // �����Ă��Ȃ�����͍��m���i��{�d��3�{�j
            return 3f;
        }
        else
        {
            // �e�򂪏��Ȃ��قǍ��m��
            // maxAmmo�̔����ȉ��Ȃ�d��1.5�{�A����ȏ�Ȃ�1�{
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

    // �����_���ȕ���^�C�v���擾
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

    // ����^�C�v�ɑΉ�����v���n�u���擾
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

    // �A�C�e���̔j����Ď�
    private IEnumerator MonitorItemDestruction(GameObject item)
    {
        while (item != null)
        {
            yield return null;
        }

        // �A�C�e�����j�󂳂ꂽ�烊�X�g����폜
        spawnedItems.Remove(item);
    }

    // �蓮�ŃA�C�e�����X�|�[���i�f�o�b�O�p�j
    public void SpawnItemManually()
    {
        if (spawnedItems.Count < maxItemsOnMap)
        {
            SpawnItem();
        }
        else
        {
            Debug.Log("�A�C�e�������ő�ɒB���Ă��܂��B");
        }
    }

    // �X�|�[���̗L��/������؂�ւ�
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

    // ���݂̃A�C�e�������擾
    public int GetCurrentItemCount()
    {
        return spawnedItems.Count;
    }

    // �f�o�b�O�p�F�X�|�[���͈͂�\��
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

        // �X�|�[���ς݃A�C�e���̈ʒu��\��
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