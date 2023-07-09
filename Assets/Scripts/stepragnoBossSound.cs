using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stepragnoBossSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] stoneClips;
    [SerializeField] AudioSource audioSource;
    private void stepragno()
    {
    
        AudioClip clip = stoneClips[0];
        audioSource.PlayOneShot(clip);
        
    }
}
