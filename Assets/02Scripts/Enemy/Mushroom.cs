using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Mushroom : Enemy
{
    public override float MoveSpeed { get; set; }
    public override float AttackRange { get;  set; }

    public override float Damage_Distance { get; set; }

    public override Vector3 Damage_Direction { get; set; }
   
    public override float AttackForce { get; set; }

    public override float AttackTime { get; set; }
    public override float AttackSpeed { get; set; }

    #region 특수개체의 특수공격용, 일반개체 사용X
    public override float SpecialAttackCoolTime { get; set; }
    public override float SpecialAttackRange { get; set; }
    public override float SpecialAttackTime { get; set; }
    public override EnemyType Type { get; set; }
    #endregion

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
        EnemySpawner.TotalEnemyCount--;
    }

    protected override void Start()
    {
        base.Start();
        //stateMachine.StateInit(new List<EnemyStateBase>()
        //{
        //    new EnemyStateIdle(stateMachine),
        //    new EnemyStateMove(stateMachine),
        //    new EnemyStateAttack(stateMachine),
        //    new EnemyStateHurt(stateMachine),
        //    new EnemyStateAvoidWater(stateMachine),
        //    new EnemyStateDie(stateMachine)
        //});
        stateMachine.StateInit(new Dictionary<State, EnemyStateBase>()
        {
            {State.Init, new EnemyStateInit(stateMachine)},
            {State.Idle, new EnemyStateIdle(stateMachine) },
            {State.Move, new EnemyStateMove(stateMachine)},
            {State.Attack, new EnemyStateAttack(stateMachine)},
            {State.Hurt, new EnemyStateHurt(stateMachine)},
            {State.AvoidWater, new EnemyStateAvoidWater(stateMachine)},
            {State.Die, new EnemyStateDie(stateMachine)},
        });

    }
}
