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
/// �� ������ ���̽� Ŭ�����Դϴ�.
/// </summary>
public abstract class EnemyStateBase
{
    // ���� ���ο����� ���� �ܰ�
    public enum EStepInState
    {
        None,
        Start,
        Playing,
        End
    }

    // ���ο� ���·��� ��ȯ ����
    public abstract bool CanExecute();

    protected EStepInState _currentStep;

    // �ʿ��� ������Ʈ ����
    protected Rigidbody rigid;
    protected Animator animator;
    protected CapsuleCollider col;
    protected EnemyAIController enemyController;
    protected StateMachine stateMachine;
    protected Enemy enemyCharacter;
    protected NavMeshAgent enemyAgent;

    // �����ڿ��� ������Ʈ �ʱ�ȭ
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
    /// ���� ���ο��� ���� �ܰ�� �����մϴ�.
    /// </summary>
    /// <returns>���� ��ȯ</returns>
    public abstract EState MoveNextStep();

    /// <summary>
    /// ���� ������ �ܰ踦 �ʱ�ȭ�մϴ�.
    /// </summary>
    public void Reset()
    {
        _currentStep = EStepInState.None;
    }

}
