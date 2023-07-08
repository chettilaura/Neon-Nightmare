using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateAI : MonoBehaviour
{
    private VirusAI _ai;
    public float _minDist = 2500;
    [SerializeField] private GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        _ai = GetComponent<VirusAI>();
    }

    // Update is called once per frame
    void Update()
    { 
        float dist = (transform.position - _player.transform.position).sqrMagnitude;
        if (dist > _minDist)
            _ai.enabled = false;
        else
            _ai.enabled = true;
    }
}
