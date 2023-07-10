using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {

    void Interact(Transform interactorTransform,MonoBehaviour movimenti, MonoBehaviour dash, MonoBehaviour rampino, GameObject containerGameObject,bool dashvalue,bool rampinovalue);
    string GetInteractText();
    Transform GetTransform();

}