using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private PlayerLifeDeath playerLifeDeath;
    public float life=2;
    public Collider eplosion_collider;
    
    private void Awake()
    {
        eplosion_collider =  GetComponent<Collider>();
        
    }


    public IEnumerator ChangeColliderStatus()
    {
        eplosion_collider.enabled=true;
        yield return new WaitForSeconds(2);
        eplosion_collider.enabled=false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerLifeDeath = other.GetComponent<PlayerLifeDeath>();
            playerLifeDeath.lightAttack();
        }
    }
}