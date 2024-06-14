using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using static EnemySpawner;

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

    public GameObject waterGround;

    // 슬라임 성장 정보 -> 웨이브 상태
 

    // 물 y값 1단계: 0.5, 2단계: 3.1, 3단계: 4.7, 4단계: 6.1 (변동가능)

    private Dictionary<WaveName, float> heightOfWater;
    [SerializeField] private GameObject map;
    [SerializeField] private NavMeshSurface navMeshMap;

    private void Start()
    {
        heightOfWater = new Dictionary<WaveName, float>
        {
            {WaveName.General, 0.5f },
            {WaveName.Trainee, 3.1f },
            {WaveName.Three, 4.7f },
            {WaveName.Four, 6.1f }
        };

        navMeshMap = map.GetComponent<NavMeshSurface>();

    }

    public void ChangeMap(WaveName wave)
    {
        waterGround.transform.position = new Vector3(waterGround.transform.position.x, heightOfWater[wave], waterGround.transform.position.z);
        
        // 딜레이 확인 필요
        navMeshMap.BuildNavMesh();
    }

    public void StartWaterCoroutine(WaveName wave)
    {
        StartCoroutine(C_WaterUP(wave));
    }

    public IEnumerator C_WaterUP(WaveName wave)
    {
        while (waterGround.transform.position.y < heightOfWater[wave])
        {
            waterGround.transform.position += new Vector3(0, 0.1f, 0);

            yield return new WaitForSeconds(0.1f);
        }
        navMeshMap.BuildNavMesh();
    }

    // 적 스폰 초기화 - 적 싹 밀고 다시 첨부터
    // 맵 물 올라갔던 거 다시 밑으로


}
