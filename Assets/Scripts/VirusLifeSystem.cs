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
    private bool dead;
    [SerializeField] private Slider _lifeSlider;
    public GameObject trigger;
    public ParticleSystem explosionParticleSystem;
    [SerializeField]  AudioClip[] stoneClipsTrojan;
    [SerializeField]  AudioClip[] stoneClipsVirus;
    [SerializeField] AudioSource audioSource;



    void Start()
    {
        dead= false;
        virusHealth = maxVirusHealth;
        //_lifeSlider =  GetComponent<Slider>();
        _lifeSlider.value = maxVirusHealth;
        _animator = GetComponent<Animator>();
        if(_animator == null)
            _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(virusHealth == 0)
        {
            _lifeSlider.gameObject.SetActive(false);
            _animator.SetBool("isDead", true);
            
            if(GetComponent<Trojan>() != null)
            {
                //esplosione dopo morte + suoni morte
                explosionParticleSystem.Play();
                AudioClip clipTrojan = stoneClipsTrojan[0];
                audioSource.PlayOneShot(clipTrojan);

            }

            if(GetComponent<VirusAI>() != null)
            {
                //suoni morte
                AudioClip clipVirus = stoneClipsVirus[0];
                audioSource.PlayOneShot(clipVirus);
           

            }

            Invoke("Destroy", 2);
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
        if(trigger!=null && dead == false)
        {
               Dash_Spawner y = trigger.GetComponent<Dash_Spawner>();
                y.contatore++;
                dead=true;
        }
        transform.gameObject.SetActive(false);
    }
}
