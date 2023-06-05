using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_script : MonoBehaviour
{
    private PlayerLifeDeath PlayerLifeDeath;
    void Start()
    {
        gameObject.tag = "enemy";
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerLifeDeath = other.GetComponent<PlayerLifeDeath>();
            PlayerLifeDeath.lightAttack();
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
