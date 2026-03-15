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
    public GameObject detectScreenUI;
    Transform target;

    void Start()
    {
        target = PlayerManager.instance.player.transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            // Chase player
            navMeshAgent.SetDestination(target.position);
            detectScreenUI.SetActive(true);

            if (distance <= navMeshAgent.stoppingDistance)
            {
                FaceTarget();
            }
        }
        else
        {
            // Patrol waypoints
            detectScreenUI.SetActive(false);
            Walking();
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void Walking()
    {
        if (wayPoint.Count == 0)
        {
            return;
        }

        float distanceToWayPoint = Vector3.Distance(wayPoint[currentWayPointIndex].position, transform.position);

        if (distanceToWayPoint <= 2)
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