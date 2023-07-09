using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stepragnoNonBossSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] stoneClips;
    [SerializeField] AudioSource audioSource;
    private void stepragnonoboss()
    {
    
        AudioClip clip = stoneClips[0];
        audioSource.PlayOneShot(clip);
        
    }
}
