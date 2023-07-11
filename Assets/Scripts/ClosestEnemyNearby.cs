using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClosestEnemyNearby : MonoBehaviour
{
    public Transform orientation;
    public Transform playerObj_orientation;
    private GameObject _closestEnemy;
    private Animator _animator;
    private List<GameObject> _enemiesNearby = new List<GameObject>();
    public ParticleSystem particleSys;
    Vector3 LastTarget = new Vector3(0,0,0);
    GameObject[] enemies;

    private VirusLifeSystem virusLife;
    private int _countTime = 50;
    public bool fire= false;
    public bool fPressed = false;

    [SerializeField] float _minDistance = 300;

    //suono spari 
    [SerializeField] private AudioClip[] stoneClips;
    [SerializeField] AudioSource audioSource;
    private int contatoresuonosparo=0; 
    

    void Start()
    {
        _animator = GetComponent<Animator>();
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
        //Debug.Log("clos enem "+ _closestEnemy);
        //sparo con F
        if (Input.GetKey(KeyCode.F)){

            _animator.SetLayerWeight(1, 1);
            _animator.SetBool("firing", true);

            fPressed = true; 

            if (_closestEnemy != null)
            {
                virusLife = _closestEnemy.GetComponent<VirusLifeSystem>();
                if(virusLife == null)
                {
                    virusLife = _closestEnemy.GetComponentInChildren<VirusLifeSystem>();
                    if(virusLife == null)
                        virusLife = _closestEnemy.GetComponentInParent<VirusLifeSystem>();
                }
                float dist = (_closestEnemy.transform.position - transform.position).sqrMagnitude;
                if(dist <= _minDistance && virusLife.virusHealth!=0)
                {
                    fire = true;
                    contatoresuonosparo++;
                  //  Debug.Log("sparo");
                    fireAtEnemy(_closestEnemy.transform.position);
                    if (_countTime == 50 && virusLife.virusHealth > 0)
                    {
                        virusLife.Attack();
                        _countTime = 0;
                    //    Debug.Log("attacchiamo");
                    }
                    
                    _countTime++;


                    //suono spari 
                    //AudioClip clip = stoneClips[0];
                    //audioSource.PlayOneShot(clip);
                    if (contatoresuonosparo%20==0){
                        sparoSound();
                    }
                    
                    
                } else
                {
                    fire = false;
                    _animator.SetLayerWeight(1, 0);
                    if (_animator.GetCurrentAnimatorStateInfo(1).IsName("NotFire"))
                        _animator.SetBool("firing", false);
                   // Debug.Log("primo else");
                }
                
                if (virusLife.virusHealth == 0)
                    _countTime = 0;

            } else
            {
                fire = false;
            }

            /*if(_enemiesNearby.Count != 0)
            {
                _animator.SetLayerWeight(1, 1);
                _animator.SetBool("firing", true);
            } else
            {
                _animator.SetBool("firing", false);
                if (_animator.GetCurrentAnimatorStateInfo(1).IsName("NotFire"))
                    _animator.SetLayerWeight(1, 0);
            }*/
        }
        else{
            fire=false;
            _animator.SetBool("firing", false);
            if(_animator.GetCurrentAnimatorStateInfo(1).IsName("NotFire"))
                _animator.SetLayerWeight(1, 0);
           // Debug.Log("non spara più e layer 0");
            fPressed = false;
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
            {
                projectiles.enabled = true; //controllo se premuto tasto F per sparare
            }
            else
            {
                projectiles.enabled = false;
            }


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

            /* RaycastHit hit;
            if (closestEnemy != null)
            {
                virusLife = closestEnemy.GetComponent<VirusLifeSystem>();
                /*if (Physics.Raycast(playerObj_orientation.position, closestEnemy.transform.position - playerObj_orientation.position, out hit, Mathf.Infinity, 9))
                { //9 è il layer del player, così ignora quel layer nel raycast
                    Debug.DrawRay(playerObj_orientation.position, closestEnemy.transform.position - playerObj_orientation.position, Color.red);
                    virusLife = hit.transform.GetComponent<VirusLifeSystem>();
                    if (virusLife == null)
                    {
                        virusLife = hit.transform.GetComponentInChildren<VirusLifeSystem>();
                        if (virusLife == null)
                        {
                            virusLife = hit.transform.GetComponentInParent<VirusLifeSystem>();
                        }
                    }
                } else
                {
                    closestEnemy = null;
                } */
            //}

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


    void sparoSound(){
        AudioClip clip = stoneClips[0];
        audioSource.PlayOneShot(clip);
    }
}
