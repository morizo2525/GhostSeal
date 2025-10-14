using UnityEngine;

public class BombTest : MonoBehaviour
{
    [Header("�e�X�g�Ώ�")]
    public PlayerBombAttack bombAttack;     // ���e�U���X�N���v�g
    public PlayerAttack_Bow bowAttack;      // �|�U���X�N���v�g
    public PlayerBoomBowAttack boomBowAttack;   // ������U���X�N���v�g

    [Header("�e�X�g�ݒ�")]
    public KeyCode bombKey = KeyCode.B;     // ���e�����L�[
    public KeyCode arrowKey = KeyCode.A;    // ��˃L�[
    public KeyCode boomBowKey = KeyCode.X;      // ������˃L�[

    [Header("�f�o�b�O���")]
    public bool showDebugInfo = true;       // �f�o�b�O����\��

    void Update()
    {
        // ���e�����e�X�g
        if (Input.GetKeyDown(bombKey))
        {
            TestBombThrow();
        }

        // ��˃e�X�g�i��r�p�j
        if (Input.GetKeyDown(arrowKey))
        {
            TestArrowShoot();
        }

        // ������˃e�X�g
        if (Input.GetKeyDown(boomBowKey))
        {
            TestBoomBowShoot();
        }
    }

    void TestBombThrow()
    {
        if (bombAttack == null)
        {
            Debug.LogError("PlayerBombAttack���ݒ肳��Ă��܂���I");
            return;
        }

        if (showDebugInfo)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log($"[���e�e�X�g] �}�E�X�ʒu: {mousePos}, ����: {Time.time:F2}�b");
        }

        bombAttack.ThrowBomb();
    }

    void TestArrowShoot()
    {
        if (bowAttack == null)
        {
            Debug.LogWarning("PlayerAttack_Bow���ݒ肳��Ă��܂���");
            return;
        }

        if (showDebugInfo)
        {
            Debug.Log($"[��e�X�g] ��𔭎�, ����: {Time.time:F2}�b");
        }

        bowAttack.BowShoot();
    }

    void TestBoomBowShoot()
    {
        if (boomBowAttack == null)
        {
            Debug.LogError("PlayerBoomBowAttack���ݒ肳��Ă��܂���I");
            return;
        }

        if (showDebugInfo)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log($"[������e�X�g] �}�E�X�ʒu: {mousePos}, ����: {Time.time:F2}�b");
        }

        boomBowAttack.ShootBoomArrow();
    }
}