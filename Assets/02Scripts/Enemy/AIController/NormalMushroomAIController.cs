using UnityEngine;

public class NormalMushroomAIController : MushroomAIController
{

    protected override void Update()
    {
        base.Update();

        AttackDetect = Physics.OverlapSphere(transform.position + Vector3.up * 1.8f, enemyCharacter.AttackRange, targetLayer);

        // 타켓(플레이어) 감지
        if (AttackDetect.Length > 0 && !attacked)
        {
            attacked = true;
            stateMachine.ChangeState(EState.Attack);
        }

        if (stateMachine.currentStateType == EState.Move && attacked)
        {
            attacked = false;
        }
    }
   

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (enemyCharacter != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position + Vector3.up * 1.8f, enemyCharacter.AttackRange);
        }
    }
#endif


}

