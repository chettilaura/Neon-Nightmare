using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_Spawner : MonoBehaviour
{
    public int contatore;
    private bool triggerato;
    public GameObject dialogo;
        public Mov movimenti;
        public Dash_script dash;
        public Grapping_Hook rampino;
        public GameObject nextTrigger;
        private bool old_dash,old_rampino;

      private void OnTriggerStay()
    {
        if(contatore== 3 && !triggerato)
        {
        triggerato=true;
        old_dash = dash.enabled;
        old_rampino=rampino.enabled;
        rampino.enabled = false;
        movimenti.enabled = false;
        dash.enabled = false;
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



    void Start()
    {
        triggerato=false;
        
    }
}
