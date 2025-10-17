using UnityEngine;

public class PlayerDamageChecker : MonoBehaviour
{
    [Header("�GLayer")]
    [SerializeField] private LayerMask enemyLayer;

    [Header("�_���[�W��")]
    [SerializeField] private int damage = 1;

    [Header("�A���_���[�W�h�~")]
    [SerializeField] private float invincibleTime = 0.5f;

    [Header("Box����")]
    [SerializeField] private Vector2 boxSize = new Vector2(1f, 1f);
    [SerializeField] private Vector2 boxOffset = Vector2.zero;

    [Header("�㉺Circle����")]
    [SerializeField] private float circleRadius = 0.5f;
    [SerializeField] private float circleYOffset = 0.5f;

    [Header("�v���C���[�ړ��X�N���v�g")]
    [SerializeField] private PlayerMove playerMove;

    private PlayerHPManager playerHPManager;
    private float invincibleTimer = 0f;
    private bool isTouchingEnemy = false;

    private void Start()
    {
        playerHPManager = GetComponent<PlayerHPManager>();
        if (playerMove == null) playerMove = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        // ����𒆂ł� invincibleTimer �͕K�����炷
        if (invincibleTimer > 0f) invincibleTimer -= Time.deltaTime;

        Vector2 pos = (Vector2)transform.position;
        float horizontalSign = 1f;
        if (playerMove != null) horizontalSign = Mathf.Sign(playerMove.transform.localScale.x);

        Vector2 currentBoxOffset = new Vector2(boxOffset.x * horizontalSign, boxOffset.y);
        Vector2 topCirclePos = pos + currentBoxOffset + Vector2.up * circleYOffset;
        Vector2 bottomCirclePos = pos + currentBoxOffset + Vector2.down * circleYOffset;

        bool hitDetected = Physics2D.OverlapBox(pos + currentBoxOffset, boxSize, 0f, enemyLayer);
        hitDetected |= Physics2D.OverlapCircle(topCirclePos, circleRadius, enemyLayer);
        hitDetected |= Physics2D.OverlapCircle(bottomCirclePos, circleRadius, enemyLayer);

        // �ڐG����ƃ_���[�W����
        if (hitDetected)
        {
            if (!isTouchingEnemy) { Debug.Log("�G�ƐڐG�J�n�I"); isTouchingEnemy = true; }

            if ((playerMove == null || !playerMove.isInvincible) && invincibleTimer <= 0f)
            {
                playerHPManager.TakeDamage(damage);
                invincibleTimer = invincibleTime;
                Debug.Log("�� �_���[�W�����I �c��HP�X�V");
            }
        }
        else
        {
            if (isTouchingEnemy) { Debug.Log("�G�Ƃ̐ڐG���������ꂽ�B"); isTouchingEnemy = false; }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector2 pos = (Vector2)transform.position;
        float horizontalSign = 1f;
        if (playerMove != null) horizontalSign = Mathf.Sign(playerMove.transform.localScale.x);

        Vector2 currentBoxOffset = new Vector2(boxOffset.x * horizontalSign, boxOffset.y);
        Vector2 topCirclePos = pos + currentBoxOffset + Vector2.up * circleYOffset;
        Vector2 bottomCirclePos = pos + currentBoxOffset + Vector2.down * circleYOffset;

        Gizmos.color = isTouchingEnemy ? Color.red : Color.green;
        Gizmos.DrawWireCube(pos + currentBoxOffset, boxSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(topCirclePos, circleRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(bottomCirclePos, circleRadius);
    }
#endif
}
