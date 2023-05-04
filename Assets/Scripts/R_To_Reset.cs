using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class R_To_Reset : MonoBehaviour
{
    // Start is called before the first frame update
    private bool canRestart = false;
    
    void Start()
    {

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && !canRestart)
        {
            Debug.Log("R pressed");
            canRestart = true;
            Restart();
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
