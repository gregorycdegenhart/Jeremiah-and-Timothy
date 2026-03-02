using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatAI : MonoBehaviour
{

    public List<Transform> wayPoint;

    NavMeshAgent navMeshAgent;

    public int currentWayPointIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Walking();
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
}
