using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayer : MonoBehaviour
{
    Enemy enemy;

    private void Start()
    {
        Invoke("FindEnemy", 3);
        Invoke("Damage", 8);
    }

    private void FindEnemy()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();

    }

    private void Damage()
    {
        Debug.Log("플레이어에서 데미지");
        enemy.OnDamaged(5, -enemy.transform.forward);
    }
}
