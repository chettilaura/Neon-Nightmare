using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestEnemyNearby : MonoBehaviour
{
    public Transform orientation;
    public ParticleSystem particleSys;
    public float maxDistance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FindClosestEnemy();
        fireAtEnemy();

    }


    Enemy FindClosestEnemy()
    {
        var projectiles = particleSys.emission;
        float distanceToClosest = Mathf.Infinity;
        Enemy closestEnemy = null;
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();

        foreach(Enemy currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - orientation.position).sqrMagnitude;
            if(distanceToEnemy < distanceToClosest)
            {
                distanceToClosest = distanceToEnemy;
                closestEnemy = currentEnemy;
            }
        }
        if(distanceToClosest < maxDistance)
        {
            projectiles.enabled = true;
        }
        else
        {
            projectiles.enabled = false;
        }

        return closestEnemy;
            
    }

    void fireAtEnemy()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];
        int count = particleSys.GetParticles(particles);
        for (int i = 0; i < count; i++)
        {
            ParticleSystem.Particle particle = particles[i];

            Vector3 v1 = particleSys.transform.TransformPoint(particle.position);
            Vector3 v2 = FindClosestEnemy().transform.position;

            Vector3 tarPosi = (v2 - v1) * (particle.remainingLifetime / particle.startLifetime);
            particle.position = particleSys.transform.InverseTransformPoint(v2 - tarPosi);
            particles[i] = particle;
        }

        particleSys.SetParticles(particles, count);
    }
}
