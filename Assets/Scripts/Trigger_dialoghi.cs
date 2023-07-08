using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_dialoghi : MonoBehaviour
{
        public GameObject dialogo;
        public Mov movimenti;
        public Dash_script dash;
        public Grapping_Hook rampino;

    private void OnTriggerEnter()
    {

        rampino.enabled = false;
        movimenti.enabled = false;
        dash.enabled = false;
        GameObject instantiatedPrefab = Instantiate(dialogo);
        instantiatedPrefab.SetActive(true);
        ConversationController x= instantiatedPrefab.transform.Find("Dialogue/Conversation_go").GetComponent<ConversationController>();
        x.setMovement_trigger(movimenti,dash,rampino);
        Destroy(this);
    }
    /*    private void OnTriggerExit()
    {
                rampino.enabled = false;
                movimenti.enabled = false;
                dash.enabled = false;
        Destroy(dialogo);
        Destroy(this);
    }*/

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
