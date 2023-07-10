using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {

    public MonoBehaviour rampino;
    public MonoBehaviour movimenti;
    public MonoBehaviour dash;
    private Look_Around look_Around;
    private Animator animator;
    private bool old_dash,old_rampino;
    [SerializeField] private GameObject containerGameObject;

    public void Start()
    {
        look_Around = GameObject.Find("Camera").GetComponent<Look_Around>();
        animator = GameObject.Find("Player").GetComponent<Animator>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E) && containerGameObject.activeSelf) {
            containerGameObject.SetActive(false);

            IInteractable interactable = GetInteractableObject();
            if (interactable != null) {
                animator.SetBool("isTalking", true);
                animator.SetBool("isWalking", false);

                animator.SetLayerWeight(1, 0);
                animator.SetLayerWeight(2, 0);
                old_dash = dash.enabled;
                old_rampino=rampino.enabled;
                rampino.enabled = false;
                movimenti.enabled = false;
                dash.enabled = false;
                look_Around.enabled = false;
                interactable.Interact(transform,movimenti,dash,rampino,containerGameObject,old_dash,old_rampino);
            }
        }
    }

    public IInteractable GetInteractableObject() {
        List<IInteractable> interactableList = new List<IInteractable>();
        float interactRange = 3f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray) {
            if (collider.TryGetComponent(out IInteractable interactable)) {
                interactableList.Add(interactable);
               
            }
        }

        IInteractable closestInteractable = null;
        foreach (IInteractable interactable in interactableList) {
            if (closestInteractable == null) {
                closestInteractable = interactable;
            } else {
                if (Vector3.Distance(transform.position, interactable.GetTransform().position) < 
                    Vector3.Distance(transform.position, closestInteractable.GetTransform().position)) {
                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable;
    }

}