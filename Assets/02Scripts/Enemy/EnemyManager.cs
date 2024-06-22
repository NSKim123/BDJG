using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

public enum WaveName
{
    General = 1,
    Trainee,
    Three,
    Four,
}

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

    public int TotalCount { get; set; }

    public WaveName currentWave;

    public Transform centerPos;

    // ****코드 정리 필요

    private void Start()
    {
        currentWave = WaveName.General;
        TotalCount = 0;

    }



}
