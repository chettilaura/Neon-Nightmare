using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeDeath : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private GameObject _playerCamera;

    [Space]
    [Range(0, 100f)] public float playerHealth;
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
    void Start()
    {
        playerHealth = maxPlayerHealth;
        _respawnPoint = transform.position;
        _respawnPoint.y += 20f;
        _movementScript = GetComponent<Mov>();
        _scriptDash = GetComponent<Dash_script>();
        _scriptHook = GetComponent<Grapping_Hook>();
        _scriptClosestEnemyNearby = GetComponent<ClosestEnemyNearby>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < 15f)
        {
            _playerCamera.transform.position = _respawnPoint;
            transform.position = _respawnPoint;
            playerHealth = maxPlayerHealth;
        }
        HealthBarFiller();
        if (playerHealth == 0)
        {
            _playerAnimator.SetBool("isDying", true);
            _movementScript.enabled = false;
            _scriptDash.enabled = false;
            _scriptHook.enabled = false;
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
                _time = 0;
            }
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

            if (playerHealth >= 65)
                HealthBarIMG[i].color = Color.green;
            else if (playerHealth >= 30)
                HealthBarIMG[i].color = Color.yellow;
            else if (playerHealth < 30)
                HealthBarIMG[i].color = Color.red;
        }
        if (playerHealth > maxPlayerHealth)
        {
            bonus.enabled = true;
        } else
        {
            bonus.enabled = false;
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
    }
}
