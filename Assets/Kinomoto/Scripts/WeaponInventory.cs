using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    //拾える武器の種類を定義
    public enum WeaponType
    {
        None,
        Bow,    //弓
        Bomb,   //爆弾
        Trap    //罠
    }

    //武器の組み合わせによる特殊効果を定義
    public enum WeponComboType
    {
        None,           //なし
        BombBow,        //爆弾＋弓
        TrapBow,        //罠＋弓
        BombTrap,       //爆弾＋罠
    }

    [Header("所有武器（最大2種類）")]
    public WeaponType weaponSlot1 = WeaponType.None; //武器スロット1
    public WeaponType weaponSlot2 = WeaponType.None; //武器スロット2

    [Header("武器の残段数")]
    public int bowAmmo = 0;     //弓の矢の残数
    public int bombAmmo = 0;    //爆弾の残数
    public int trapAmmo = 0;    //罠の残数
    public int maxAmmo = 10;    //各武器の最大残数

    private WeponComboType currentCombo = WeponComboType.None; //現在の武器コンボ

    void Start()
    {
        UpdateWeaponCombo(); //初期化時に武器コンボを更新
    }

    //武器を拾うメソッド
    public bool PikcupWeapon(WeaponType weaponType, int ammo)
    {
        if (weaponType == WeaponType.None)
        {
            Debug.LogWarning("無効な武器タイプです。");
            return false;
        }

        //既に同じ武器を持っている場合は、弾薬を追加
        if (weaponSlot1 == weaponType || weaponSlot2 == weaponType)
        {
            AddAmmo(weaponType, ammo);
            Debug.Log(weaponType + "の弾薬を追加しました。現在の残数: ");
            return true;
        }

        //武器スロット１が開いている場合
        if (weaponSlot1 == WeaponType.None)
        {
            weaponSlot1 = weaponType;
            AddAmmo(weaponType, ammo);
            UpdateWeaponCombo();
            Debug.Log(weaponType + "をスロット1に装備しました。");
            return true;
        }

        //武器スロット２が開いている場合
        if (weaponSlot2 == WeaponType.None)
        {
            weaponSlot2 = weaponType;
            AddAmmo(weaponType, ammo);
            UpdateWeaponCombo();
            Debug.Log(weaponType + "をスロット2に装備しました。");
            return true;
        }

        //両方のスロットが埋まっている場合
        Debug.Log("武器スロットが満杯です。");
        return false;
    }

    private void AddAmmo(WeaponType weaponType, int ammo)
    {
        switch (weaponType)
        {
            case WeaponType.Bow:
                bowAmmo = Mathf.Min(bowAmmo + ammo, maxAmmo);
                break;
            case WeaponType.Bomb:
                bombAmmo = Mathf.Min(bombAmmo + ammo, maxAmmo);
                break;
            case WeaponType.Trap:
                trapAmmo = Mathf.Min(trapAmmo + ammo, maxAmmo);
                break;
        }
    }

    //武器の仕様（弾薬の消費）
    public bool UseWeapon(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Bow:
                if (bowAmmo > 0)
                {
                    bowAmmo--;
                    CheckWeaponEmpty(weaponType);
                    Debug.Log("弓を使用しました。残りの矢: " + bowAmmo);
                    return true;
                }
                break;

            case WeaponType.Bomb:
                if (bombAmmo > 0)
                {
                    bombAmmo--;
                    CheckWeaponEmpty(weaponType);
                    Debug.Log("爆弾を使用しました。残りの爆弾: " + bombAmmo);
                    return true;
                }
                break;
            case WeaponType.Trap:
                if (trapAmmo > 0)
                {
                    trapAmmo--;
                    CheckWeaponEmpty(weaponType);
                    Debug.Log("罠を設置しました。残りの罠: " + trapAmmo);
                    return true;
                }
                break;
        }
        Debug.Log("弾薬が不足しています。");
        return false;
    }

    //弾薬が尽きた武器を武器スロットから削除
    private void CheckWeaponEmpty(WeaponType weaponType)
    {
        int ammo = GetAmmo(weaponType);

        if (ammo <= 0)
        {
            if (weaponSlot1 == weaponType)
            {
                weaponSlot1 = WeaponType.None;
            }
            else if (weaponSlot2 == weaponType)
            {
                weaponSlot2 = WeaponType.None;
            }
            UpdateWeaponCombo();
        }
    }

    //両方のスロットが埋まっている場合の組み合わせ効果
    private void UpdateWeaponCombo()
    {
        if (weaponSlot1 != WeaponType.None && weaponSlot2 != WeaponType.None)
        {
            if ((weaponSlot1 == WeaponType.Bomb && weaponSlot2 == WeaponType.Bow) ||
                (weaponSlot1 == WeaponType.Bow && weaponSlot2 == WeaponType.Bomb))
            {
                currentCombo = WeponComboType.BombBow;
                Debug.Log("爆弾＋弓のコンボ効果:爆弾矢が発動！");
            }
            else if ((weaponSlot1 == WeaponType.Trap && weaponSlot2 == WeaponType.Bow) ||
                     (weaponSlot1 == WeaponType.Bow && weaponSlot2 == WeaponType.Trap))
            {
                currentCombo = WeponComboType.TrapBow;
                Debug.Log("罠＋弓のコンボ効果:トラップアローが発動！");
            }
            else if ((weaponSlot1 == WeaponType.Bomb && weaponSlot2 == WeaponType.Trap) ||
                     (weaponSlot1 == WeaponType.Trap && weaponSlot2 == WeaponType.Bomb))
            {
                currentCombo = WeponComboType.BombTrap;
                Debug.Log("爆弾＋罠のコンボ効果:地雷が発動！");
            }
            else
            {
                currentCombo = WeponComboType.None;
            }
        }
        else
        {
            currentCombo = WeponComboType.None;
        }
    }

    //現在の組み合わせ（コンボ）効果を取得
    public WeponComboType GetCurrentCombo()
    {
        return currentCombo;
    }

    //武器の所有状況を取得
    public bool HasWeapon(WeaponType weaponType)
    {
        return weaponSlot1 == weaponType || weaponSlot2 == weaponType;
    }

    //弾薬の数を取得
    public int GetAmmo(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Bow:
                return bowAmmo;
            case WeaponType.Bomb:
                return bombAmmo;
            case WeaponType.Trap:
                return trapAmmo;
            default:
                return 0;
        }
    }

    //スロットが開いているかを確認
    public bool HasEmptySlot()
    {
        return weaponSlot1 == WeaponType.None || weaponSlot2 == WeaponType.None;
    }
}
