using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.AI;

public class CatAI : MonoBehaviour
{

    public List<Transform> wayPoint;

    NavMeshAgent navMeshAgent;

    public int currentWayPointIndex = 0;

    public float lookRadius = 10f;

    Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {;
        Walking();

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            navMeshAgent.SetDestination(target.position);

            if (distance <= navMeshAgent.stoppingDistance)
            {
                // Attack the target
                FaceTarget();
            }
        }
    }

    void FaceTarget ()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void Walking()
    {
        if(wayPoint.Count == 0)
        {
            return;
        }

        float distanceToWayPoint = Vector3.Distance(wayPoint[currentWayPointIndex].position, transform.position);

        if(distanceToWayPoint <= 2)
        {
            currentWayPointIndex = (currentWayPointIndex + 1) % wayPoint.Count;
        }

        navMeshAgent.SetDestination(wayPoint[currentWayPointIndex].position);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
