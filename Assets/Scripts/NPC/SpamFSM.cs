using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpamFSM : MonoBehaviour
{
    [SerializeField] private List<Transform> _waypoints;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _minChaseDistance = 3f;
    [SerializeField] private float _minAttackDistance = 2f;
    [SerializeField] private float _stoppingDistance = 1f;

    private FiniteStateMachine<SpamFSM> _stateMachine;

    private NavMeshAgent _navMeshAgent;
    private int _currentWayPointIndex = 0;

    private VirusLifeSystem _virusLife;
    
    private ParticleSystem _particles;

     //suono sciame 
    [SerializeField] AudioClip[] stoneClips;
    [SerializeField] AudioSource audioSource;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _virusLife = GetComponent<VirusLifeSystem>();
        _particles = GetComponentInChildren<ParticleSystem>();

        _stateMachine = new FiniteStateMachine<SpamFSM>(this);

        //STATES
        State patrolState = new PatrolState("Patrol", this);
        State chaseState = new ChaseState("Chase", this);
        State stopState = new StopState("Stop", this);

        //TRANSITIONS
        _stateMachine.AddTransition(patrolState, chaseState, () => DistanceFromTarget() <= _minChaseDistance);
        _stateMachine.AddTransition(chaseState, patrolState, () => DistanceFromTarget() > _minChaseDistance);
        _stateMachine.AddTransition(chaseState, stopState, () => DistanceFromTarget() <= _stoppingDistance);
        _stateMachine.AddTransition(stopState, chaseState, () => DistanceFromTarget() > _stoppingDistance);

        //START STATE
        _stateMachine.SetState(patrolState);
    }

    void Update()
    {
        _stateMachine.Tik();
        //suono sciame (va qui??) 
            AudioClip clip = stoneClips[0];
            audioSource.PlayOneShot(clip);

        if(_virusLife.virusHealth == 0)
        {
            _particles.Stop();
            Destroy(transform.gameObject);
        }
    }
    public void StopAgent(bool stop) => _navMeshAgent.isStopped = stop;
    public void SetWayPointDestination()
    {
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && _navMeshAgent.velocity.sqrMagnitude <= 0f)
        {
            _currentWayPointIndex = (_currentWayPointIndex + 1) % _waypoints.Count;
            Vector3 nextWayPointPos = _waypoints[_currentWayPointIndex].position;
            _navMeshAgent.SetDestination(new Vector3(nextWayPointPos.x, transform.position.y, nextWayPointPos.z));
        }
    }
    public void FollowTarget()
    {
        _navMeshAgent.SetDestination(_target.transform.position);
        //_navMeshAgent.transform.LookAt(_target.transform);
        _navMeshAgent.transform.rotation = Quaternion.Inverse(_target.transform.rotation);
    } 

    //TRANSITION FUNCTIONS
    private float DistanceFromTarget() => Vector3.Distance(_target.transform.position, transform.position);
}

public class PatrolState : State
{
    private SpamFSM _spam;
    public PatrolState(string name, SpamFSM spam) : base(name)
    {
        _spam = spam;
    }

    public override void Enter()
    {
        _spam.StopAgent(false);
    }

    public override void Tik()
    {
        _spam.SetWayPointDestination();
    }

    public override void Exit()
    {

    }
}

public class ChaseState : State
{
    private SpamFSM _spam;
    public ChaseState(string name, SpamFSM spam) : base(name)
    {
        _spam = spam;
    }

    public override void Enter()
    {
        _spam.StopAgent(false);
    }

    public override void Tik()
    {
        _spam.FollowTarget();
    }

    public override void Exit()
    {
    }
}

public class StopState : State
{
    private SpamFSM _spam;
    public StopState(string name, SpamFSM spam) : base(name)
    {
        _spam = spam;
    }

    public override void Enter()
    {
        _spam.StopAgent(true);
    }

    public override void Tik()
    {
         
    }

    public override void Exit()
    {
    }
}


