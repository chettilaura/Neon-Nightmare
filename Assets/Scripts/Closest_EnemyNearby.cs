using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closest_EnemyNearby : MonoBehaviour
{
    public Transform orientation;
    public ParticleSystem particleSys;
    Vector3 LastTarget = new Vector3(0,0,0);
    List<Transform> enemies = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FindClosestEnemy();
        if (FindClosestEnemy() == null)
        {
            fireAtEnemy(LastTarget);
        }
        else
        {
            fireAtEnemy(FindClosestEnemy().position);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "enemy")
        {
            enemies.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "enemy")
        {
            LastTarget = other.transform.position;
            enemies.Remove(other.transform);
        }
    }

    Transform FindClosestEnemy()
    {
        var projectiles = particleSys.emission;
        float distanceToClosest = Mathf.Infinity;
        Transform closestEnemy = null;

        if (enemies.Count == 0)
        {
            projectiles.enabled = false;
        }

        else
        {
            projectiles.enabled = true;
            foreach (Transform currentEnemy in enemies)
            {
                float distanceToEnemy = (currentEnemy.position - orientation.position).sqrMagnitude;
                if (distanceToEnemy < distanceToClosest)
                {
                    distanceToClosest = distanceToEnemy;
                    closestEnemy = currentEnemy;
                }
            }

        }

        return closestEnemy;
            
    }

    void fireAtEnemy(Vector3 v2)
    {
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
