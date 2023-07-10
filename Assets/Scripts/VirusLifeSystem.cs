using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirusLifeSystem : MonoBehaviour
{
    // Start is called before the first frame update
    [Space]
    [Range(0, 250f)] public float virusHealth;
    public float maxVirusHealth;
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
            _lifeSlider.gameObject.SetActive(false);
            if(_animator != null)
               _animator.SetBool("isDead", true);
            Invoke("Destroy", 1);
        }

        if(virusHealth < 0)
            virusHealth = 0;
        _lifeSlider.value = virusHealth/maxVirusHealth;
    }

    public void Attack()
    {
        virusHealth -= damage;
    }

    public void Destroy()
    {
        transform.gameObject.SetActive(false);
    }
}
