using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class EnemyAIController : MonoBehaviour
{
    protected StateMachine _stateMachine;
    protected Enemy _enemyCharacter;

    public GameObject target;

    [SerializeField] protected LayerMask _targetLayer;
    [SerializeField] protected bool _attacked = false;


    public abstract Collider[] AttackDetect { get;}

    private MapManager _mapManager;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        _stateMachine = GetComponent<StateMachine>();
        _enemyCharacter = GetComponent<Enemy>();
        _mapManager = FindAnyObjectByType<MapManager>();
        _mapManager.OnChangeDestination += ChangeDestination;

    }




    public abstract void ChangeDestination();
}




