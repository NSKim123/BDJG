using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : Enemy
{

    public override void OnDead()
    {
        base.OnDead();
        EnemySpawner.totalEnemyCount--;
    }

    protected override void Start()
    {
        base.Start();

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

    protected void OnEnable()
    {
        if (isInitialized)
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

}
