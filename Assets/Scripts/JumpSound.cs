using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] stoneClips;
    [SerializeField] AudioSource audioSource;
   

    private void jump()
    {
    
        AudioClip clip = stoneClips[0];
        audioSource.PlayOneShot(clip);
        
    }


}