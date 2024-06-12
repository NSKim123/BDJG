using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance
    {
        get
        {
            return _instance;
        }
    }
    private static EnemyManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int MushroomCount { get; set; }
    public int CactusCount { get; set; }


    // 슬라임 성장 정보 -> 웨이브 상태
    // 게임 오버 등 상태


}
