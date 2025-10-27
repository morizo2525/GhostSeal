using UnityEngine;

public class PlayerDethHandler : MonoBehaviour
{
    [SerializeField] private PlayerHPManager playerHPManager;
    [SerializeField] private string gameOverSceneName = "GameOverScene";

    private void Awake()
    {
        if (playerHPManager == null)
        {
            playerHPManager = GetComponent<PlayerHPManager>();
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
        // SceneTransitionでシーン遷移
        SceneTransition.LoadScene("GameOverScene");
    }
}
