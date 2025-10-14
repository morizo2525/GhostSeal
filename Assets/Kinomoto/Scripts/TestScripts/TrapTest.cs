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
                Debug.LogError("PlayerTrapCreator���ݒ肳��Ă��܂���");
            }
        }
    }
}