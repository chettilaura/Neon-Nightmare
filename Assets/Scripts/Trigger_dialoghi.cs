using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_dialoghi : MonoBehaviour
{
    public GameObject dialogo;
    public Mov movimenti;
    public Dash_script dash;
    public Grapping_Hook rampino;
    public GameObject nextTrigger;
    private Look_Around Look_Around;
    public bool triggerato;
    private Animator animator;


    void Start()
    {
        triggerato = false;
        Look_Around = GameObject.Find("Camera").GetComponent<Look_Around>();
        animator = GameObject.Find("Player").GetComponent<Animator>();
    }

    private void OnTriggerEnter()
    {
        if(!triggerato)
        {
            triggerato=true;
            Debug.Log("Dentro");
            rampino.enabled = false;
            movimenti.enabled = false;
            dash.enabled = false;
            Look_Around.enabled = false;
            animator.SetBool("isIdle", true);
            animator.SetBool("isWalking", false);
            GameObject instantiatedPrefab = Instantiate(dialogo);
            instantiatedPrefab.SetActive(true);
            ConversationController x= instantiatedPrefab.transform.Find("Dialogue/Conversation_go").GetComponent<ConversationController>();
            x.setMovement_trigger(movimenti,dash,rampino);
            if(nextTrigger!= null)
            {
                nextTrigger.SetActive(true);
            }
            Destroy(this);
        }
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
