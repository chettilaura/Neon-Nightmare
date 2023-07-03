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
    private Animator _animator;

    [SerializeField] private Slider _lifeSlider;


    void Start()
    {
        virusHealth = maxVirusHealth;
        //_lifeSlider =  GetComponent<Slider>();
        _lifeSlider.value = maxVirusHealth;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(virusHealth == 0)
        {
            Debug.Log("Dead");
            _lifeSlider.gameObject.SetActive(false);
            _animator.SetBool("isDead", true);
        }
        _lifeSlider.value = virusHealth/100f;
    }

    public void Attack()
    {
        virusHealth -= damage;
    }
}
