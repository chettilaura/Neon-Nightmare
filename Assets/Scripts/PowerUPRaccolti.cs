using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUPRaccolti : MonoBehaviour
{
    public int contatore;
    private bool triggerato;
    public GameObject dialogo;
    public Mov movimenti;
    public Dash_script dash;
    public Grapping_Hook rampino;
    public GameObject nextTrigger;
    private bool old_dash,old_rampino;

    private Animator animator;


    void Start()
    {
        triggerato = false;
        animator = GameObject.Find("Player").GetComponent<Animator>();
    }

    private void OnTriggerExit()
      {
        if(contatore== 3 && !triggerato)
        {
            triggerato=true;
            old_dash = dash.enabled;
            old_rampino=rampino.enabled;
            rampino.enabled = false;
            movimenti.enabled = false;
            dash.enabled = false;
            animator.SetBool("isTalking", true);
            animator.SetLayerWeight(1, 0);
            animator.SetLayerWeight(2, 0);
            GameObject instantiatedPrefab = Instantiate(dialogo);
            instantiatedPrefab.SetActive(true);
            ConversationController x= instantiatedPrefab.transform.Find("Dialogue/Conversation_go").GetComponent<ConversationController>();
            x.setMovement_trigger(movimenti,dash,rampino,old_dash,old_rampino);
            if(nextTrigger!= null)
            {
                nextTrigger.SetActive(true);
            }
            Destroy(this);
        }
      }



}
