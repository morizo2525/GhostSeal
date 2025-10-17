using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    //�v���C���[�̍U���S�̂��Ǘ�����X�N���v�g

    [Header("��{�U���X�N���v�g")]
    public PlayerSwordAttack swordAttack;   //���U���i��{�U���j�X�N���v�g

    [Header("�E���镐��̍U���X�N���v�g")]
    public PlayerAttack_Bow bowAttack;       //�|�U���X�N���v�g
    public PlayerBombAttack bombAttack;      //���e�U���X�N���v�g
    public PlayerTrapCreator trapCreator;    //㩐ݒu�X�N���v�g

    [Header("�g�ݍ��킹�U���X�N���v�g")]
    public PlayerBoomBowAttack boomBowAttack; //������X�N���v�g
    public PlayerTrapBowAttack trapBowAttack; //�g���b�v��X�N���v�g

    [Header("����C���x���g���̃X�N���v�g")]
    public WeaponInventory weaponInventory;

    private void Start()
    {
        //WeaponInventory�X�N���v�g�̎Q�Ƃ��擾
        if (weaponInventory == null)
        {
            weaponInventory = GetComponent<WeaponInventory>();
            if (weaponInventory == null)
            {
                Debug.LogError("WeaponInventory�X�N���v�g��������܂���B");
            }
        }

        //�|�U���X�N���v�g�̎Q�Ƃ��擾
        if (bowAttack == null)
        {
            bowAttack = GetComponent<PlayerAttack_Bow>();
        }

        //���e�U���X�N���v�g�̎Q�Ƃ��擾
        if (bombAttack == null)
        {
            bombAttack = GetComponent<PlayerBombAttack>();
        }
        //㩐ݒu�X�N���v�g�̎Q�Ƃ��擾
        if (trapCreator == null)
        {
            trapCreator = GetComponent<PlayerTrapCreator>();
        }
    }

    void Update()
    {
        //���N���b�N�Ŋ�{�̌��U��
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (swordAttack != null)
            {
                swordAttack.SwordAttack(); //���U�����\�b�h���Ăяo��
            }
        }

        //�E�N���b�N�ŏE�������̕���U��
        //WeaponInventory����Q�Ƃ��ď��L�󋵂��m�F�A�X�N���v�g�̌Ăяo��������
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            UseWeapon();
        }
    }

    //�E����������g�p���郁�\�b�h
    void UseWeapon()
    {
        if (weaponInventory == null) return;

        WeaponInventory.WeponComboType combo = weaponInventory.GetCurrentCombo();

        //�R���{���ʁi2��̕�����������Ă���j������ꍇ
        if (combo != WeaponInventory.WeponComboType.None)
        {
            UseComboAttack(combo);
        }
        //�P�̂̕�����g�p����ꍇ
        else
        {
            UseSingleWeapon();
        }
    }

    //�R���{�U�����g�p���郁�\�b�h
    void UseComboAttack(WeaponInventory.WeponComboType combo)
    {
        switch (combo)
        {
            case WeaponInventory.WeponComboType.BombBow:
                //���e�{�|�̃R���{����: ���e��𔭎�
                if (boomBowAttack != null && weaponInventory.UseComboWeapon())
                {
                    boomBowAttack.ShootBoomArrow(); //���e��𔭎�
                    Debug.Log("���e��𔭎˂��܂����I");
                }
                break;

            case WeaponInventory.WeponComboType.TrapBow:
                //�+�|�F�g���b�v�A���[
                if (trapBowAttack != null && weaponInventory.UseComboWeapon())
                {
                    trapBowAttack.BowShootTrapArrow(); //�g���b�v�A���[�𔭎�
                    Debug.Log("�g���b�v�A���[�𔭎˂��܂����I");
                }
                break;

            case WeaponInventory.WeponComboType.BombTrap:
                //���e + 㩁F�n��
                if (trapCreator != null && weaponInventory.UseComboWeapon())
                {
                    trapCreator.CreateMineTrap();
                    Debug.Log("�n���g���b�v��ݒu���܂����I");
                }
                break;
        }
    }

    //�P�̂̕�����g�p���郁�\�b�h
    void UseSingleWeapon()
    {
        // �D�揇�ʁF�| > ���e > �
        if (weaponInventory.HasWeapon(WeaponInventory.WeaponType.Bow))
        {
            if (bowAttack != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Bow))
            {
                bowAttack.BowShoot(); //�ʏ�̖�𔭎�
                Debug.Log("�ʏ�̖�𔭎˂��܂���");
            }
        }
        else if (weaponInventory.HasWeapon(WeaponInventory.WeaponType.Bomb))
        {
            if (bombAttack != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Bomb))
            {
                bombAttack.ThrowBomb(); //���e��P�̂œ�����
                Debug.Log("���e�𓊝����܂���");
            }
        }
        else if (weaponInventory.HasWeapon(WeaponInventory.WeaponType.Trap))
        {
            if (trapCreator != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Trap))
            {
                trapCreator.CreateTrap(); //㩂�ݒu
            }
        }
        else
        {
            Debug.Log("�g�p�\�ȕ��킪����܂���");
        }
    }
}