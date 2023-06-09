using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestEnemyNearby : MonoBehaviour
{
    public Transform orientation;
    public Transform playerObj_orientation;
    private Transform _closestEnemy;
    public ParticleSystem particleSys;
    Vector3 LastTarget = new Vector3(0,0,0);
    List<Transform> enemies = new List<Transform>();
    

    void Start()
    {

    }

   
    void Update()
    {
        _closestEnemy = FindClosestEnemy();
        if (_closestEnemy == null)
        {
            //fireAtEnemy(LastTarget);
        }
        else
        {
            fireAtEnemy(_closestEnemy.position);
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
        }

        else
        {
            projectiles.enabled = true;

            //ciclo su enemies
            foreach (Transform currentEnemy in enemies)
            {
                float distanceToEnemy = (currentEnemy.position - orientation.position).sqrMagnitude;
                

                if (distanceToEnemy < distanceToClosest)
                {
                    distanceToClosest = distanceToEnemy;
                    closestEnemy = currentEnemy;
                }
            }

        

                            //a fine ciclo sugli enemies controllo che non ci sia oggetto in mezzo tra player e nemico scelto
                            //se c'è e quello è il più vicino ritorno null -> nella update andrà a prendermi il lastTarget
                                    
                            RaycastHit hit;
                            if(closestEnemy!=null){
                                if (Physics.Raycast(playerObj_orientation.position, closestEnemy.position - playerObj_orientation.position, out hit, Mathf.Infinity, 9)){ //9 è il layer del player, così ignora quel layer nel raycast
                                    Debug.DrawRay(playerObj_orientation.position, closestEnemy.position - playerObj_orientation.position, Color.red);
                                  //  Debug.Log(hit.transform.tag);

                                if (hit.transform.tag != "enemy" /*&& hit.transform.tag != "Player"*/)
                                    {
                                        projectiles.enabled = false;
                                        //Debug.Log(hit.transform.tag);
                                        //Debug.Log("oggetto in mezzo che non è enemy");
                                        closestEnemy = null;
                                    }
                                
                                }
                            }

        }
        

        return closestEnemy;
            
    }

    void fireAtEnemy(Vector3 v2)
    {
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
    }
}
