using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class morteRagnoSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] stoneClips;
    [SerializeField] AudioSource audioSource;
    private void morteragno()
    {
    
        AudioClip clip = stoneClips[0];
        audioSource.PlayOneShot(clip);
        
    }
}
