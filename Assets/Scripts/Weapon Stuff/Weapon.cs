using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
   // Start is called before the first frame update
   public WeaponType weaponType = WeaponType.None;
   public new string name = "Weapon";
   public Projectile projectile;
   public int currentAmmo;
   public int maxAmmo;

    public Weapon (WeaponType _weaponType) {
      weaponType = _weaponType;
   }

   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {

   }
}
