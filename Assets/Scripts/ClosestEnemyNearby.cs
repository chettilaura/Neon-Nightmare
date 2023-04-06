using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestEnemyNearby : MonoBehaviour
{
    public Transform orientation;
    public ParticleSystem particles;
    public float maxDistance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FindClosestEnemy();
    }

    void FindClosestEnemy()
    {
        var projectiles = particles.emission;
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
            Debug.DrawLine(orientation.position, closestEnemy.transform.position);
            projectiles.enabled = true;
        }

        else
        {
            projectiles.enabled = false;
        }
            
    }
}
