

public class MushroomAIController : EnemyAIController
{
    protected override void Update()
    {
        base.Update();

        // 버섯은 idle 상태를 거치지않음. 바로 Move로 전환.
        if (stateMachine.currentStateType == EState.Idle)
        {
            stateMachine.ChangeState(EState.Move);
        }
    }
}
