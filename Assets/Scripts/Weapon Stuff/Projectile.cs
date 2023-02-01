using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{
   Hitscan,
   Projectile
}

public class Projectile : MonoBehaviour
{
   public ProjectileType projectilyType = ProjectileType.Hitscan;
   public float projectileSpeed;
   public float projectileDamage;
   public bool canAbsorb;


   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {

   }
}
