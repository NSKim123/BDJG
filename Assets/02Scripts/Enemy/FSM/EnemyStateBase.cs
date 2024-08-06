using UnityEngine;
using UnityEngine.AI;

public enum EState
{
    Init,
    Idle,
    Move,
    Attack,
    AttackSpecial,
    Hurt,
    AvoidWater,
    Die,
}

/// <summary>
/// 각 상태의 베이스 클래스입니다.
/// </summary>
public abstract class EnemyStateBase
{
    // 상태 내부에서의 진행 단계
    public enum EStepInState
    {
        None,
        Start,
        Playing,
        End
    }

    // 새로운 상태로의 전환 조건
    public abstract bool CanExecute();

    protected EStepInState _currentStep;

    // 필요한 컴포넌트 선언
    protected Rigidbody rigid;
    protected Animator animator;
    protected CapsuleCollider col;
    protected EnemyAIController enemyController;
    protected StateMachine stateMachine;
    protected Enemy enemyCharacter;
    protected NavMeshAgent enemyAgent;

    // 생성자에서 컴포넌트 초기화
    public EnemyStateBase(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.animator = stateMachine.GetComponent<Animator>();
        this.col = stateMachine.GetComponent<CapsuleCollider>();
        this.rigid = stateMachine.GetComponent<Rigidbody>();
        this.enemyCharacter = stateMachine.GetComponent<Enemy>();
        this.enemyController = stateMachine.GetComponent<EnemyAIController>();
        this.enemyAgent = stateMachine.GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// 상태 내부에서 다음 단계로 진행합니다.
    /// </summary>
    /// <returns>상태 반환</returns>
    public abstract EState MoveNextStep();

    /// <summary>
    /// 상태 내부의 단계를 초기화합니다.
    /// </summary>
    public void Reset()
    {
        _currentStep = EStepInState.None;
    }

}
