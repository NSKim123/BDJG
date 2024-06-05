using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    EnemyAIController aiController;
    EnemyMovement enemyMove;
    EnemyAttack enemyAttack;


    private void Start()
    {
        aiController = GetComponent<EnemyAIController>();
        enemyMove = GetComponent<EnemyMovement>();
        enemyAttack = GetComponent<EnemyAttack>();

        aiController.onStartMove += enemyMove.MoveToPlayer;
        aiController.onStartAttack += enemyAttack.AttackPlayer;
    }


   

}

