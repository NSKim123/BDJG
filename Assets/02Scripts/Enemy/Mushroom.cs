using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]

public class Mushroom : Enemy
{
    public override float MoveSpeed { get; set; }
    public override float DetectPlayerDistance { get;  set; }

    public override float Damage_Distance { get; set; }

    public override Vector3 Damage_Direction { get; set; }
   
    public override float AttackForce { get; set; }

    public override float AttackTime { get; set; }

    public override void OnDamaged(float distance, Vector3 direction)
    {
        //Debug.Log("����");
        Damage_Distance = distance;
        Damage_Direction = direction;
        stateMachine.ChangeState(State.Hurt);
        
        //Debug.Log(stateMachine.currentStateType);
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

        //Debug.Log(MoveSpeed + "�ӵ�");
        
         
    }
}