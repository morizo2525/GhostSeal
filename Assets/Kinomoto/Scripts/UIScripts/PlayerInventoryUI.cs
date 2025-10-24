using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInventoryUI : MonoBehaviour
{
    [Header("参照するインベントリ")]
    public WeaponInventory inventory;

    [Header("個別武器UI")]
    public GameObject bowUI;
    public GameObject bombUI;
    public GameObject trapUI;

    [Header("コンボ武器UI")]
    public GameObject bombBowUI;
    public GameObject trapBowUI;
    public GameObject bombTrapUI;

    [Header("共通残弾数テキスト")]
    public TextMeshProUGUI ammoText;

    [Header("武器アイコンの透明度")]
    [Range(0f, 1f)] public float inactiveAlpha = 0.3f;
    [Range(0f, 1f)] public float activeAlpha = 1f;

    void Start()
    {
        if (inventory == null)
        {
            inventory = FindObjectOfType<WeaponInventory>();
            if (inventory == null)
            {
                Debug.LogError("WeaponInventoryが見つかりません!");
            }
        }

        // 初期状態で全てのUIを非表示
        HideAllWeaponUI();
    }

    void Update()
    {
        if (inventory == null) return;

        // 武器を所持している場合は常に表示
        UpdateWeaponUI();
    }

    private void UpdateWeaponUI()
    {
        // まず全てを非表示
        HideAllWeaponUI();

        WeaponInventory.WeponComboType currentCombo = inventory.GetCurrentCombo();

        // コンボがある場合はコンボ武器のみ表示
        if (currentCombo != WeaponInventory.WeponComboType.None)
        {
            ShowComboWeapon(currentCombo);
        }
        else
        {
            // コンボがない場合は個別武器を表示
            ShowIndividualWeapons();
        }

        // 残弾数テキストの更新
        UpdateAmmoText();
    }

    private void ShowIndividualWeapons()
    {
        // 弾薬がある武器のみ表示
        if (inventory.HasWeapon(WeaponInventory.WeaponType.Bow) &&
            inventory.GetAmmo(WeaponInventory.WeaponType.Bow) > 0)
        {
            if (bowUI != null)
            {
                bowUI.SetActive(true);
                SetUIAlpha(bowUI, activeAlpha);
            }
        }

        if (inventory.HasWeapon(WeaponInventory.WeaponType.Bomb) &&
            inventory.GetAmmo(WeaponInventory.WeaponType.Bomb) > 0)
        {
            if (bombUI != null)
            {
                bombUI.SetActive(true);
                SetUIAlpha(bombUI, activeAlpha);
            }
        }

        if (inventory.HasWeapon(WeaponInventory.WeaponType.Trap) &&
            inventory.GetAmmo(WeaponInventory.WeaponType.Trap) > 0)
        {
            if (trapUI != null)
            {
                trapUI.SetActive(true);
                SetUIAlpha(trapUI, activeAlpha);
            }
        }
    }

    private void ShowComboWeapon(WeaponInventory.WeponComboType comboType)
    {
        switch (comboType)
        {
            case WeaponInventory.WeponComboType.BombBow:
                int bombBowAmmo = Mathf.Min(inventory.GetAmmo(WeaponInventory.WeaponType.Bomb),
                                           inventory.GetAmmo(WeaponInventory.WeaponType.Bow));
                if (bombBowAmmo > 0 && bombBowUI != null)
                {
                    bombBowUI.SetActive(true);
                    SetUIAlpha(bombBowUI, activeAlpha);
                }
                break;

            case WeaponInventory.WeponComboType.TrapBow:
                int trapBowAmmo = Mathf.Min(inventory.GetAmmo(WeaponInventory.WeaponType.Trap),
                                           inventory.GetAmmo(WeaponInventory.WeaponType.Bow));
                if (trapBowAmmo > 0 && trapBowUI != null)
                {
                    trapBowUI.SetActive(true);
                    SetUIAlpha(trapBowUI, activeAlpha);
                }
                break;

            case WeaponInventory.WeponComboType.BombTrap:
                int bombTrapAmmo = Mathf.Min(inventory.GetAmmo(WeaponInventory.WeaponType.Bomb),
                                             inventory.GetAmmo(WeaponInventory.WeaponType.Trap));
                if (bombTrapAmmo > 0 && bombTrapUI != null)
                {
                    bombTrapUI.SetActive(true);
                    SetUIAlpha(bombTrapUI, activeAlpha);
                }
                break;
        }
    }

    private void HideAllWeaponUI()
    {
        if (bowUI != null) bowUI.SetActive(false);
        if (bombUI != null) bombUI.SetActive(false);
        if (trapUI != null) trapUI.SetActive(false);
        if (bombBowUI != null) bombBowUI.SetActive(false);
        if (trapBowUI != null) trapBowUI.SetActive(false);
        if (bombTrapUI != null) bombTrapUI.SetActive(false);
        if (ammoText != null) ammoText.text = "";
    }

    private void UpdateAmmoText()
    {
        if (ammoText == null) return;

        string ammoDisplay = "";
        WeaponInventory.WeponComboType currentCombo = inventory.GetCurrentCombo();

        // コンボがある場合
        if (currentCombo != WeaponInventory.WeponComboType.None)
        {
            switch (currentCombo)
            {
                case WeaponInventory.WeponComboType.BombBow:
                    int bombBowAmmo = Mathf.Min(inventory.GetAmmo(WeaponInventory.WeaponType.Bomb),
                                                inventory.GetAmmo(WeaponInventory.WeaponType.Bow));
                    if (bombBowAmmo > 0)
                    {
                        ammoDisplay = "×" + bombBowAmmo.ToString("00");
                    }
                    break;

                case WeaponInventory.WeponComboType.TrapBow:
                    int trapBowAmmo = Mathf.Min(inventory.GetAmmo(WeaponInventory.WeaponType.Trap),
                                                inventory.GetAmmo(WeaponInventory.WeaponType.Bow));
                    if (trapBowAmmo > 0)
                    {
                        ammoDisplay = "×" + trapBowAmmo.ToString("00");
                    }
                    break;

                case WeaponInventory.WeponComboType.BombTrap:
                    int bombTrapAmmo = Mathf.Min(inventory.GetAmmo(WeaponInventory.WeaponType.Bomb),
                                                 inventory.GetAmmo(WeaponInventory.WeaponType.Trap));
                    if (bombTrapAmmo > 0)
                    {
                        ammoDisplay = "×" + bombTrapAmmo.ToString("00");
                    }
                    break;
            }
        }
        else
        {
            // コンボがない場合は個別武器の残弾数
            bool first = true;

            if (inventory.HasWeapon(WeaponInventory.WeaponType.Bow) &&
                inventory.GetAmmo(WeaponInventory.WeaponType.Bow) > 0)
            {
                ammoDisplay += "×" + inventory.GetAmmo(WeaponInventory.WeaponType.Bow).ToString("00");
                first = false;
            }

            if (inventory.HasWeapon(WeaponInventory.WeaponType.Bomb) &&
                inventory.GetAmmo(WeaponInventory.WeaponType.Bomb) > 0)
            {
                if (!first) ammoDisplay += "\n";
                ammoDisplay += "×" + inventory.GetAmmo(WeaponInventory.WeaponType.Bomb).ToString("00");
                first = false;
            }

            if (inventory.HasWeapon(WeaponInventory.WeaponType.Trap) &&
                inventory.GetAmmo(WeaponInventory.WeaponType.Trap) > 0)
            {
                if (!first) ammoDisplay += "\n";
                ammoDisplay += "×" + inventory.GetAmmo(WeaponInventory.WeaponType.Trap).ToString("00");
            }
        }

        ammoText.text = ammoDisplay;
    }

    private void SetUIAlpha(GameObject uiObject, float alpha)
    {
        if (uiObject == null) return;

        Image[] images = uiObject.GetComponentsInChildren<Image>();
        foreach (Image img in images)
        {
            Color color = img.color;
            color.a = alpha;
            img.color = color;
        }

        TextMeshProUGUI[] texts = uiObject.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in texts)
        {
            if (text != ammoText)
            {
                Color color = text.color;
                color.a = alpha;
                text.color = color;
            }
        }
    }
}