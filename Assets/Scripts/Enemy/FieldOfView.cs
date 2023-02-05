using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public List<EnemyTarget> targets = new List<EnemyTarget>();

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    private float waitTime = 1f;
    private float currentWaitTime = 0f;

    private bool gotPlayers = false;

    private void Update()
    {
        if (!gotPlayers)
        {
            currentWaitTime += Time.deltaTime;

            if (currentWaitTime >= waitTime)
            {
                Debug.Log("adding player ;)");

                EnemyTarget targ = new EnemyTarget(GetComponent<GenericEnemyController>().Target);
                targets.Add(targ);

                if (targets.Count > 0)
                {
                    StartCoroutine(FOVRoutine());
                }

                gotPlayers = true;
                currentWaitTime = 0;
            }

        }
       
    }

    public bool CanSeePlayer()
    {
        bool val = false;
        foreach (EnemyTarget target in targets)
        {
            if (target.visible)
            {
                val = true;
            }
        }
        return val;
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;

            foreach (EnemyTarget target in targets)
            {
                FieldOfViewCheck(target);
            }
        }
    }

    public List<GameObject> GetAllVisiblePlayers()
    {
        List<GameObject> objs = new List<GameObject>();

        foreach (EnemyTarget target in targets)
        {
            if (target.visible)
            {
                objs.Add(target.target);
            }
        }

        return objs;
    }

    public EnemyTarget GetTargetFromPlayer(LocalPlayerControllerState _player)
    {
        EnemyTarget val = null;

        foreach(EnemyTarget targ in targets)
        {
            if(targ.target.GetComponent<LocalPlayerControllerState>() == _player)
            {
                val = targ;
            }
        }

        return val;
    }

    public void ResetTargetList()
    {
        targets.Clear();

        gotPlayers = false;
    }

    private void FieldOfViewCheck(EnemyTarget _target)
    {

        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {

            for (int i = 0; i < rangeChecks.Length; i++)
            {
                Transform target = rangeChecks[i].transform;

                if (target.gameObject == _target.target)
                {

                    Vector3 directionToTarget = (target.position - transform.position).normalized;

                    if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                    {
                        float distanceToTarget = Vector3.Distance(transform.position, target.position);


                        if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                        {
                            _target.visible = true;
                        }
                    }
                }
            }


           

        }
        else 
        {

            if (_target.visible)
            {
                //_target.visible = false;
                //GetComponent<BaseEnemyStateMachine>().ClearTarget();
            }
        }
    }
}

[System.Serializable]
public class EnemyTarget
{
    public GameObject target;
    public bool visible;
    public Vector3 lastKnownLocation;

    public EnemyTarget(GameObject _Target)
    {
        target = _Target;
        visible = false;

    }
}