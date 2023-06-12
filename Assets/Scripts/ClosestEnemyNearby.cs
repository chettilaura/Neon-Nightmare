using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClosestEnemyNearby : MonoBehaviour
{
    public Transform orientation;
    public Transform playerObj_orientation;
    private Transform _closestEnemy;
    public ParticleSystem particleSys;
    Vector3 LastTarget = new Vector3(0,0,0);
    List<Transform> enemies = new List<Transform>();

    private VirusLifeSystem virusLife;
    private int _countTime = 50;
    private bool fire= false;
    

    void Start()
    {

    }

   
    void Update()
    {

        _closestEnemy = FindClosestEnemy();
        //sparo con F
        if (Input.GetKey(KeyCode.F)){
            fire=true;

            if (_closestEnemy != null)
            {
                Debug.Log("sparo");
                fireAtEnemy(_closestEnemy.position);
                if (_countTime == 50 && virusLife.virusHealth > 0)
                {
                    virusLife.Attack();
                    _countTime = 0;
                }
                _countTime++;
            }
        }
        else{
            fire=false;
        }



    }

    
    //tiene lista di enemies che aggiunge e rimuove quando entrano e escono dal trigger (ontriggerenter e ontriggerexit)


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "enemy")
        {
            enemies.Add(other.transform);
            //Debug.Log("nemico entrato in zona");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "enemy")
        {
            LastTarget = other.transform.position;
            enemies.Remove(other.transform);
            //Debug.Log("nemico uscito da zona");
        }
    }

    Transform FindClosestEnemy()
    {
        var projectiles = particleSys.emission;
        float distanceToClosest = Mathf.Infinity;
        Transform closestEnemy = null;

        if (enemies.Count == 0)
        {
            projectiles.enabled = false; //fine sparo quando non ci sono nemici
            fire=false;
        }

        else
        {
            if (fire)
                projectiles.enabled = true; //controllo se premuto tasto F per sparare
            else
                projectiles.enabled = false;

            //ciclo su enemies
            foreach (Transform currentEnemy in enemies)
            {
                if (currentEnemy != null)
                {
                    float distanceToEnemy = (currentEnemy.position - orientation.position).sqrMagnitude;


                    if (distanceToEnemy < distanceToClosest)
                    {
                        distanceToClosest = distanceToEnemy;
                        closestEnemy = currentEnemy;
                    }
                }

            }



            //a fine ciclo sugli enemies controllo che non ci sia oggetto in mezzo tra player e nemico scelto
            //se c'è e quello è il più vicino ritorno null -> nella update andrà a prendermi il lastTarget

            RaycastHit hit;
            if (closestEnemy != null)
            {
                if (Physics.Raycast(playerObj_orientation.position, closestEnemy.position - playerObj_orientation.position, out hit, Mathf.Infinity, 9))
                { //9 è il layer del player, così ignora quel layer nel raycast
                    Debug.DrawRay(playerObj_orientation.position, closestEnemy.position - playerObj_orientation.position, Color.red);
                    //  Debug.Log(hit.transform.tag);

                    if (hit.transform.tag != "enemy" /*&& hit.transform.tag != "Player"*/)
                    {
                        projectiles.enabled = false;
                        fire=false;
                        //Debug.Log(hit.transform.tag);
                        //Debug.Log("oggetto in mezzo che non è enemy");
                        closestEnemy = null;
                    }
                    else
                    {
                        virusLife = hit.transform.GetComponent<VirusLifeSystem>();
                        if (virusLife == null)
                        {
                            virusLife = hit.transform.GetComponentInChildren<VirusLifeSystem>();
                            if (virusLife == null)
                            {
                                virusLife = hit.transform.GetComponentInParent<VirusLifeSystem>();
                            }
                        }
                    }

                }
            }

        }
        

        return closestEnemy;
            
    }

    void fireAtEnemy(Vector3 v2)
    {
        RaycastHit hit;
        if (Physics.Raycast(playerObj_orientation.position, v2 - playerObj_orientation.position, out hit, Mathf.Infinity, 9)){
            Debug.DrawRay(playerObj_orientation.position, v2 - playerObj_orientation.position, Color.red);
        }

        //Debug.Log("sparo");
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];
        int count = particleSys.GetParticles(particles);
        for (int i = 0; i < count; i++)
        {
            ParticleSystem.Particle particle = particles[i];

            Vector3 v1 = particleSys.transform.TransformPoint(particle.position);
           

            Vector3 tarPosi = (v2 - v1) * (particle.remainingLifetime / particle.startLifetime);
            particle.position = particleSys.transform.InverseTransformPoint(v2 - tarPosi);
            particles[i] = particle;
        }

        particleSys.SetParticles(particles, count);
        //Debug.Log(virusLife.gameObject.name);
    }
}
