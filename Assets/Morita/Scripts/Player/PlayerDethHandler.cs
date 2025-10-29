using UnityEngine;

public class PlayerDethHandler : MonoBehaviour
{
    [SerializeField] private PlayerHPManager playerHPManager;
    [SerializeField] private string gameOverSceneName = "GameOverScene";

    private GameObject          gameOverCanvas; // GameOver専用Canvas
    private AnimationController animController;
    private MonoBehaviour[] PlayerMove;
    private void Awake()
    {
        if (playerHPManager == null)
        {
            playerHPManager = GetComponent<PlayerHPManager>();
        }

        if (animController == null)
        {
            animController = GetComponent<AnimationController>();
        }

        if (gameOverCanvas == null)
        {
            gameOverCanvas = GameObject.Find("GameOverCanvas");
            if (gameOverCanvas == null)
            {
                Debug.LogError("GameOver_Canvasが見つかりません。シーンに配置してください。");
            }
        }

        // プレイヤーの操作スクリプトを取得
        PlayerMove = GetComponents<MonoBehaviour>();

        // GameOver Canvasを初期状態で非表示
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }

        // HP管理スクリプトの死亡イベントに登録
        playerHPManager.PlayerDied += OnPlayerDie;
    }

    private void OnDestroy()
    {
        // 忘れずイベント解除
        if (playerHPManager != null)
            playerHPManager.PlayerDied -= OnPlayerDie;
    }

    private void OnPlayerDie()
    {
        // プレイヤーの操作を無効化
        DisablePlayerControl();

        // 判定無効化のためレイヤーを切り替える
        gameObject.layer = LayerMask.NameToLayer("DeadPlayer");

        // 死亡アニメーション再生
        animController.PlayerDeathAnim();
    }

    // ゲームオーバー時のスプライト表示(アニメーションイベントから呼び出される)
    public void ShowGameOverSprite()
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }
    }

    // シーン遷移(アニメーションイベントから呼び出される)
    public void LoadGameOverScene()
    {
        SceneTransition.LoadScene(gameOverSceneName);
    }

    // プレイヤーの操作を無効化
    private void DisablePlayerControl()
    {
        // 方法1: 特定のコンポーネントを無効化（推奨）
        // プロジェクトの操作スクリプト名に合わせて変更してください
        var playerController = GetComponent<PlayerMove>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }
}
