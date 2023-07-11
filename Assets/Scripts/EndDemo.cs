using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndDemo : MonoBehaviour
{

    [SerializeField] private Canvas _end;
    // Start is called before the first frame update
    public void OnTriggerEnter(Collider other)
    {
        _end.gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }
}
