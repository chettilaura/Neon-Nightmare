using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Trojan : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask WhatIsGround, WhatIsPlayer;
    

    //patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //attacking

    public float attackRange = 5f; // Distanza massima per attaccare
    public ParticleSystem explosionParticleSystem;  // Riferimento al Particle System dell'esplosione

    //states
    public float sightRange, explosionRange;
    public bool playerInSight, playerInExplosionRange;

    private void Awake()
    {
        player = GameObject.Find("PlayerCapsule").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, WhatIsPlayer);
        playerInExplosionRange = Physics.CheckSphere(transform.position, explosionRange, WhatIsPlayer);

        if (!playerInSight && !playerInExplosionRange) Patrol();
        if (playerInSight && !playerInExplosionRange) Charge();
        if(playerInSight && playerInExplosionRange) Explode();
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

    private void Charge()
    {
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                agent.SetDestination(player.position);
                 //CARICA
    }



    private void Explode()
    {
        
    ParticleSystem explosionInstance = Instantiate(explosionParticleSystem, transform.position, Quaternion.identity);
    // Avvia il Particle System
        explosionInstance.Play();
        StartCoroutine(WaitForExplosionToFinish(explosionInstance));

    }


private IEnumerator WaitForExplosionToFinish(ParticleSystem explosionInstance)
{
       while (explosionInstance.IsAlive(true))
    {
        yield return null;
    }

    // Il Particle System ha terminato di riprodurre le particelle
    // Puoi eseguire altre azioni o distruggere l'istanza dell'esplosione
    Destroy(explosionInstance.gameObject);
}

}
