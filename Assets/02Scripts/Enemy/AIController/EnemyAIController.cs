using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyAIController : MonoBehaviour
{
    protected StateMachine stateMachine;
    protected Enemy enemyCharacter;

    public GameObject Target;

    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected bool attacked = false;

    // 감지된 플레이어
    public Collider[] AttackDetect { get; set; }


    private void Start()
    {
        Init();
    }

    protected virtual void OnDisable()
    {
        attacked = false;
    }

    protected virtual void Update()
    {
        if (Time.timeScale == 0.0f)
        {
            return;
        }
    }

    private void Init()
    {
        Target = GameObject.FindGameObjectWithTag("Player");
        stateMachine = GetComponent<StateMachine>();
        enemyCharacter = GetComponent<Enemy>();
    }
}




