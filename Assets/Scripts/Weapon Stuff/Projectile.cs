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
   public LayerMask ignore;
   private LineRenderer hitScanLine;

   void Start()
   {
      Physics.IgnoreLayerCollision(7, 8);
      Physics.IgnoreLayerCollision(7, 6);
      Physics.IgnoreLayerCollision(7, 7);
      hitScanLine = GetComponent<LineRenderer>();
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
      if (projectileType == ProjectileType.Hitscan) {
         Vector3[] positions = new Vector3[2];
         positions[0] = transform.position;
         positions[1] = transform.position + transform.forward * 10;
         hitScanLine.SetPositions(positions);
         Destroy(gameObject, .1f);
      }
   }
   void OnDrawGizmos() {
      Gizmos.color = Color.red;
      Gizmos.DrawSphere(transform.position, RocketExplosionRadius);
   }
   void OnCollisionEnter(Collision collision)
   {
      //https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html
      
      if (!(ignore == (ignore | (1<< collision.collider.gameObject.layer)))) {
         Collider[] hitColliders = Physics.OverlapSphere(transform.position, RocketExplosionRadius);
         foreach(Collider c in hitColliders) {
            if (c.gameObject.layer == 6) {
               LocalPlayerControllerState playerController = c.gameObject.GetComponent<LocalPlayerControllerState>();
               Vector3 away = c.gameObject.transform.position - gameObject.transform.position;
               playerController.Explode(away.normalized * 10);
            }
            if (c.gameObject.layer == 8) {
              c.gameObject.GetComponent<EnemyTraits>()?.TakeDamage((int)projectileDamage);
            }
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
