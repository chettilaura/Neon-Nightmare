using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private Toggle _notFullScreen;
    [SerializeField] private GameObject _options;
    [SerializeField] private GameObject _controls;

    public GameObject PauseMenuUI;

    private Animator animator;

    public void Start()
    {
        animator = GameObject.Find("Player").GetComponent<Animator>();
    }


    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!animator.GetBool("isTalking"))
            {
                if (GameIsPaused)
                {
                    Resume();
                    _options.SetActive(false);
                    _controls.SetActive(false);
                }
                else
                {
                    Pause();
                }
            }

        }


    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main_Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void FullScreen()
    {
        if(_notFullScreen.isOn)
            Screen.fullScreen = false;
        else
            Screen.fullScreen = true;
    }

    public void ChangeVolume()
    {
        AudioListener.volume = _volumeSlider.value;
        Debug.Log("volume :" + _volumeSlider.value);
    }
}
