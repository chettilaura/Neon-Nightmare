using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusLifeSystem : MonoBehaviour
{
    // Start is called before the first frame update
    [Space]
    [Range(0, 100f)] public float virusHealth;
    public float maxVirusHealth = 100f;
    public float damage;


    void Start()
    {
        virusHealth = maxVirusHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(virusHealth == 0)
        {
            Debug.Log("Dead");
        }
    }

    public void Attack()
    {
        virusHealth -= damage;
    }
}
