using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    //プレイヤーの攻撃全体を管理するスクリプト

    [Header("基本攻撃スクリプト")]
    public PlayerSwordAttack swordAttack;   //剣攻撃（基本攻撃）スクリプト

    //[Header("拾える武器の攻撃スクリプト")]

    //後で攻撃スクリプトを準備し、マージ等した際にそのスクリプトの名前に変更
    //三種類の攻撃スクリプトを事前に準備する必要あり（武器組み合わせ時）
    //例：PlayerBowAttackの部分を該当スクリプト名に変更

    //public PlayerBowAttack bowAttack;       //弓攻撃スクリプト
    //public PlayerBombAttack bombAttack;     //爆弾攻撃スクリプト
    //public PlayerTrapAttack trapAttack;     //罠設置スクリプト

    [Header("武器インベントリのスクリプト")]
    public WeaponInventory weaponInventory;

    private void Start()
    {
        //WaponInventoryスクリプトの参照を取得
        if (weaponInventory == null)
        {
            weaponInventory = GetComponent<WeaponInventory>();
            if (weaponInventory == null)
            {
                Debug.LogError("WeaponInventoryスクリプトが見つかりません。");
            }
        }
    }

    void Update()
    {
        //左クリックで基本の剣攻撃
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (swordAttack != null)
            {
                swordAttack.SwordAttack(); //剣攻撃メソッドを呼び出す
            }
        }

        //右クリックで拾った他の武器攻撃
        //WeaponInventoryから参照して所有状況を確認、スクリプトの呼び出しをする
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //例:弓攻撃や爆弾設置など
            UseWeapon();
        }
    }

    //拾った武器を使用するメソッド
    void UseWeapon()
    {
        if (weaponInventory == null) return;

        WeaponInventory.WeponComboType combo = weaponInventory.GetCurrentCombo();

        //コンボ効果（2種の武器を所持している）がある場合
        if (combo != WeaponInventory.WeponComboType.None)
        {
            UseComboAttack(combo);
        }
        //単体の武器を使用する場合
        else
        {
            UseSingleWeapon();
        }
    }

    //コンボ攻撃を使用するメソッド
    void UseComboAttack(WeaponInventory.WeponComboType combo)
    {
        //個々の処理の実装も三種類の攻撃スクリプトを事前に準備する必要あり

        //switch (combo)
        //{
        //case WeaponInventory.WeponComboType.BombBow:
        //        爆弾＋弓のコンボ効果: 爆弾矢を発射
        //        if (bowAttack != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Bow))
        //        {
        //            bowAttack.ShootBombArrow(); //爆弾矢を発射
        //        }
        //        break;
        //case WeaponInventory.WeponComboType.TrapBow:
        //    // 罠+弓：トラップアロー
        //    if (bowAttack != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Bow))
        //    {
        //        bowAttack.TrapArrowAttack();
        //    }
        //    break;

        //case WeaponInventory.WeponComboType.BombTrap:
        //    // 爆弾+罠：地雷
        //    if (trapAttack != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Trap))
        //    {
        //        trapAttack.LandmineAttack();
        //    }
        //    break;
        //}
    }

    //単体の武器を使用するメソッド
    void UseSingleWeapon()
    {
        //個々の処理の実装も三種類の攻撃スクリプトを事前に準備する必要あり

        //if (weaponInventory.HasWeapon(WeaponInventory.WeaponType.Bow))
        //{
        //    if (bowAttack != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Bow))
        //    {
        //        bowAttack.ShootArrow(); //通常の矢を発射
        //    }
        //}
        //else if (weaponInventory.HasWeapon(WeaponInventory.WeaponType.Bomb))
        //{
        //    if (bombAttack != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Bomb))
        //    {
        //        bombAttack.ThrowBomb(); //爆弾を投げる
        //    }
        //}
        //else if (weaponInventory.HasWeapon(WeaponInventory.WeaponType.Trap))
        //{
        //    if (trapAttack != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Trap))
        //    {
        //        trapAttack.PlaceTrap(); //罠を設置
        //    }
        //}
    }

    // 武器の種類に応じて攻撃を実行
    //void ExecuteWeaponAttack(WeaponInventory.WeaponType weaponType)
    //{
    //    switch (weaponType)
    //    {
    //        case WeaponInventory.WeaponType.Bow:
    //            if (bowAttack != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Bow))
    //            {
    //                bowAttack.NormalArrowAttack();
    //            }
    //            break;

    //        case WeaponInventory.WeaponType.Bomb:
    //            if (bombAttack != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Bomb))
    //            {
    //                bombAttack.PlaceBomb();
    //            }
    //            break;

    //        case WeaponInventory.WeaponType.Trap:
    //            if (trapAttack != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Trap))
    //            {
    //                trapAttack.PlaceTrap();
    //            }
    //            break;
    //    }
    //}
}
