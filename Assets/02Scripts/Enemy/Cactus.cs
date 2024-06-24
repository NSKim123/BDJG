using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : Enemy
{
    public override float MoveSpeed { get; set; }
    public override float DetectPlayerDistance { get; set; }
    public override float Damage_Distance { get; set; }
    public override Vector3 Damage_Direction { get; set; }
    public override float AttackForce { get; set; }
    public override float AttackSpeed { get; set ; }
    public override float AttackTime { get; set; }


    public override void OnDamaged(float distance, Vector3 direction)
    {
        base.OnDamaged(distance, direction);
        Damage_Distance = distance;
        Damage_Direction = direction;
        stateMachine.ChangeState_AllowSameState(State.Hurt);
    }

    public override void OnDead()
    {
        base.OnDead();
        stateMachine.ChangeState(State.Die);
        EnemyManager.Instance.TotalCount--;
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.StateInit(new List<EnemyStateBase>()
        {
            new EnemyStateIdle(stateMachine),
            new EnemyStateMove(stateMachine),
            new EnemyStateAttack(stateMachine),
            new EnemyStateHurt(stateMachine),
            new EnemyStateAvoidWater(stateMachine),
            new EnemyStateDie(stateMachine)
        });
    }

}
