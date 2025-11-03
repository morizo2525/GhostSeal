using UnityEngine;

public class BowVisualManager : MonoBehaviour
{
    [Header("弓オブジェクト参照")]
    [SerializeField] private GameObject normalBowObject;    // 通常の弓オブジェクト
    [SerializeField] private GameObject boomBowObject;      // 爆弾矢用の弓オブジェクト
    [SerializeField] private GameObject trapBowObject;      // トラップ矢用の弓オブジェクト

    [Header("描画設定")]
    [SerializeField] private float bowDistance = 0.5f;      // プレイヤーからの距離

    [Header("アニメーション設定")]
    [SerializeField] private string shootAnimationTrigger = "Shoot";

    [Header("参照スクリプト")]
    [SerializeField] private PlayerAttackManager attackManager;     // 武器攻撃管理スクリプト
    [SerializeField] private WeaponInventory weaponInventory;       // 武器インベントリ
    [SerializeField] private PlayerHPManager playerHPManager;     // プレイヤーHP管理スクリプト

    private Camera mainCamera;
    private GameObject currentBowObject;    // 現在表示中の弓オブジェクト
    private Vector3 playerPosition;         // プレイヤーの現在位置

    // 各弓のAnimator参照
    private Animator normalBowAnimator;
    private Animator boomBowAnimator;
    private Animator trapBowAnimator;

    private void Start()
    {
        mainCamera = Camera.main;

        // 参照の自動取得
        if (attackManager == null)
        {
            attackManager = GetComponent<PlayerAttackManager>();
            if (attackManager == null)
            {
                Debug.LogError("PlayerAttackManagerが見つかりません。");
            }
        }

        if (weaponInventory == null)
        {
            weaponInventory = GetComponent<WeaponInventory>();
            if (weaponInventory == null)
            {
                Debug.LogError("WeaponInventoryが見つかりません。");
            }
        }

        // 弓オブジェクトの初期設定
        InitializeBowObjects();

        // InitializeAnimators();
        InitializeAnimators();
    }

    private void Update()
    {
        // プレイヤーの現在位置を取得
        playerPosition = transform.position;

        // どの弓を表示すべきか判定
        UpdateBowVisibility();

        // 表示中の弓があれば、位置と回転を更新
        if (currentBowObject != null && currentBowObject.activeSelf)
        {
            UpdateBowPositionAndRotation();
        }
    }

    /// <summary>
    /// 弓オブジェクトの初期設定
    /// </summary>
    private void InitializeBowObjects()
    {
        // 通常の弓オブジェクトが設定されているか確認
        if (normalBowObject == null)
        {
            Debug.LogWarning("通常の弓オブジェクトが設定されていません。");
        }
        else
        {
            normalBowObject.SetActive(false);
        }

        // 爆弾矢の弓オブジェクトが設定されているか確認
        if (boomBowObject == null)
        {
            Debug.LogWarning("爆弾矢用の弓オブジェクトが設定されていません。");
        }
        else
        {
            boomBowObject.SetActive(false);
        }

        // トラップ矢の弓オブジェクトが設定されているか確認
        if (trapBowObject == null)
        {
            Debug.LogWarning("トラップ矢用の弓オブジェクトが設定されていません。");
        }
        else
        {
            trapBowObject.SetActive(false);
        }
    }

