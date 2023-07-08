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
        private bool triggerato;

    private void OnTriggerEnter()
    {
        if(!triggerato)
        {
        triggerato=true;
        Debug.Log("Dentro");
        rampino.enabled = false;
        movimenti.enabled = false;
        dash.enabled = false;
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



    void Start()
    {
        triggerato=false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
