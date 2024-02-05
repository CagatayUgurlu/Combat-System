using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State<EnemyController>
{
    EnemyController enemy;
    public override void Enter(EnemyController owner)
    {
        enemy = owner;
        //Debug.Log("Entered Idle State");
    }

    public override void Execute()
    {
        /*Debug.Log("Executing Idle State");
        if (Input.GetKeyDown(KeyCode.T))
        {
            enemy.ChangeState(EnemyStates.Chase);
            //enemy.StateMachine.ChangeState(enemy.GetComponent<ChaseState>());
        }*/
        foreach (var target in enemy.TargetsInRange)
        {
            var vectorToTarget = target.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, vectorToTarget);

            if (angle <= enemy.fieldOfView / 2)
            {
                enemy.Target = target;
                enemy.ChangeState(EnemyStates.Chase);
                break;
            }
        }
    }

    public override void Exit()
    {
        //Debug.Log("Exiting Idle State");
    }
}
