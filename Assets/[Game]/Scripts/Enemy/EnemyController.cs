using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates { Idle, Chase }
public class EnemyController : MonoBehaviour
{
    [field: SerializeField] public float fieldOfView { get; private set; } = 180f;
    public List<MeleeFighter> TargetsInRange { get; set; } = new List<MeleeFighter>();
    public MeleeFighter Target { get; set; }
    public StateMachine<EnemyController> StateMachine { get; private set; }

    Dictionary<EnemyStates, State<EnemyController>> stateDict;

    public NavMeshAgent navAgent { get; private set; }

    public Animator Animator { get; private set; }

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();

        stateDict = new Dictionary<EnemyStates, State<EnemyController>>();
        stateDict[EnemyStates.Idle] = GetComponent<IdleState>();
        stateDict[EnemyStates.Chase] = GetComponent<ChaseState>();

        StateMachine = new StateMachine<EnemyController>(this);
        StateMachine.ChangeState(stateDict[EnemyStates.Idle]);
    }

    public void ChangeState(EnemyStates state)
    {
        StateMachine.ChangeState(stateDict[state]);
    }

    private void Update()
    {
        StateMachine.Execute();
    }
}
