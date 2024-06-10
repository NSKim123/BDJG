using UnityEngine;

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

    // �ʿ��� ������Ʈ ����
    protected Rigidbody rigid;
    protected Animator animator;
    protected CapsuleCollider collider;
    protected EnemyAIController enemyController;
    protected StateMachine stateMachine;
    protected Enemy enemyCharacter;
    // player ����

    public EnemyStateBase(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.animator = stateMachine.GetComponent<Animator>();
        this.collider = stateMachine.GetComponent<CapsuleCollider>();
        this.rigid = stateMachine.GetComponent<Rigidbody>();
        this.enemyCharacter = stateMachine.GetComponent<Enemy>();
        this.enemyController = stateMachine.GetComponent<EnemyAIController>();
    }

    public abstract State MoveNextStep();

    public void Reset()
    {
        _currentStep = StepInState.None;
    }
}
