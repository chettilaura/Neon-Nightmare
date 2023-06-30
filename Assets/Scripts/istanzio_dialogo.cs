using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class istanzio_dialogo : MonoBehaviour
{
    

    
    public GameObject dialogue_prefab; 

    public DialogueScript dialogue;

    //movimento camera dialoghi 
    //public CinemachineVirtualCamera camera_dialoghi; //camera per i dialoghi 

    private GameObject dialogueBoxClone;
      

    void Update()
    {
        
                dialogueBoxClone = (GameObject)GameObject.Instantiate(dialogue_prefab, transform.position, Quaternion.identity);
                dialogue = ((dialogueBoxClone.transform.Find("Canvas_dialogue")?.gameObject).transform.Find("dialogueBox")?.gameObject).GetComponent<DialogueScript>();
           
    }
}