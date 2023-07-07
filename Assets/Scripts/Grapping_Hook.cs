using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grapping_Hook : MonoBehaviour
{

    [SerializeField] GameObject preview;
    [SerializeField] Transform hook;
    [SerializeField] Transform launchPointR;
    [SerializeField] Transform launchPointL;


    [SerializeField] private Rigidbody rb;
    [SerializeField] private Mov movement;
    [SerializeField] private LineRenderer lineRenderer;
    [Tooltip("Forza con cui il player viene tirato verso il gancio")]
    [SerializeField] private float grapplingForce=4000;
    public GameObject transistor_bagpack;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform _player;

    [SerializeField] private AudioClip[] stoneClips;
    [SerializeField] AudioSource audioSource;

    private Animator _animator;
    private Vector3 moveDir;
    private bool isHooked = false;
    Vector3 startingPoint, endPoint;
    float seconds;
    private bool beenHooked= false;
    private Quaternion _rotation;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _rotation = _player.rotation;
        var launchPoint = (launchPointR.position + launchPointL.position) / 2;
        lineRenderer.SetPosition(0, launchPoint);
        lineRenderer.SetPosition(1, hook.position);
        if ((Input.GetMouseButton(1))&&(!isHooked)) {//mirino
            _animator.SetLayerWeight(2, 1);
            preview.SetActive(true);

            _player.rotation = Quaternion.Slerp(_player.rotation, orientation.rotation, Time.deltaTime * 4);
            if(_rotation!= _player.rotation)
            {
                Debug.Log("qui");
                _animator.SetBool("rotating", true);
            } else 
                _animator.SetBool("rotating", false);

            //movement.canDoubleJump=false;
            //movement.readyToJump=false;
            _animator.SetBool("grappingHookStart", true);
            transistor_bagpack.SetActive(false);
            lineRenderer.enabled = true;

            if (Input.GetMouseButton(0)) {
                _animator.SetBool("hook", true);
                // _animator.SetBool("grappingHookStart", false);;
                isHooked =true;
                movement.canDoubleJump=false;
                movement.readyToJump=false;
                preview.SetActive(false);
                seconds=0;
                startingPoint = launchPoint;
                endPoint=preview.transform.position;
                hook.position = startingPoint;
                hook.gameObject.SetActive(true);
                beenHooked=true;
                
                

                //AudioClip clip2 = stoneClips[0];
                //audioSource.PlayOneShot(clip2);
            }


        }else{
            preview.SetActive(false);
            _animator.SetBool("grappingHookStart", false);
            _animator.SetLayerWeight(2, 0);
            if (!beenHooked)
            {
                lineRenderer.enabled = false;
            }
        }



        //Debug.Log(Time.deltaTime);

        if (isHooked == true)
        {
            if (seconds < 1)
            {
                seconds += Time.deltaTime * 3;
                hook.position = Vector3.Lerp(startingPoint, endPoint, seconds);
            }
            else if (seconds != 1)
            {
                seconds = 1;
                rb.AddForce((endPoint - startingPoint).normalized * grapplingForce, ForceMode.Force);
                //iniza a seguire l'evento

                StartCoroutine(DelayRecharge());
            }

            /* if(seconds>=1){ //hook piazzato
                 isHooked=false;
             }*/

        }
        else
        {
            _animator.SetBool("hook", false);
            beenHooked = false;
        }


        if (_animator.GetBool("isIdle") && (!_animator.GetBool("grappingHookStart") && !_animator.GetBool("hook")))
            lineRenderer.enabled = false;

    }
    private IEnumerator DelayRecharge(){
        yield return new WaitForSeconds(0.5f);
        movement.returnOnGroundEvent+=RechargeHook;
    }
    private void RechargeHook() {
        isHooked=false;
        movement.returnOnGroundEvent-=RechargeHook;
        hook.gameObject.SetActive(false);
        transistor_bagpack.SetActive(true);
    } 

}
