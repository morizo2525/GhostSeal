using UnityEngine;

public class TrapTest : MonoBehaviour
{
    [SerializeField] private PlayerTrapCreator trapCreator;
    [SerializeField] private PlayerTrapBowAttack bowWithTrap;
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // �g���b�v��𔭎�
            if (bowWithTrap != null)
            {
                bowWithTrap.BowShootTrapArrow();
            }
            else
            {
                Debug.LogError("PlayerAttack_BowWithTrap���ݒ肳��Ă��܂���");
            }
        }
    }
}