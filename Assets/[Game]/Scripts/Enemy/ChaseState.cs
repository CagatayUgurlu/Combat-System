using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State<EnemyController>
{
    [SerializeField] float distanceToStand = 2f;
    EnemyController enemy;

    public override void Enter(EnemyController owner)
    {
        enemy = owner;
        enemy.navAgent.stoppingDistance = distanceToStand;
        //Debug.Log("Entered Chase State");

    }

    public override void Execute()
    {
        enemy.navAgent.SetDestination(enemy.Target.transform.position);
        enemy.Animator.SetFloat("moveAmount", enemy.navAgent.velocity.magnitude / enemy.navAgent.speed);
        //Debug.Log("Executing Chase State");
    }

    public override void Exit()
    {
        //Debug.Log("Exiting Chase State");
    }
}
