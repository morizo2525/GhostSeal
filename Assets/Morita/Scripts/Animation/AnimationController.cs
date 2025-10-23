using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animatorコンポーネントが見つかりません。");
        }
    }
    //  ------------プレイヤー用アニメーション制御関数------------

    // 待機
    public void PlayerIdleAnim()
    {
        SetBool("IsMoving", false);
    }

    // 左右移動
    public void PlayerBesideMoveAnim()
    {
        SetBool("IsMoving", true);
    }

    /// <summary>
    /// トリガー型のアニメーションを再生
    /// </summary>
    public void PlayAnimation(string triggerName)
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
    }

    /// <summary>
    /// Bool型のパラメータを設定
    /// </summary>
    public void SetBool(string paramName, bool value)
    {
        if (animator != null)
        {
            animator.SetBool(paramName, value);
        }
    }

    /// <summary>
    /// Int型のパラメータを設定
    /// </summary>
    public void SetInt(string paramName, int value)
    {
        if (animator != null)
        {
            animator.SetInteger(paramName, value);
        }
    }

    /// <summary>
    /// Float型のパラメータを設定
    /// </summary>
    public void SetFloat(string paramName, float value)
    {
        if (animator != null)
        {
            animator.SetFloat(paramName, value);
        }
    }
}
