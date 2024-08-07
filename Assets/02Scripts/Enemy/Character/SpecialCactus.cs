using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCactus : Enemy
{

    protected override void Start()
    {
        base.Start();

        stateMachine.InitState(new Dictionary<EState, EnemyStateBase>()
        {
            {EState.Init, new EnemyStateInit(stateMachine)},
            {EState.Idle, new EnemyStateIdle(stateMachine) },
            {EState.Move, new EnemyStateMove(stateMachine)},
            {EState.Attack, new EnemyStateAttack(stateMachine)},
            {EState.AttackSpecial, new EnemyStateAttackSpecial_Cactus(stateMachine)},
            {EState.Hurt, new EnemyStateHurt(stateMachine)},
            {EState.Die, new EnemyStateDie(stateMachine)},
        });
    }

    protected void OnEnable()
    {
        if (isInitialized)
        {
            stateMachine.InitState(new Dictionary<EState, EnemyStateBase>()
            {
                {EState.Init, new EnemyStateInit(stateMachine)},
                {EState.Idle, new EnemyStateIdle(stateMachine) },
                {EState.Move, new EnemyStateMove(stateMachine)},
                {EState.Attack, new EnemyStateAttack(stateMachine)},
                {EState.AttackSpecial, new EnemyStateAttackSpecial_Cactus(stateMachine)},
                {EState.Hurt, new EnemyStateHurt(stateMachine)},
                {EState.Die, new EnemyStateDie(stateMachine)},
            });
        }
    }

}
