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
   public ProjectileType projectileType;
   public float projectileSpeed;
   public float projectileDamage;
   public bool canAbsorb;
   public float RocketExplosionRadius;
   private float lifetime = 0;
   private float maxLifetime = 10;
   private Rigidbody rb;
   LayerMask ignore;

   void Start()
   {
      ignore = LayerMask.NameToLayer("Projectile");
      if (projectileType == ProjectileType.Projectile)
      {
         rb = gameObject.GetComponent<Rigidbody>();
      }
      Destroy(gameObject, maxLifetime);
      Vector3 direction = transform.forward;
      //direction.y = 0;
      if (projectileType == ProjectileType.Projectile)
      {
         rb.AddForce(direction * projectileSpeed, ForceMode.Force);
      }
   }
   void OnDrawGizmos() {
      Gizmos.color = Color.red;
      Gizmos.DrawSphere(transform.position, RocketExplosionRadius);
   }
   void OnCollisionEnter(Collision collision)
   {
      Debug.Log(collision.collider.name);
      if (collision.collider.gameObject.layer != ignore) {
         Collider[] hitColliders = Physics.OverlapSphere(transform.position, RocketExplosionRadius);
         foreach(Collider c in hitColliders) {
            if (c.gameObject.layer == 6) {
               LocalPlayerControllerState playerController = c.gameObject.GetComponent<LocalPlayerControllerState>();
               Vector3 away = c.gameObject.transform.position - gameObject.transform.position;
               playerController.Explode(away.normalized * 10);
            }
            //put the rest of the code here when enemies are done
         }
         Destroy(gameObject);
      }
   }
   public void OnHit()
   {

   }
   // Update is called once per frame
   void Update()
   {
      lifetime += Time.deltaTime;
   }
}
