using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Trojan : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask WhatIsGround, WhatIsPlayer;
    public Explosion explotion_script;

    //patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //attacking

    public ParticleSystem explosionParticleSystem;  // Riferimento al Particle System dell'esplosione

    //states
    public float sightRange, explosionRange;
    public bool playerInSight, playerInExplosionRange;
    public bool ableToCharge;

    //vita
    private VirusLifeSystem _health;

      //suono trojan 
    [SerializeField] private AudioClip[] stoneClips;
    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        player = GameObject.Find("maincharacter").transform;
        agent = GetComponent<NavMeshAgent>();
        _health = GetComponent<VirusLifeSystem>();  
        ableToCharge=true;
    }

    private void Update()
    {
        if(_health.virusHealth> 0)
        {
            playerInSight = Physics.CheckSphere(transform.position, sightRange, WhatIsPlayer);
            playerInExplosionRange = Physics.CheckSphere(transform.position, explosionRange, WhatIsPlayer);

            if (!playerInSight && !playerInExplosionRange) Patrol();
            if (playerInSight && ableToCharge) Charge();
            // if (playerInSight && playerInExplosionRange) Explode();
        }

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


    private IEnumerator WaitBeforeNextAttack()
    {
        ableToCharge=false;
        yield return new WaitForSeconds(2);
        ableToCharge=true;
    }


    private void Charge()
    {         
                transform.LookAt(new Vector3(player.position.x, player.position.y, player.position.z));
                agent.SetDestination(player.position);

    }

private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.CompareTag("Player"))
        {
        // Avvia il Particle System

        
        explosionParticleSystem.Play();

        //suono trojan 
        AudioClip clip = stoneClips[0];
        audioSource.PlayOneShot(clip);
        
        StartCoroutine(WaitBeforeNextAttack());
        StartCoroutine(explotion_script.ChangeColliderStatus());
        }
    }

   

}
