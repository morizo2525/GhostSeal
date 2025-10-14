using UnityEngine;

public class TrapTest : MonoBehaviour
{
    [SerializeField] private PlayerTrapCreator trapCreator;

    private GameObject spawnedEnemy;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (trapCreator != null)
            {
                trapCreator.CreateTrap();
            }
            else
            {
                Debug.LogError("PlayerTrapCreator‚ªİ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñ");
            }
        }
    }
}