using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GenericEnemyController : MonoBehaviour
{
    public GameObject Target;
    public NavMeshAgent navMeshAgent;
    public GameObject weapon;
    public Weapon enemyWeapon;
    public Animator anim;
    public GameObject gibs;
    public FieldOfView eyes;
    public Action onEnemyDied;


    public float distanceToTarget;
    public float followDistance = 10f;
    public float fireDistance = 10f;
    public float lookSpeed = 5;
    public float waitTime = 1f;
    public bool aggroed = false;
    private float fireWait = 1f;

    private float currentSpeed;
    private Vector3 previousPosition;

    void Start()
    {
        Target = GameObject.Find("Player");
        eyes = GetComponent<FieldOfView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(eyes.CanSeePlayer() && !aggroed)
        {
            aggroed = true;
        }

        if (aggroed)
        {
            distanceToTarget = Mathf.Abs((Target.transform.position - gameObject.transform.position).magnitude);
            if (distanceToTarget > followDistance)
            {
                navMeshAgent.SetDestination(Target.transform.position);
                FaceTarget();

                if(distanceToTarget <= fireDistance)
                {
                    FaceTarget();
                    if (fireWait <= 0)
                    {
                        enemyWeapon.Fire();
                        fireWait = waitTime;
                    }
                    fireWait -= Time.deltaTime;
                }
                
            }
            else
            {

                navMeshAgent.SetDestination(gameObject.transform.position);
                FaceTarget();
                if (fireWait <= 0)
                {
                    enemyWeapon.Fire();
                    fireWait = waitTime;
                }
                fireWait -= Time.deltaTime;

            }

            Vector3 curMove = transform.position - previousPosition;
            currentSpeed = curMove.magnitude / Time.deltaTime;
            previousPosition = transform.position;

            anim.SetFloat("Speed", currentSpeed);
        }
        
    }
    private void FaceTarget()
    {
        //Vector3 lookPos = Target.transform.position - weapon.transform.position;
        //lookPos.y = 0;
        //Quaternion rotation = Quaternion.LookRotation(lookPos);
        //weapon.transform.rotation = Quaternion.Slerp(weapon.transform.rotation, rotation, lookSpeed);
        //this.transform.LookAt(Target.transform.position);

        var lookPos = Target.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * lookSpeed);
    }

    public void Die()
    {
        onEnemyDied?.Invoke();
        var c = GameObject.Instantiate(gibs, this.transform.position, this.transform.rotation);
        Destroy(c, 10f);
        Destroy(gameObject);
    }
}
