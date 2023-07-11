using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private Slider _volume;
    [SerializeField] private Toggle _notFullScreen;
    [SerializeField] private TextMeshProUGUI _resolution;

    public void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        _volume.value = AudioListener.volume;
    }

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
    }

    public void FullScreen()
    {
        if (_notFullScreen.isOn)
            Screen.fullScreen = false;
        else
            Screen.fullScreen = true;
    }

    public void SetResolution(int val)
    {
        switch (val)
        {
            case 0:
                Screen.SetResolution(1920, 1080, !_notFullScreen.isOn);
                break;
            case 1:
                Screen.SetResolution(2560, 1440, !_notFullScreen.isOn);
                break;
            case 2:
                Screen.SetResolution(3840, 2160, !_notFullScreen.isOn);
                break;
            default:
                Screen.SetResolution(Screen.width, Screen.height, !_notFullScreen.isOn);
                break;
        }
    }
}
