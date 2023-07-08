using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateTrojanAI : MonoBehaviour
{
    private Trojan _ai;
    private float _minDist = 2500;
    [SerializeField] private GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        _ai = GetComponent<Trojan>();
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
