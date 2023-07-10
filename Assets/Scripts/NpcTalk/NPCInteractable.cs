using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable {

    [SerializeField] private string interactText;

    private Animator animator;
    public GameObject dialogo;
   // private NPCHeadLookAt npcHeadLookAt;
    public GameObject trigger;
    public GameObject selfdestroy;
    private void Awake() {
     //   animator = GetComponent<Animator>();
       // npcHeadLookAt = GetComponent<NPCHeadLookAt>();
    }

    public void Interact(Transform interactorTransform,MonoBehaviour movimenti, MonoBehaviour dash, MonoBehaviour rampino, GameObject containerGameObject, bool dashvalue, bool rampinovalue) {
       // ChatBubble3D.Create(transform.transform, new Vector3(-.3f, 1.7f, 0f), ChatBubble3D.IconType.Happy, "Hello there!");
        //animator.SetTrigger("Talk");
        GameObject instantiatedPrefab = Instantiate(dialogo);
         ConversationController x= instantiatedPrefab.transform.Find("Dialogue/Conversation_go").GetComponent<ConversationController>();
      //  Debug.Log("Old_dash Interact= " + dashvalue);
       // Debug.Log("Old_rampino Interact= "+ rampinovalue);
          x.setMovement_interact(movimenti,dash,rampino,containerGameObject,dashvalue,rampinovalue);
          if(trigger!=null)
          {
            PowerUPRaccolti y = trigger.GetComponent<PowerUPRaccolti>();
            y.contatore++;
            
            Destroy(this);
          }

    }

     private void OnDisable()
    {
        Destroy(gameObject);
    }

    public string GetInteractText() {
        return interactText;
    }

    public Transform GetTransform() {
        return transform;
    }

}