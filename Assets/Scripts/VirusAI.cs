using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class VirusAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;
    public GameObject bulletPrefab;
    public Transform BulletSpawnPoint;
    public float bulletSpeed;
    public float bulletSpread;
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

     //suono spari 
    [SerializeField] private AudioClip[] stoneClips;
    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        if (!alreadyAttacked)
        {
            float randomRange = Random.Range(-bulletSpread, bulletSpread);
            var bullet = Instantiate(bulletPrefab, BulletSpawnPoint.position, BulletSpawnPoint.rotation);
            bullet.GetComponent<Rigidbody>().velocity = Quaternion.Euler(0f, randomRange, 0f) * BulletSpawnPoint.forward * bulletSpeed;
            alreadyAttacked = true;

             //suono spari 
                    AudioClip clip = stoneClips[0];
                    audioSource.PlayOneShot(clip);

            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void Flee()
    {
        Vector3 FleeDirection = player.transform.position - transform.position;
        FleeDirection.Normalize();
        Vector3 FleeDestination = transform.position - FleeDirection * fleeRange;
        agent.SetDestination(FleeDestination);

    }
}
