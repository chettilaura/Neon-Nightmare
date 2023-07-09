using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mortePlayerSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] stoneClips;
    [SerializeField] AudioSource audioSource;

    void morteplayer(){
        AudioClip clip = stoneClips[0];
        audioSource.PlayOneShot(clip);
    }
}
