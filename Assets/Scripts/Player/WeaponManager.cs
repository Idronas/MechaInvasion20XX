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
   public Weapon activeweapon = null;
   public PlayerInput playerInput;
   private InputAction weapon1;
   private InputAction weapon2;
   private InputAction weapon3;

   void Start()
   {
      weapon1 = playerInput.actions["Pistol"];
      weapon2 = playerInput.actions["Shotgun"];
      weapon3 = playerInput.actions["RocketLauncher"];
   }

   // Update is called once per frame
   void Update()
   {
      if (weapon1.triggered)
      {
         SelectWeapon(new Weapon(WeaponType.Pistol));
      }
      if (weapon2.triggered)
      {
         SelectWeapon(new Weapon(WeaponType.Shotgun));
      }
      if (weapon3.triggered)
      {
         SelectWeapon(new Weapon(WeaponType.RocketLauncher));
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
      if (currentTypes.Contains(weapon.weaponType) && weapon != activeweapon)
      {
         activeweapon?.gameObject.SetActive(false);
         activeweapon = weapon;
         activeweapon?.gameObject.SetActive(true);
      }
   }
}
