using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
   // Start is called before the first frame update
   public WeaponType weaponType = WeaponType.None;
   public new string name = "Weapon";
   public Projectile projectile;
   public GameObject projectilePrefab;
   public int currentAmmo;
   public int maxAmmo;
   [SerializeField]
   private Transform shootPos;
   public int bulletsPerShot = 5;
   public float bulletSpread = 2.0f;


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
            Instantiate(projectilePrefab, shootPos.position, shootPos.transform.rotation * Quaternion.Euler(new Vector3(Random.Range(-bulletSpread, bulletSpread), Random.Range(-bulletSpread, bulletSpread), Random.Range(-bulletSpread, bulletSpread))));
         }
      }
   }
}
