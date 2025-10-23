using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator�R���|�[�l���g��������܂���B");
        }
    }
    //  ------------�v���C���[�p�A�j���[�V��������֐�------------

    // �ҋ@
    public void PlayerIdleAnim()
    {
        SetBool("IsMoving", false);
    }

    // ���E�ړ�
    public void PlayerBesideMoveAnim()
    {
        SetBool("IsMoving", true);
    }

    /// <summary>
    /// �g���K�[�^�̃A�j���[�V�������Đ�
    /// </summary>
    public void PlayAnimation(string triggerName)
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
    }

    /// <summary>
    /// Bool�^�̃p�����[�^��ݒ�
    /// </summary>
    public void SetBool(string paramName, bool value)
    {
        if (animator != null)
        {
            animator.SetBool(paramName, value);
        }
    }

    /// <summary>
    /// Int�^�̃p�����[�^��ݒ�
    /// </summary>
    public void SetInt(string paramName, int value)
    {
        if (animator != null)
        {
            animator.SetInteger(paramName, value);
        }
    }

    /// <summary>
    /// Float�^�̃p�����[�^��ݒ�
    /// </summary>
    public void SetFloat(string paramName, float value)
    {
        if (animator != null)
        {
            animator.SetFloat(paramName, value);
        }
    }
}
