using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLifeDeath : MonoBehaviour
{
    private int _lives = 100;
    public int Lives { get => _lives; } //takes the value of _lives and makes it a public variable
    [SerializeField] private Vector3 _respawnPoint;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DeadZone.OnPlayerFall += Fall; //to die after falling
    }

    private void Fall()
    {
        _lives -= _lives;

        if(_lives == 0)
        {
            SceneManager.LoadScene(0);
        }

        transform.position = _respawnPoint;
    }
}
