using UnityEngine;

public class TrapTest : MonoBehaviour
{
    [SerializeField] private PlayerTrapCreator trapCreator;
    [SerializeField] private PlayerTrapBowAttack bowWithTrap;
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // ƒgƒ‰ƒbƒv–î‚ğ”­Ë
            if (bowWithTrap != null)
            {
                bowWithTrap.BowShootTrapArrow();
            }
            else
            {
                Debug.LogError("PlayerAttack_BowWithTrap‚ªİ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñ");
            }
        }
    }
}