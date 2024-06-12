using UnityEngine;
using UnityEngine.AI;

public enum State
{
    Move,
    Attack,
    Hurt,
    Die,
}


public abstract class EnemyStateBase
{
    public enum StepInState
    {
        None,
        Start,
        Playing,
        End
    }


    public abstract bool canExecute();

    public StepInState currentStep => _currentStep;

    protected StepInState _currentStep;

    // 필요한 컴포넌트 선언
    protected Rigidbody rigid;
    protected Animator animator;
    protected CapsuleCollider collider;
    protected EnemyAIController enemyController;
    protected StateMachine stateMachine;
    protected Enemy enemyCharacter;
    protected NavMeshAgent enemyAgent;


    public EnemyStateBase(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.animator = stateMachine.GetComponent<Animator>();
        this.collider = stateMachine.GetComponent<CapsuleCollider>();
        this.rigid = stateMachine.GetComponent<Rigidbody>();
        this.enemyCharacter = stateMachine.GetComponent<Enemy>();
        this.enemyController = stateMachine.GetComponent<EnemyAIController>();
        this.enemyAgent = stateMachine.GetComponent<NavMeshAgent>();
    }

    public abstract State MoveNextStep();

    public void Reset()
    {
        _currentStep = StepInState.None;
    }
}
