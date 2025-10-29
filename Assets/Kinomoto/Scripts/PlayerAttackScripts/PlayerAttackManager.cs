using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    //プレイヤーの攻撃全体を管理するスクリプト

    [Header("基本攻撃スクリプト")]
    public PlayerSwordAttack swordAttack;   //剣攻撃（基本攻撃）スクリプト

    [Header("拾える武器の攻撃スクリプト")]
    public PlayerAttack_Bow bowAttack;       //弓攻撃スクリプト
    public PlayerBombAttack bombAttack;      //爆弾攻撃スクリプト
    public PlayerTrapCreator trapCreator;    //罠設置スクリプト

    [Header("組み合わせ攻撃スクリプト")]
    public PlayerBoomBowAttack boomBowAttack; //爆発矢スクリプト
    public PlayerTrapBowAttack trapBowAttack; //トラップ矢スクリプト

    [Header("武器インベントリのスクリプト")]
    public WeaponInventory weaponInventory;

    [Header("攻撃サウンド設定")]
    public AudioClip bowShootSE;           // 通常の弓矢発射SE
    public AudioClip bombThrowSE;          // 爆弾投擲SE
    public AudioClip trapCreateSE;         // 罠設置SE
    public AudioClip boomBowShootSE;       // 爆弾矢発射SE
    public AudioClip trapBowShootSE;       // トラップ矢発射SE
    public AudioClip mineTrapCreateSE;     // 地雷設置SE
    [Range(0f, 1f)]
    public float seVolume = 1.0f;          // SEの音量
    private AudioSource audioSource;

    private void Start()
    {
        // AudioSourceコンポーネントを取得または追加
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        //WeaponInventoryスクリプトの参照を取得
        if (weaponInventory == null)
        {
            weaponInventory = GetComponent<WeaponInventory>();
            if (weaponInventory == null)
            {
                Debug.LogError("WeaponInventoryスクリプトが見つかりません。");
            }
        }

        //弓攻撃スクリプトの参照を取得
        if (bowAttack == null)
        {
            bowAttack = GetComponent<PlayerAttack_Bow>();
        }

        //爆弾攻撃スクリプトの参照を取得
        if (bombAttack == null)
        {
            bombAttack = GetComponent<PlayerBombAttack>();
        }

        //罠設置スクリプトの参照を取得
        if (trapCreator == null)
        {
            trapCreator = GetComponent<PlayerTrapCreator>();
        }
    }

    void Update()
    {
        //左クリックで基本の剣攻撃
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (swordAttack != null)
            {
                swordAttack.SwordAttack(); //剣攻撃メソッドを呼び出す（剣のSEは剣攻撃スクリプト内で再生）
            }
        }

        //右クリックで拾った他の武器攻撃
        //WeaponInventoryから参照して所有状況を確認、スクリプトの呼び出しをする
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
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
        switch (combo)
        {
            case WeaponInventory.WeponComboType.BombBow:
                //爆弾＋弓のコンボ効果: 爆弾矢を発射
                if (boomBowAttack != null && weaponInventory.UseComboWeapon())
                {
                    // 爆弾矢SE再生
                    PlaySE(boomBowShootSE);

                    boomBowAttack.ShootBoomArrow(); //爆弾矢を発射
                    Debug.Log("爆弾矢を発射しました！");
                }
                break;

            case WeaponInventory.WeponComboType.TrapBow:
                //罠+弓：トラップアロー
                if (trapBowAttack != null && weaponInventory.UseComboWeapon())
                {
                    // トラップ矢SE再生
                    PlaySE(trapBowShootSE);

                    trapBowAttack.BowShootTrapArrow(); //トラップアローを発射
                    Debug.Log("トラップアローを発射しました！");
                }
                break;

            case WeaponInventory.WeponComboType.BombTrap:
                //爆弾 + 罠：地雷
                if (trapCreator != null && weaponInventory.UseComboWeapon())
                {
                    // 地雷設置SE再生
                    PlaySE(mineTrapCreateSE);

                    trapCreator.CreateMineTrap();
                    Debug.Log("地雷トラップを設置しました！");
                }
                break;
        }
    }

    //単体の武器を使用するメソッド
    void UseSingleWeapon()
    {
        // 優先順位：弓 > 爆弾 > 罠
        if (weaponInventory.HasWeapon(WeaponInventory.WeaponType.Bow))
        {
            if (bowAttack != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Bow))
            {
                // 弓矢SE再生
                PlaySE(bowShootSE);

                bowAttack.BowShoot(); //通常の矢を発射
                Debug.Log("通常の矢を発射しました");
            }
        }
        else if (weaponInventory.HasWeapon(WeaponInventory.WeaponType.Bomb))
        {
            if (bombAttack != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Bomb))
            {
                // 爆弾投擲SE再生
                PlaySE(bombThrowSE);

                bombAttack.ThrowBomb(); //爆弾を単体で投げる
                Debug.Log("爆弾を投擲しました");
            }
        }
        else if (weaponInventory.HasWeapon(WeaponInventory.WeaponType.Trap))
        {
            if (trapCreator != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Trap))
            {
                // 罠設置SE再生
                PlaySE(trapCreateSE);

                trapCreator.CreateTrap(); //罠を設置
            }
        }
        else
        {
            Debug.Log("使用可能な武器がありません");
        }
    }

    // SEを再生するヘルパーメソッド
    private void PlaySE(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip, seVolume);
        }
    }
}