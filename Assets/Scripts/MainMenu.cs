using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private Slider _volume;
    [SerializeField] private Toggle _notFullScreen;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT :(");
        Application.Quit();
    }

    public void ChangeVolume()
    {
        AudioListener.volume = _volume.value;
        Debug.Log("volume :" + _volume.value);
    }

    public void FullScreen()
    {
        if (_notFullScreen.isOn)
            Screen.fullScreen = false;
        else
            Screen.fullScreen = true;
    }
}
