using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private PlayerLifeDeath playerLifeDeath;
    public float life=2;

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