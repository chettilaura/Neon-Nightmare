using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeDeath : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private GameObject _playerCamera;
    private Look_Around _lookAround;

    [Space]
    public float playerHealth;
    public float maxPlayerHealth = 100f;
    public Image[] HealthBarIMG;
    public Image bonus;

    [Space]
    public float lightDamage = 10;
    public float mediumDamage = 20;
    public float bigDamage = 30;

    private Mov _movementScript;
    private Dash_script _scriptDash;
    private Grapping_Hook _scriptHook;
    private ClosestEnemyNearby _scriptClosestEnemyNearby;

    protected float _timeForRespawn = 5;
    private float _time = 0;

    private Vector3 _respawnPoint;

    private bool _attacked = false;

    [Space]
    [SerializeField] private CanvasGroup _brokenGlass;
    private float _damageTimer= 10f;

    [Space]
    [SerializeField] private AudioClip damageAudio;
    private AudioSource _healthAudioSource;


    private float _lastTime = 1000;
    void Start()
    {
        playerHealth = maxPlayerHealth;
        _respawnPoint = transform.position;
        _respawnPoint.y += 20f;
        _movementScript = GetComponent<Mov>();
        _scriptDash = GetComponent<Dash_script>();
        _scriptHook = GetComponent<Grapping_Hook>();
        _scriptClosestEnemyNearby = GetComponent<ClosestEnemyNearby>();
        _lookAround = _playerCamera.transform.Find("Camera").GetComponent<Look_Around>();
    }

    // Update is called once per frame
    void Update()
    {

        HealthBarFiller();
        if (playerHealth == 0)
        {
            _playerAnimator.SetBool("isDying", true);
            _movementScript.enabled = false;
            _scriptDash.enabled = false;
            _scriptHook.enabled = false;
            _lookAround.enabled = false;
            _scriptClosestEnemyNearby.enabled = false;
            _time += Time.deltaTime;
            if (_time >= _timeForRespawn)
            {
                _playerAnimator.SetBool("isDying", false);
                playerHealth = maxPlayerHealth;
                transform.position = _respawnPoint;
                _playerCamera.transform.position = _respawnPoint;
                _movementScript.enabled = true;
                _scriptDash.enabled = true;
                _scriptHook.enabled = true;
                _scriptClosestEnemyNearby.enabled = true;
                _lookAround.enabled = true;
                _brokenGlass.alpha = 0;
                _time = 0;
            }
        }

        if (_attacked)
            DamageGUI();
        if((Time.realtimeSinceStartup - _lastTime) > _damageTimer && _brokenGlass.alpha > 0)
        {
            _attacked = false;
            RestoreGUI();
        }

    }


    public void lightAttack()
    {
        playerHealth -= lightDamage;
        if (playerHealth <= 0)
            playerHealth = 0;
        _lastTime = Time.time;
        _attacked = true;
    }

    public void mediumAttack()
    {
        playerHealth -= mediumDamage;
        if (playerHealth <= 0)
            playerHealth = 0;

        _lastTime = Time.time;
        _attacked = true;
    }

    public void bigAttack()
    {
        playerHealth -= bigDamage;
        if (playerHealth <= 0)
            playerHealth = 0;
        _lastTime = Time.time;
        _attacked = true;
    }


    private void HealthBarFiller()
    {
        for(int i=0; i < HealthBarIMG.Length; i++)
        {
            if (playerHealth > maxPlayerHealth / HealthBarIMG.Length * i)
                HealthBarIMG[i].enabled = true;
            else
                HealthBarIMG[i].enabled = false;

            if (playerHealth >= 65)
                HealthBarIMG[i].color = Color.green;
            else if (playerHealth >= 30)
                HealthBarIMG[i].color = Color.yellow;
            else if (playerHealth < 30)
                HealthBarIMG[i].color = Color.red;
        }

        if (playerHealth > maxPlayerHealth)
        {
            bonus.gameObject.SetActive(true);
        }
        else
        {
            bonus.gameObject.SetActive(false);

        }


    }

    public void BonusLife()
    {
        playerHealth += 50;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            _respawnPoint = other.transform.position;
            //_respawnPoint.y = 20f;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("death"))
        {
            _playerCamera.transform.position = _respawnPoint;
            transform.position = _respawnPoint;
            playerHealth = maxPlayerHealth;
        }
    }


    private void DamageGUI()
    {
        if(_brokenGlass.alpha < 1)
        {
            _brokenGlass.alpha += Time.deltaTime;
        }
        //_healthAudioSource.PlayOneShot(damageAudio);

    }


    private void RestoreGUI()
    {
        if (_brokenGlass.alpha >= 0)
        {
            _brokenGlass.alpha -= Time.deltaTime;
        } 
    }

}
