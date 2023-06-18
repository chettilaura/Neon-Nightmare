using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirusLifeSystem : MonoBehaviour
{
    // Start is called before the first frame update
    [Space]
    [Range(0, 100f)] public float virusHealth;
    public float maxVirusHealth = 100f;
    public float damage;

    [SerializeField] private Slider _lifeSlider;


    void Start()
    {
        virusHealth = maxVirusHealth;
        //_lifeSlider =  GetComponent<Slider>();
        _lifeSlider.value = maxVirusHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(virusHealth == 0)
        {
            Debug.Log("Dead");
            _lifeSlider.gameObject.SetActive(false);
        }
        _lifeSlider.value = virusHealth/100f;
    }

    public void Attack()
    {
        virusHealth -= damage;
    }
}
