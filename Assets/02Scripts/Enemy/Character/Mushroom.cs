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
    public override EEnemyType Type { get; set; }
    #endregion

    public override void OnDamaged(float distance, Vector3 direction)
    {
        base.OnDamaged(distance, direction);
        Damage_Distance = distance;
        Damage_Direction = direction;
        stateMachine.ChangeState_AllowSameState(EState.Hurt);
    }

    public override void OnDead()
    {
        base.OnDead();
        stateMachine.ChangeState(EState.Die);
        EnemySpawner.TotalEnemyCount--;
    }

    protected override void Start()
    {
        base.Start();
        InitStateDictionary();

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        InitStateDictionary();
    }

    /// <summary>
    /// 필요한 State를 추가합니다.
    /// </summary>
    private void InitStateDictionary()
    {
        stateMachine.InitState(new Dictionary<EState, EnemyStateBase>()
        {
            {EState.Init, new EnemyStateInit(stateMachine)},
            {EState.Idle, new EnemyStateIdle(stateMachine)},
            {EState.Move, new EnemyStateMove(stateMachine)},
            {EState.Attack, new EnemyStateAttack(stateMachine)},
            {EState.Hurt, new EnemyStateHurt(stateMachine)},
            {EState.Die, new EnemyStateDie(stateMachine)},
        });
    }

}
