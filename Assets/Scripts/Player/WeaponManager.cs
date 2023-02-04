using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum WeaponType
{
    None,
    Pistol,
    Shotgun,
    RocketLauncher,
    Sword
}
public class WeaponManager : MonoBehaviour
{
    private int loadoutSize;
    public List<WeaponType> currentTypes;
    public Weapon pistol;
    public Weapon shotgun;
    public Weapon rocketlauncher;
    public Weapon activeweapon = new Weapon(WeaponType.None);
    public PlayerInput playerInput;
    public Animator anim;
    private InputAction weapon1;
    private InputAction weapon2;
    private InputAction weapon3;
    private InputAction fire;
    private InputAction reload;

    void Start()
    {
        weapon1 = playerInput.actions["Pistol"];
        weapon2 = playerInput.actions["Shotgun"];
        weapon3 = playerInput.actions["RocketLauncher"];
        fire = playerInput.actions["Shoot"];
        reload = playerInput.actions["Reload"];
        activeweapon = pistol;

        pistol.weaponMan = this;
        shotgun.weaponMan = this;
        rocketlauncher.weaponMan = this;

        SelectWeapon(pistol);
    }

    // Update is called once per frame
    void Update()
    {
        if (weapon1.triggered)
        {
            SelectWeapon(pistol);
        }
        if (weapon2.triggered)
        {
            SelectWeapon(shotgun);
        }
        if (weapon3.triggered)
        {
            SelectWeapon(rocketlauncher);
        }
        if (fire.triggered)
        {
            Fire();
        }
        if (reload.triggered)
        {
            Reload();
        }
    }
    public void RegisterNewWeapon(Weapon weapon)
    {
        loadoutSize++;
        if (!currentTypes.Contains(weapon.weaponType))
        {
            currentTypes.Add(weapon.weaponType);
            return;
        }
        activeweapon.currentAmmo = activeweapon.maxAmmo;
    }

    public void SelectWeapon(Weapon weapon)
    {
        if (currentTypes.Contains(weapon.weaponType) && !weapon.Equals(activeweapon))
        {
            activeweapon?.gameObject.SetActive(false);
            activeweapon = weapon;
            activeweapon?.gameObject.SetActive(true);
            activeweapon?.onWeaponSelect?.Invoke();
        }
    }

    public void Reload()
    {
        activeweapon.Reload();
    }

    public void Fire()
    {
        activeweapon.Fire();
    }

    public void ReloadAllWeapons()
    {
        pistol.ReloadInstant();
        shotgun.ReloadInstant();
        rocketlauncher.ReloadInstant();
    }
}
