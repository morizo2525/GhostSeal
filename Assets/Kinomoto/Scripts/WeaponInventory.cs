using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    //�E���镐��̎�ނ��`
    public enum WeaponType
    {
        None,
        Bow,    //�|
        Bomb,   //���e
        Trap    //�
    }

    //����̑g�ݍ��킹�ɂ�������ʂ��`
    public enum WeponComboType
    {
        None,           //�Ȃ�
        BombBow,        //���e�{�|
        TrapBow,        //㩁{�|
        BombTrap,       //���e�{�
    }

    [Header("���L����i�ő�2��ށj")]
    public WeaponType weaponSlot1 = WeaponType.None; //����X���b�g1
    public WeaponType weaponSlot2 = WeaponType.None; //����X���b�g2

    [Header("����̎c�i��")]
    public int bowAmmo = 0;     //�|�̖�̎c��
    public int bombAmmo = 0;    //���e�̎c��
    public int trapAmmo = 0;    //㩂̎c��
    public int maxAmmo = 10;    //�e����̍ő�c��

    [Header("���݂̃R���{�i�ǂݎ���p�j")]
    [SerializeField] private WeponComboType currentCombo = WeponComboType.None; //���݂̕���R���{

    // Inspector��ł̕ύX�����m���邽�߂̕ϐ�
    private WeaponType previousSlot1 = WeaponType.None;
    private WeaponType previousSlot2 = WeaponType.None;

    void Start()
    {
        UpdateWeaponCombo(); //���������ɕ���R���{���X�V
        previousSlot1 = weaponSlot1;
        previousSlot2 = weaponSlot2;
    }

    void Update()
    {
        // Inspector��ŃX���b�g���ύX���ꂽ���`�F�b�N
        if (weaponSlot1 != previousSlot1 || weaponSlot2 != previousSlot2)
        {
            UpdateWeaponCombo();
            previousSlot1 = weaponSlot1;
            previousSlot2 = weaponSlot2;
        }
    }

    //������E�����\�b�h
    public bool PikcupWeapon(WeaponType weaponType, int ammo)
    {
        if (weaponType == WeaponType.None)
        {
            Debug.LogWarning("�����ȕ���^�C�v�ł��B");
            return false;
        }

        //���ɓ�������������Ă���ꍇ�́A�e���ǉ�
        if (weaponSlot1 == weaponType || weaponSlot2 == weaponType)
        {
            AddAmmo(weaponType, ammo);
            Debug.Log(weaponType + "�̒e���ǉ����܂����B���݂̎c��: " + GetAmmo(weaponType));
            return true;
        }

        //����X���b�g�P���J���Ă���ꍇ
        if (weaponSlot1 == WeaponType.None)
        {
            weaponSlot1 = weaponType;
            AddAmmo(weaponType, ammo);
            UpdateWeaponCombo();
            previousSlot1 = weaponSlot1;
            Debug.Log(weaponType + "���X���b�g1�ɑ������܂����B");
            return true;
        }

        //����X���b�g�Q���J���Ă���ꍇ
        if (weaponSlot2 == WeaponType.None)
        {
            weaponSlot2 = weaponType;
            AddAmmo(weaponType, ammo);
            UpdateWeaponCombo();
            previousSlot2 = weaponSlot2;
            Debug.Log(weaponType + "���X���b�g2�ɑ������܂����B");
            return true;
        }

        //�����̃X���b�g�����܂��Ă���ꍇ
        Debug.Log("����X���b�g�����t�ł��B");
        return false;
    }

    private void AddAmmo(WeaponType weaponType, int ammo)
    {
        switch (weaponType)
        {
            case WeaponType.Bow:
                bowAmmo = Mathf.Min(bowAmmo + ammo, maxAmmo);
                break;
            case WeaponType.Bomb:
                bombAmmo = Mathf.Min(bombAmmo + ammo, maxAmmo);
                break;
            case WeaponType.Trap:
                trapAmmo = Mathf.Min(trapAmmo + ammo, maxAmmo);
                break;
        }
    }

    //�R���{������g�p�i�����̒e�������j
    public bool UseComboWeapon()
    {
        if (currentCombo == WeponComboType.None)
        {
            Debug.Log("�R���{���킪��������Ă��܂���B");
            return false;
        }

        switch (currentCombo)
        {
            case WeponComboType.BombBow:
                if (bombAmmo > 0 && bowAmmo > 0)
                {
                    bombAmmo--;
                    bowAmmo--;
                    CheckWeaponEmpty(WeaponType.Bomb);
                    CheckWeaponEmpty(WeaponType.Bow);
                    Debug.Log("���e����g�p���܂����B�c�� - ���e: " + bombAmmo + " ��: " + bowAmmo);
                    return true;
                }
                break;

            case WeponComboType.TrapBow:
                if (trapAmmo > 0 && bowAmmo > 0)
                {
                    trapAmmo--;
                    bowAmmo--;
                    CheckWeaponEmpty(WeaponType.Trap);
                    CheckWeaponEmpty(WeaponType.Bow);
                    Debug.Log("�g���b�v�A���[���g�p���܂����B�c�� - �: " + trapAmmo + " ��: " + bowAmmo);
                    return true;
                }
                break;

            case WeponComboType.BombTrap:
                if (bombAmmo > 0 && trapAmmo > 0)
                {
                    bombAmmo--;
                    trapAmmo--;
                    CheckWeaponEmpty(WeaponType.Bomb);
                    CheckWeaponEmpty(WeaponType.Trap);
                    Debug.Log("�n�����g�p���܂����B�c�� - ���e: " + bombAmmo + " �: " + trapAmmo);
                    return true;
                }
                break;
        }

        Debug.Log("�R���{����̒e�򂪕s�����Ă��܂��B");
        return false;
    }

    //����̎d�l�i�e��̏���j
    public bool UseWeapon(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Bow:
                if (bowAmmo > 0)
                {
                    bowAmmo--;
                    CheckWeaponEmpty(weaponType);
                    Debug.Log("�|���g�p���܂����B�c��̖�: " + bowAmmo);
                    return true;
                }
                break;

            case WeaponType.Bomb:
                if (bombAmmo > 0)
                {
                    bombAmmo--;
                    CheckWeaponEmpty(weaponType);
                    Debug.Log("���e���g�p���܂����B�c��̔��e: " + bombAmmo);
                    return true;
                }
                break;
            case WeaponType.Trap:
                if (trapAmmo > 0)
                {
                    trapAmmo--;
                    CheckWeaponEmpty(weaponType);
                    Debug.Log("㩂�ݒu���܂����B�c����: " + trapAmmo);
                    return true;
                }
                break;
        }
        Debug.Log("�e�򂪕s�����Ă��܂��B");
        return false;
    }

    //�e�򂪐s��������𕐊�X���b�g����폜
    private void CheckWeaponEmpty(WeaponType weaponType)
    {
        int ammo = GetAmmo(weaponType);

        if (ammo <= 0)
        {
            if (weaponSlot1 == weaponType)
            {
                weaponSlot1 = WeaponType.None;
                previousSlot1 = WeaponType.None;
            }
            else if (weaponSlot2 == weaponType)
            {
                weaponSlot2 = WeaponType.None;
                previousSlot2 = WeaponType.None;
            }
            UpdateWeaponCombo();
        }
    }

    //�����̃X���b�g�����܂��Ă���ꍇ�̑g�ݍ��킹����
    private void UpdateWeaponCombo()
    {
        if (weaponSlot1 != WeaponType.None && weaponSlot2 != WeaponType.None)
        {
            if ((weaponSlot1 == WeaponType.Bomb && weaponSlot2 == WeaponType.Bow) ||
                (weaponSlot1 == WeaponType.Bow && weaponSlot2 == WeaponType.Bomb))
            {
                currentCombo = WeponComboType.BombBow;
                Debug.Log("���e�{�|�̃R���{����:���e������I");
            }
            else if ((weaponSlot1 == WeaponType.Trap && weaponSlot2 == WeaponType.Bow) ||
                     (weaponSlot1 == WeaponType.Bow && weaponSlot2 == WeaponType.Trap))
            {
                currentCombo = WeponComboType.TrapBow;
                Debug.Log("㩁{�|�̃R���{����:�g���b�v�A���[�������I");
            }
            else if ((weaponSlot1 == WeaponType.Bomb && weaponSlot2 == WeaponType.Trap) ||
                     (weaponSlot1 == WeaponType.Trap && weaponSlot2 == WeaponType.Bomb))
            {
                currentCombo = WeponComboType.BombTrap;
                Debug.Log("���e�{㩂̃R���{����:�n���������I");
            }
            else
            {
                currentCombo = WeponComboType.None;
            }
        }
        else
        {
            currentCombo = WeponComboType.None;
        }
    }

    //���݂̑g�ݍ��킹�i�R���{�j���ʂ��擾
    public WeponComboType GetCurrentCombo()
    {
        return currentCombo;
    }

    //����̏��L�󋵂��擾
    public bool HasWeapon(WeaponType weaponType)
    {
        return weaponSlot1 == weaponType || weaponSlot2 == weaponType;
    }

    //�e��̐����擾
    public int GetAmmo(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Bow:
                return bowAmmo;
            case WeaponType.Bomb:
                return bombAmmo;
            case WeaponType.Trap:
                return trapAmmo;
            default:
                return 0;
        }
    }

    //�X���b�g���J���Ă��邩���m�F
    public bool HasEmptySlot()
    {
        return weaponSlot1 == WeaponType.None || weaponSlot2 == WeaponType.None;
    }
}