using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using System.Collections;

[System.Serializable]
public class QuestionEvent : UnityEvent<Question> {}

public class ConversationController : MonoBehaviour
{
    public Conversation conversation;
    public Conversation defaultConversation;
    public QuestionEvent questionEvent;

    public GameObject speakerLeft;
    public GameObject speakerRight;

    private SpeakerUIController speakerUILeft;
    private SpeakerUIController speakerUIRight;

    public MonoBehaviour rampino;
    public MonoBehaviour movimenti;
    public MonoBehaviour dash;
    private Look_Around _look_around;
    [SerializeField] private GameObject containerGameObject;

    private int activeLineIndex;
    private bool conversationStarted = false;
    private bool first_line = false;

    public void ChangeConversation(Conversation nextConversation) {
        conversationStarted = false;
        conversation = nextConversation;
        AdvanceLine();
    }

    public void setMovement_trigger(MonoBehaviour movimenti, MonoBehaviour dash, MonoBehaviour rampino){
            this.rampino = rampino;
            this.movimenti = movimenti;
            this.dash = dash;
            this.containerGameObject=null;
    }

    public void setMovement_interact(MonoBehaviour movimenti, MonoBehaviour dash, MonoBehaviour rampino, GameObject containerGameObject){
            this.rampino = rampino;
            this.movimenti = movimenti;
            this.dash = dash;
            this.containerGameObject = containerGameObject;

            this.containerGameObject.SetActive(false);
    }

    private void Start()
    {
        speakerUILeft  = speakerLeft.GetComponent<SpeakerUIController>();
        speakerUIRight = speakerRight.GetComponent<SpeakerUIController>();
        _look_around = GameObject.Find("Camera").GetComponent<Look_Around>();
    }

    private void Update()
    {
        if(first_line == false)
        {
            AdvanceLine();
            first_line=true;
            return;
        }
        if (Input.GetKeyDown("space"))
            AdvanceLine();
        else if (Input.GetKeyDown("x"))
            EndConversation();
    }

    private void EndConversation() {
        conversation = defaultConversation;
        conversationStarted = false;
        speakerUILeft.Hide();
        speakerUIRight.Hide();
        rampino.enabled = true;
        movimenti.enabled = true;
        dash.enabled = true;
        _look_around.enabled = true;
        GameObject parentObject = this.transform.parent.gameObject.transform.parent.gameObject;
        if(containerGameObject!=null)
        containerGameObject.SetActive(true);
        Debug.Log("Conversazione finita");
        Destroy(parentObject);
    }

    private void Initialize() {
        conversationStarted = true;
        activeLineIndex = 0;
        speakerUILeft.Speaker = conversation.speakerLeft;
        speakerUIRight.Speaker = conversation.speakerRight;
    }

    private void AdvanceLine() {
        if (conversation == null) return;
        if (!conversationStarted) Initialize();

        if (activeLineIndex < conversation.lines.Length)
            DisplayLine();
        else
            AdvanceConversation();            
    }

    private void DisplayLine() {
        Line line = conversation.lines[activeLineIndex];
        Character character = line.character;

        if (speakerUILeft.SpeakerIs(character))
        {
            SetDialog(speakerUILeft, speakerUIRight, line);
        }
        else {
            SetDialog(speakerUIRight, speakerUILeft, line);
        }

        activeLineIndex += 1;
    }

    private void AdvanceConversation() {
        // These are really three types of dialog tree node
        // and should be three different objects with a standard interface
        if (conversation.question != null)
            questionEvent.Invoke(conversation.question);
        else if (conversation.nextConversation != null)
            ChangeConversation(conversation.nextConversation);
        else
            EndConversation();
    }

    private void SetDialog(
        SpeakerUIController activeSpeakerUI,
        SpeakerUIController inactiveSpeakerUI,
        Line line
    ) {
        activeSpeakerUI.Show();
        inactiveSpeakerUI.Hide();

        activeSpeakerUI.Dialog = "";
        activeSpeakerUI.Mood = line.mood;

        StopAllCoroutines();
        StartCoroutine(EffectTypewriter(line.text, activeSpeakerUI));
    }

    private IEnumerator EffectTypewriter(string text, SpeakerUIController controller) {
        foreach(char character in text.ToCharArray()) {
            controller.Dialog += character;
            yield return new  WaitForSeconds(0.01f);
            // yield return null;
        }
    }
}
