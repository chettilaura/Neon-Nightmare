using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCollection : MonoBehaviour
{
    [SerializeField] PlayerLifeDeath _playerLife;
    private void OnTriggerEnter(Collider other)
    {        
        if (other.tag == "Player")
        {
            Destroy(this.gameObject);
            _playerLife.BonusLife();
        }
    }
}
