using UnityEngine;

public class TrapTest : MonoBehaviour
{
    [SerializeField] private PlayerTrapCreator trapCreator;
    [SerializeField] private PlayerTrapBowAttack bowWithTrap;
    [SerializeField] private BoomTrap boomTrap;
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

        //�n���g���b�v�𐶐�
        if (Input.GetKeyDown(KeyCode.G))
        {
            // �����ɒn���g���b�v�𐶐�
            if (trapCreator != null)
            {
                trapCreator.CreateMineTrap();
            }
            else
            {
                Debug.LogError("PlayerTrapCreator���ݒ肳��Ă��܂���");
            }
        }

        //�m�F�������Ƃ��ɃR�����g���O��
        if (Input.GetKeyDown(KeyCode.T))
        {
            // �����Ƀg���b�v�𐶐�
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