using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class virusBullet : MonoBehaviour
    
{
    private PlayerLifeDeath playerLifeDeath;
    public float life;

    private void Awake()
    {
        Destroy(gameObject, life);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerLifeDeath = other.GetComponent<PlayerLifeDeath>();
            playerLifeDeath.lightAttack();
            Destroy(gameObject);
        }
    }
}
