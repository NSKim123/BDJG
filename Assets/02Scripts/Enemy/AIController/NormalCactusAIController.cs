using Unity.VisualScripting;
using UnityEngine;

public class NormalCactusAIController : CactusAIController
{

    protected override void Update()
    {
        base.Update();

        if (isDetected)
        {
            AttackDetect = Physics.OverlapSphere(transform.position + Vector3.up * 1.8f, enemyCharacter.AttackRange, targetLayer);

            if (AttackDetect.Length > 0 && !attacked)
            {
                attacked = true;
                stateMachine.ChangeState(EState.Attack);
            }
        }

        if (stateMachine.currentStateType != EState.Attack && attacked)
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
