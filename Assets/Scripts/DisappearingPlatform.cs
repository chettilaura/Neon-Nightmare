using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    public float disappearingTime;
    [SerializeField] private GameObject[] _platforms;
    //public float appearingTime;

    public float currentTime;
    private bool isEnabled;
    // Start is called before the first frame update
    void Start()
    {
        isEnabled = true;
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (isEnabled)
        {
            if (currentTime >= disappearingTime)
            {
                currentTime = 0;
                Hide();
            }
            else if (currentTime >= 6 && currentTime < 8)
            {
                _platforms[0].SetActive(false);
                _platforms[1].SetActive(true);
            }
            else if (currentTime >= 8 && currentTime < disappearingTime)
            {
                _platforms[1].SetActive(false);
                _platforms[2].SetActive(true);

            }
        } else
        {
            if (currentTime >= 1)
                Hide();
        }


        
    }

    void Hide() 
    {
        isEnabled = !isEnabled;
        if (isEnabled)
        {
            _platforms[0].SetActive(true);
        } else
        {
            _platforms[2].SetActive(false);
        }
     
    }
}
