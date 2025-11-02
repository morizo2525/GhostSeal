using UnityEngine;

public class PlayerDethHandler : MonoBehaviour
{
    [SerializeField] private PlayerHPManager          playerHPManager;
    [SerializeField] private GameOverCanvasController gameOverController;
    [SerializeField] private string gameOverSceneName = "GameOverScene";

    private AnimationController animController;
    private MonoBehaviour[] PlayerMove;
    private MonoBehaviour[] PlayerSwordAttack;
    private MonoBehaviour[] PlayerAttackManager;

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

        if (gameOverController == null)
        {
            gameOverController = GetComponent<GameOverCanvasController>();
        }

        // プレイヤーの操作スクリプトを取得
        PlayerMove          = GetComponents<MonoBehaviour>();
        PlayerSwordAttack   = GetComponents<MonoBehaviour>();
        PlayerAttackManager = GetComponents<MonoBehaviour>();

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

        // 死亡アニメーション再生
        animController.PlayerDeathAnim();

    }

    // ゲームオーバー時のスプライト表示(アニメーションイベントから呼び出される)
    public void ShowGameOverSprite()
    {
        if (gameOverController != null)
        {
            gameOverController.ShowGameOver();
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
        var playerController = GetComponent<PlayerMove>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        var playerSwordAttack = GetComponent<PlayerSwordAttack>();
        if (playerSwordAttack != null)
        {
            playerSwordAttack.enabled = false;
        }

        var playerAttackManager = GetComponent<PlayerAttackManager>();
        if (playerAttackManager != null)
        {
            playerAttackManager.enabled = false;
        }
    }
}
