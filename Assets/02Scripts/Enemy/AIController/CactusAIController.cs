using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CactusAIController : EnemyAIController
{
    private float _dis;
    private float _maxDis = 7.0f;

    protected bool isDetected = false;

    protected override void OnDisable()
    {
        base.OnDisable();
        isDetected = false;
    }

    protected override void Update()
    {
        base.Update();

        _dis = Vector3.Distance(transform.position, Target.transform.position);

        // idle 상태이며 허수아비 아이템 사용 중일 때
        if (stateMachine.currentStateType == EState.Idle && Target.TryGetComponent(out Scarecrow _))
        {
            stateMachine.ChangeState(EState.Move);
            isDetected = true;
        }

        // idle 상태일 때 플레이어를 감지했다면 이동 시작
        if (_dis <= _maxDis && !isDetected && stateMachine.currentStateType == EState.Idle)
        {
            isDetected = true;
            stateMachine.ChangeState(EState.Move);
        }
    }
}
