using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_script : MonoBehaviour
{
    private PlayerLifeDeath PlayerLifeDeath;
    private int count = 100;



    void Start()
    {
        gameObject.tag = "enemy";
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerLifeDeath = other.GetComponent<PlayerLifeDeath>();
            if (count == 100)
            {
                PlayerLifeDeath.lightAttack();
                count = 0;
            }
            count++;
        }
            
    }

    
}
