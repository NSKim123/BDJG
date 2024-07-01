using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCactus : Enemy
{
    public override float MoveSpeed { get; set; }
    public override float AttackRange { get; set; }
    public override float Damage_Distance { get; set; }
    public override Vector3 Damage_Direction { get; set; }
    public override float AttackForce { get; set; }
    public override float AttackSpeed { get; set ; }
    public override float AttackTime { get; set; }

    #region 특수개체의 특수공격용
    public override float SpecialAttackCoolTime { get; set; }
    public override float SpecialAttackRange { get; set; }
    public override float SpecialAttackTime { get; set; }
    #endregion

    public GameObject thornArea;

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
        //EnemyManager.Instance.TotalCount--;
        EnemySpawner.TotalSpecialEnemyCount--;
        EnemySpawner.CactusSpecialCount--;
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.StateInit(new Dictionary<State, EnemyStateBase>()
        {
            {State.Idle, new EnemyStateIdle(stateMachine) },
            {State.Move, new EnemyStateMove(stateMachine)},
            {State.Attack, new EnemyStateAttack(stateMachine)},
            {State.AttackSpecial, new EnemyStateAttackSpecial_Cactus(stateMachine)},
            {State.Hurt, new EnemyStateHurt(stateMachine)},
            {State.AvoidWater, new EnemyStateAvoidWater(stateMachine)},
            {State.Die, new EnemyStateDie(stateMachine)},
        });
    }

}
