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

   public void Fire() {
      Debug.Log("bang");
      if (projectile.projectileType == ProjectileType.Projectile) {
         Instantiate(projectilePrefab, shootPos.position, shootPos.transform.rotation);
      }
      if (projectile.projectileType == ProjectileType.Hitscan) {
         
      }
   }
}