    /// <summary>
    /// 弓の表示状態を更新
    /// </summary>
    private void UpdateBowVisibility()
    {
        if (weaponInventory == null) return;

        GameObject targetBow = null;

        // 爆弾矢コンボが使える場合
        if (weaponInventory.GetCurrentCombo() == WeaponInventory.WeponComboType.BombBow)
        {
            targetBow = boomBowObject;
        }
        // トラップ矢コンボが使える場合
        else if (weaponInventory.GetCurrentCombo() == WeaponInventory.WeponComboType.TrapBow)
        {
            targetBow = trapBowObject;
        }
        // 通常の弓を持っている場合
        else if (weaponInventory.HasWeapon(WeaponInventory.WeaponType.Bow) &&
                 weaponInventory.GetCurrentCombo() == WeaponInventory.WeponComboType.None)
        {
            targetBow = normalBowObject;
        }

        // 弓の表示切り替え
        if (targetBow != currentBowObject)
        {
            // 現在の弓を非表示
            if (currentBowObject != null)
            {
                currentBowObject.SetActive(false);
            }

            // 新しい弓を表示
            currentBowObject = targetBow;
            if (currentBowObject != null)
            {
                currentBowObject.SetActive(true);
            }
        }
        else if (targetBow == null && currentBowObject != null)
        {
            // 弓を表示する必要がない場合は非表示
            currentBowObject.SetActive(false);
            currentBowObject = null;
        }
        //HPが０なら非表示
        else if(playerHPManager.IsDead)
        {
            currentBowObject.SetActive(false);
            currentBowObject = null;
        }
    }

    /// <summary>
    /// プレイヤー位置とマウス位置に応じて弓の位置と回転を更新
    /// </summary>
    private void UpdateBowPositionAndRotation()
    {
        if (currentBowObject == null || mainCamera == null) return;

        // マウスのワールド座標を取得
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // プレイヤーからマウスへの方向ベクトル
        Vector2 direction = (mousePos - playerPosition).normalized;

        // 角度を計算（度数法）
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 弓の位置を更新（プレイヤー位置を基準にマウス方向に配置）
        Vector3 bowPosition = playerPosition + (Vector3)(direction * bowDistance);
        bowPosition.z = 0f;  // Z座標を固定
        currentBowObject.transform.position = bowPosition;

        // 弓の回転を更新
        currentBowObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // 左向きの場合は弓を反転
        SpriteRenderer bowRenderer = currentBowObject.GetComponent<SpriteRenderer>();
        if (bowRenderer != null)
        {
            if (direction.x < 0)
            {
                bowRenderer.flipY = true;
            }
            else
            {
                bowRenderer.flipY = false;
            }
        }
    }

    /// <summary>
    /// 弓の距離を設定（外部から変更可能）
    /// </summary>
    public void SetBowDistance(float distance)
    {
        bowDistance = distance;
    }

    // 弓の攻撃アニメーションを再生
    public void PlayBowShootAnimation()
    {
        Animator currentAnimator = GetCurrentBowAnimator();

        if (currentAnimator != null)
        {
            currentAnimator.SetTrigger("Shoot");
            Debug.Log($"弓の攻撃アニメーションを再生: {currentBowObject.name}");
        }
        else
        {
            Debug.LogWarning("現在の弓のAnimatorが見つかりません。");
        }
    }

    // Animatorを取得するメソッド
    private void InitializeAnimators()
    {
        if (normalBowObject != null)
        {
            normalBowAnimator = normalBowObject.GetComponent<Animator>();
            if (normalBowAnimator == null)
            {
                Debug.LogWarning("通常の弓にAnimatorコンポーネントがありません。");
            }
        }

        if (boomBowObject != null)
        {
            boomBowAnimator = boomBowObject.GetComponent<Animator>();
            if (boomBowAnimator == null)
            {
                Debug.LogWarning("爆弾矢の弓にAnimatorコンポーネントがありません。");
            }
        }

        if (trapBowObject != null)
        {
            trapBowAnimator = trapBowObject.GetComponent<Animator>();
            if (trapBowAnimator == null)
            {
                Debug.LogWarning("トラップ矢の弓にAnimatorコンポーネントがありません。");
            }
        }
    }

    // 現在の弓のAnimatorを取得
    private Animator GetCurrentBowAnimator()
    {
        if (currentBowObject == normalBowObject)
        {
            return normalBowAnimator;
        }
        else if (currentBowObject == boomBowObject)
        {
            return boomBowAnimator;
        }
        else if (currentBowObject == trapBowObject)
        {
            return trapBowAnimator;
        }

        return null;
    }
}