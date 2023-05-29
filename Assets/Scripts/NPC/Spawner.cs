using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _npc;
    [SerializeField] private Vector3 _position;
    [SerializeField] private Collider _groundCollider;

    private static Spawner _instance;
    public static Spawner Instance => _instance;

    void Start()
    {
        _instance = this;
        GameObject agent = Instantiate(_npc, _position, Quaternion.identity);
    }


}