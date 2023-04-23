 using UnityEngine;
 using System.Collections;
 using System.Collections.Generic;

 
 // Prequisites: create an empty GameObject, attach to it a Rigidbody w/ UseGravity unchecked
 // To empty GO also add BoxCollider and this script. Makes this the parent of the Player
 // Size BoxCollider to fit around Player model.
 
 public class magnetic : MonoBehaviour {
 
     private float moveSpeed = 6; // move speed
     private float turnSpeed = 90; // turning speed (degrees/second)
     private float lerpSpeed = 10; // smoothing speed
     private float gravity = 10; // gravity acceleration
     //private bool isGrounded;
     private float deltaGround = 0.2f; // character is grounded up to this distance
     private Vector3 surfaceNormal; // current surface normal
     private Vector3 myNormal; // character normal  
     private Transform myTransform;
     public BoxCollider boxCollider; // drag BoxCollider ref in editor
 

     private void Start(){
     myNormal = transform.up; // normal starts as character up direction
     myTransform = transform; //transform dell'empty con il box collider + rigid body 
     GetComponent<Rigidbody>().freezeRotation = true; // disable physics rotation
     }
 
     private void FixedUpdate(){
     GetComponent<Rigidbody>().AddForce(-gravity*GetComponent<Rigidbody>().mass*myNormal);// apply constant weight force according to character normal:
     }
 
     private void Update(){
     Ray ray;
     RaycastHit hit;
     ray = new Ray(myTransform.position, -myNormal); // cast ray downwards
     Physics.Raycast(ray, out hit); // use it to update myNormal and isGrounded
     surfaceNormal = hit.normal;
     myNormal = Vector3.Lerp(myNormal, surfaceNormal, lerpSpeed*Time.deltaTime); //normale capsula si allinea a normale terreno

        myTransform.Rotate(0, Input.GetAxis("Horizontal")*turnSpeed*Time.deltaTime, 0); // capsula ruota su se stessa e poi si muove con davanti e dietro

     Vector3 myForward = Vector3.Cross(myTransform.right, myNormal); // find forward direction with new myNormal (prodotto vettoriale): 
     Quaternion targetRot = Quaternion.LookRotation(myForward, myNormal); // align character to the new myNormal while keeping the forward direction:
     myTransform.rotation = Quaternion.Lerp(myTransform.rotation, targetRot, lerpSpeed*Time.deltaTime); //rotazione su piano precedente spostata su nuovo piano

        myTransform.Translate(0, 0, Input.GetAxis("Vertical")*moveSpeed*Time.deltaTime); // move the character forth/back with Vertical axis:
    }


 
 }