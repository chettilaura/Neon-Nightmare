using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestEnemyNearby : MonoBehaviour
{
    public Transform orientation;
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
        }
            
    }
}
