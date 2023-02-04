using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Global")]
    public WeaponType weaponType = WeaponType.None;
    public new string name = "Weapon";
    public Projectile projectile;
    public GameObject projectilePrefab;
    public int currentAmmo;
    public int maxAmmo;
    public float reloadTime = 1f;
    public bool reloading;
    private float reloadWait;
    public float fireDelay = 1;
    private float fireWait = 0;
    [SerializeField]
    private Transform shootPos;
    [Header("Shotgun Only")]
    public int bulletsPerShot = 5;
    public float bulletSpread = 2.0f;
    public Action onWeaponSelect;
    public string animPullUpName;

    [HideInInspector]
    public WeaponManager weaponMan;

    private void Start()
    {
        onWeaponSelect += WeaponSelected;
    }

    private void OnDestroy()
    {
        onWeaponSelect -= WeaponSelected;
    }


    void Update()
    {
        if (fireWait <= fireDelay)
        {
            fireWait += Time.deltaTime;
        }
        if (reloading && reloadWait >= 0)
        {
            reloadWait -= Time.deltaTime;
        }
        if (reloadWait <= 0)
        {
            currentAmmo = maxAmmo;
            reloading = false;
            reloadWait = reloadTime;
        }
    }

    public Weapon(WeaponType _weaponType)
    {
        weaponType = _weaponType;
    }

    public bool Equals(Weapon weapon)
    {
        if (this.weaponType == weapon.weaponType)
        {
            return true;
        }
        return false;
    }

    public void Fire()
    {
        if (fireWait <= fireDelay) return;
        if (currentAmmo <= 0) return;
        if (weaponType == WeaponType.RocketLauncher)
        {
            Instantiate(projectilePrefab, shootPos.position, shootPos.transform.rotation);
        }
        if (weaponType == WeaponType.Pistol)
        {
            Instantiate(projectilePrefab, shootPos.position, shootPos.transform.rotation);
        }
        if (weaponType == WeaponType.Shotgun)
        {
            for (int i = 0; i < bulletsPerShot; i++)
            {
                Instantiate(projectilePrefab, shootPos.position, shootPos.transform.rotation * Quaternion.Euler(new Vector3(UnityEngine.Random.Range(-bulletSpread, bulletSpread), UnityEngine.Random.Range(-bulletSpread, bulletSpread), UnityEngine.Random.Range(-bulletSpread, bulletSpread))));
            }
        }
        fireWait = 0;
        currentAmmo--;
        weaponMan?.anim?.SetTrigger("Fire");
    }

    public void WeaponSelected()
    {
        weaponMan.anim.Play(animPullUpName);
    }

    public void Reload()
    {
        reloading = true;
        reloadWait = reloadTime;
        weaponMan?.anim?.SetTrigger("Reload");
    }

    public void ReloadInstant()
    {
        currentAmmo = maxAmmo;
    }

}
