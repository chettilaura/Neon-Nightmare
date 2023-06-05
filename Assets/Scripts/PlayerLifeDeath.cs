using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeDeath : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;

    [Space]
    [Range(0, 100f)] public float playerHealth;
    public float maxPlayerHealth = 100f;
    public Image[] HealthBarIMG;

    [Space]
    public float lightDamage = 10;
    public float mediumDamage = 20;
    public float bigDamage = 30;

    private Vector3 _respawnPoint;
    void Start()
    {
        playerHealth = maxPlayerHealth;
        _respawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -10f)
        {
           transform.position = _respawnPoint;
            playerHealth = maxPlayerHealth;
        }
        HealthBarFiller();
        if (playerHealth <= 0)
        {
            _playerAnimator.SetBool("isDying", true);
        }
    }


    public void lightAttack()
    {
        playerHealth -= lightDamage;
        if (playerHealth < 0)
            playerHealth = 0;
    }

    public void mediumAttack()
    {
        playerHealth -= mediumDamage;
        if (playerHealth < 0)
            playerHealth = 0;
    }

    public void bigAttack()
    {
        playerHealth -= bigDamage;
        if (playerHealth < 0)
            playerHealth = 0;
    }


    private void HealthBarFiller()
    {
        for(int i=0; i < HealthBarIMG.Length; i++)
        {
            if (playerHealth <= maxPlayerHealth && playerHealth > maxPlayerHealth / HealthBarIMG.Length * i)
                HealthBarIMG[i].enabled = true;
            else 
                HealthBarIMG[i].enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            _respawnPoint = other.transform.position;
            Destroy(other.gameObject);
            Debug.Log("checkpoint");
        }
    }
}
