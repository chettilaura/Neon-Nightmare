using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappingHook : MonoBehaviour
{

   [SerializeField] GameObject preview;
   [SerializeField] Transform hook;
   [SerializeField] Transform launchPoint;
   private bool isHooked = false;
  Vector3 startingPoint,endPoint;
  float seconds;
  [SerializeField] private Rigidbody rb;
  //[SerializeField] private Movement movement;
  [SerializeField] private LineRenderer lineRenderer;
  [Tooltip("Forza con cui il player viene tirato verso il gancio")]
  [SerializeField] private float grapplingForce=4000;
   
    void Update()
    {
        if ((Input.GetMouseButton(1))&&(!isHooked)) {//mirino
            preview.SetActive(true);

            if (Input.GetMouseButtonDown(0)) {
                isHooked=true;
                preview.SetActive(false);
                seconds=0 ;
                startingPoint = launchPoint.position;
                endPoint=preview.transform.position;
                hook.position = startingPoint;
                hook.gameObject.SetActive(true);
                lineRenderer.enabled=true;
            }


        }else{
            preview.SetActive(false);
        }
    Debug.Log(Time.deltaTime);

        if(isHooked==true){
            if(seconds<1){
                seconds += Time.deltaTime*3;
                hook.position = Vector3.Lerp(startingPoint, endPoint, seconds );
            }else if(seconds!=1){
                seconds=1;
    rb.AddForce((endPoint-startingPoint).normalized*grapplingForce,ForceMode.Force);
    //iniza a seguire l'evento
    
    StartCoroutine(DelayRecharge());
            }
           
           /* if(seconds>=1){ //hook piazzato
                isHooked=false;
            }*/
         lineRenderer.SetPosition(0,launchPoint.position);
         lineRenderer.SetPosition(1,hook.position);
        }
    }
    private IEnumerator DelayRecharge(){
        yield return new WaitForSeconds(0.5f);
        //movement.returnOnGroundEvent+=RechargeHook;
        lineRenderer.enabled=false;
    }
    private void RechargeHook() {
        isHooked=false;
        //movement.returnOnGroundEvent-=RechargeHook;
        hook.gameObject.SetActive(false);
    } 
}
