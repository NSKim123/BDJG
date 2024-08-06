using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class EnemyAIController : MonoBehaviour
{
    protected StateMachine _stateMachine;
    protected Enemy _enemyCharacter;

    public GameObject Target;

    [SerializeField] protected LayerMask _targetLayer;
    [SerializeField] protected bool _attacked = false;

    // ������ �÷��̾�
    public abstract Collider[] AttackDetect { get; }

    private MapController _mapManager;

    private void Start()
    {
        Init();
    }

    // ������ƮǮ�� �����ϱ⶧���� �ʿ�
    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        Target = GameObject.FindGameObjectWithTag("Player");
        _stateMachine = GetComponent<StateMachine>();
        _enemyCharacter = GetComponent<Enemy>();
        _mapManager = FindAnyObjectByType<MapController>();
    }
}




