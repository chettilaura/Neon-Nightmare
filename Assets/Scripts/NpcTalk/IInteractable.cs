using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {

    void Interact(Transform interactorTransform,MonoBehaviour movimenti, MonoBehaviour dash, MonoBehaviour rampino, GameObject containerGameObject);
    string GetInteractText();
    Transform GetTransform();

}