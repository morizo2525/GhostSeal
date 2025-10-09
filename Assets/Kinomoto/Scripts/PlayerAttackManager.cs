using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    //�v���C���[�̍U���S�̂��Ǘ�����X�N���v�g

    [Header("��{�U���X�N���v�g")]
    public PlayerSwordAttack swordAttack;   //���U���i��{�U���j�X�N���v�g

    //[Header("�E���镐��̍U���X�N���v�g")]

    //��ōU���X�N���v�g���������A�}�[�W�������ۂɂ��̃X�N���v�g�̖��O�ɕύX
    //�O��ނ̍U���X�N���v�g�����O�ɏ�������K�v����i����g�ݍ��킹���j
    //��FPlayerBowAttack�̕������Y���X�N���v�g���ɕύX

    //public PlayerBowAttack bowAttack;       //�|�U���X�N���v�g
    //public PlayerBombAttack bombAttack;     //���e�U���X�N���v�g
    //public PlayerTrapAttack trapAttack;     //㩐ݒu�X�N���v�g

    [Header("����C���x���g���̃X�N���v�g")]
    public WeaponInventory weaponInventory;

    private void Start()
    {
        //WaponInventory�X�N���v�g�̎Q�Ƃ��擾
        if (weaponInventory == null)
        {
            weaponInventory = GetComponent<WeaponInventory>();
            if (weaponInventory == null)
            {
                Debug.LogError("WeaponInventory�X�N���v�g��������܂���B");
            }
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
            //��:�|�U���┚�e�ݒu�Ȃ�
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
        //�X�̏����̎������O��ނ̍U���X�N���v�g�����O�ɏ�������K�v����

        //switch (combo)
        //{
        //case WeaponInventory.WeponComboType.BombBow:
        //        ���e�{�|�̃R���{����: ���e��𔭎�
        //        if (bowAttack != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Bow))
        //        {
        //            bowAttack.ShootBombArrow(); //���e��𔭎�
        //        }
        //        break;
        //case WeaponInventory.WeponComboType.TrapBow:
        //    // �+�|�F�g���b�v�A���[
        //    if (bowAttack != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Bow))
        //    {
        //        bowAttack.TrapArrowAttack();
        //    }
        //    break;

        //case WeaponInventory.WeponComboType.BombTrap:
        //    // ���e+㩁F�n��
        //    if (trapAttack != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Trap))
        //    {
        //        trapAttack.LandmineAttack();
        //    }
        //    break;
        //}
    }

    //�P�̂̕�����g�p���郁�\�b�h
    void UseSingleWeapon()
    {
        //�X�̏����̎������O��ނ̍U���X�N���v�g�����O�ɏ�������K�v����

        //if (weaponInventory.HasWeapon(WeaponInventory.WeaponType.Bow))
        //{
        //    if (bowAttack != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Bow))
        //    {
        //        bowAttack.ShootArrow(); //�ʏ�̖�𔭎�
        //    }
        //}
        //else if (weaponInventory.HasWeapon(WeaponInventory.WeaponType.Bomb))
        //{
        //    if (bombAttack != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Bomb))
        //    {
        //        bombAttack.ThrowBomb(); //���e�𓊂���
        //    }
        //}
        //else if (weaponInventory.HasWeapon(WeaponInventory.WeaponType.Trap))
        //{
        //    if (trapAttack != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Trap))
        //    {
        //        trapAttack.PlaceTrap(); //㩂�ݒu
        //    }
        //}
    }

    // ����̎�ނɉ����čU�������s
    //void ExecuteWeaponAttack(WeaponInventory.WeaponType weaponType)
    //{
    //    switch (weaponType)
    //    {
    //        case WeaponInventory.WeaponType.Bow:
    //            if (bowAttack != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Bow))
    //            {
    //                bowAttack.NormalArrowAttack();
    //            }
    //            break;

    //        case WeaponInventory.WeaponType.Bomb:
    //            if (bombAttack != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Bomb))
    //            {
    //                bombAttack.PlaceBomb();
    //            }
    //            break;

    //        case WeaponInventory.WeaponType.Trap:
    //            if (trapAttack != null && weaponInventory.UseWeapon(WeaponInventory.WeaponType.Trap))
    //            {
    //                trapAttack.PlaceTrap();
    //            }
    //            break;
    //    }
    //}
}
