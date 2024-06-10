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

    public override float AttackTime { get; set; }


    public override void OnDamaged(float distance, Vector3 direction)
    {
        Damage_Distance = distance;
        Damage_Direction = direction;
        stateMachine.ChangeState(State.Hurt);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.StateInit(new List<EnemyStateBase>()
        {
            new EnemyStateMove(stateMachine),
            new EnemyStateAttack(stateMachine),
            new EnemyStateHurt(stateMachine),
            new EnemyStateDie(stateMachine)
        });
    }

}
