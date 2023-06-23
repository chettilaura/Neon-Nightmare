using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClosestEnemyNearby : MonoBehaviour
{
    public Transform orientation;
    public Transform playerObj_orientation;
    private GameObject _closestEnemy;
    private List<GameObject> _enemiesNearby = new List<GameObject>();
    public ParticleSystem particleSys;
    Vector3 LastTarget = new Vector3(0,0,0);
    GameObject[] enemies;

    private VirusLifeSystem virusLife;
    private int _countTime = 50;
    private bool fire= false;

    [SerializeField] float _minDistance = 300;
    

    void Start()
    {
        
    }

   
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject enemy in enemies)
        {
            float dist = (enemy.transform.position - transform.position).sqrMagnitude;
            if(dist < _minDistance && !_enemiesNearby.Contains(enemy))
            {
                _enemiesNearby.Add(enemy);
            } else
            {
                _enemiesNearby.Remove(enemy);
            }
        }
        _closestEnemy = FindClosestEnemy();
        //sparo con F
        if (Input.GetKey(KeyCode.F)){
            
            if (_closestEnemy != null)
            {
                float dist = (_closestEnemy.transform.position - transform.position).sqrMagnitude;
                if(dist <= _minDistance)
                {
                    fire = true;
                    Debug.Log("sparo");
                    fireAtEnemy(_closestEnemy.transform.position);
                    if (_countTime == 50 && virusLife.virusHealth > 0)
                    {
                        virusLife.Attack();
                        _countTime = 0;
                    }
                    _countTime++;
                } else
                {
                    fire = false;
                }

            } else
            {
                fire = false;
            }
        }
        else{
            fire=false;
        }



    }

    
    //tiene lista di enemies che aggiunge e rimuove quando entrano e escono dal trigger (ontriggerenter e ontriggerexit)


    GameObject FindClosestEnemy()
    {
        var projectiles = particleSys.emission;
        float distanceToClosest = Mathf.Infinity;
        GameObject closestEnemy = null;

        if (enemies.Length == 0)
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
            foreach (GameObject currentEnemy in _enemiesNearby)
            {
                if (currentEnemy != null)
                {
                    float distanceToEnemy = (currentEnemy.transform.position - transform.position).sqrMagnitude;


                    if (distanceToEnemy < distanceToClosest && distanceToEnemy<= _minDistance)
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
                if (Physics.Raycast(playerObj_orientation.position, closestEnemy.transform.position - playerObj_orientation.position, out hit, Mathf.Infinity, 9))
                { //9 è il layer del player, così ignora quel layer nel raycast
                    Debug.DrawRay(playerObj_orientation.position, closestEnemy.transform.position - playerObj_orientation.position, Color.red);
                    //  Debug.Log(hit.transform.tag);

                    if (hit.transform.tag != "enemy" /*&& hit.transform.tag != "Player"*/ )
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
