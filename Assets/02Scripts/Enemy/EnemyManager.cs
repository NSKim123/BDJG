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

public class EnemyManager : SingletonBase<EnemyManager>
{
    public EnemySpawner spawner;

    public int MushroomCount { get; set; }
    public int CactusCount { get; set; }

    public int TotalCount { get; set; }


    public WaveName currentWave;

    public Transform centerPos;




}
