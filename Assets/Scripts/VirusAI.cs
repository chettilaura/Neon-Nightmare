using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class VirusAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask WhatIsGround, WhatIsPlayer;

    //patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //states
    public float sightRange, fleeRange;
    public bool playerInSight, playerInFlee;

    private void Awake()
    {
        player = GameObject.Find("PlayerCapsule").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, WhatIsPlayer);
        playerInFlee = Physics.CheckSphere(transform.position, fleeRange, WhatIsPlayer);

        if (!playerInSight && !playerInFlee) Patrol();
        if (playerInSight && !playerInFlee) Attack();
        if (playerInSight && playerInFlee) Flee();
    }

    private void Patrol()
    {
        if (!walkPointSet) SearchWalkPoint();
        else agent.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, WhatIsGround))
            walkPointSet = true;
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        if (!alreadyAttacked)
        {
            ///AttackCodeHere
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void Flee()
    {
        Vector3 FleeDirection = player.position - transform.position;
        FleeDirection.Normalize();
        Vector3 FleeDestination = transform.position - FleeDirection * fleeRange;
        agent.SetDestination(FleeDestination);

    }
}
