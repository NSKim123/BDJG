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

    public event Action<WaveName> onChangeWave;
    public delegate void CoroutineStarter(WaveName wave);
    public event CoroutineStarter OnCoroutineStart;

    public EnemySpawner enemySpawnerObject;
    public MapManager mapManageObject;



    public GameObject waterGround;



    // 슬라임 성장 정보 -> 웨이브 상태


    // ****코드 정리 필요
    public WaveName currentWave;

    private Dictionary<WaveName, float> heightOfWater;
    [SerializeField] private GameObject map;
    [SerializeField] private NavMeshSurface navMeshMap;

    private void Start()
    {
        currentWave = WaveName.General;

        // 물 y값 1단계: 0.5, 2단계: 3.1, 3단계: 4.7, 4단계: 6.1 (변동가능)
        heightOfWater = new Dictionary<WaveName, float>
        {
            {WaveName.General, 0.5f },
            {WaveName.Trainee, 3.1f },
            {WaveName.Three, 4.7f },
            {WaveName.Four, 6.1f }
        };

        navMeshMap = map.GetComponent<NavMeshSurface>();

        onChangeWave += mapManageObject.ChangeMap;

    }


    public void ChangeMap(WaveName wave)
    {
        waterGround.transform.position = new Vector3(waterGround.transform.position.x, heightOfWater[wave], waterGround.transform.position.z);
        
        // 딜레이 확인 필요
        navMeshMap.BuildNavMesh();
    }

    public void StartWaterCoroutine(WaveName wave)
    {
        //StartCoroutine(C_WaterUP(wave));
    }

    public IEnumerator C_WaterUP(int wave)
    {
        while (waterGround.transform.position.y < heightOfWater[(WaveName)wave])
        {
            waterGround.transform.position += new Vector3(0, 0.1f, 0);
            // 물 트리거 확인해서 닿는애들 죽기
            yield return new WaitForSeconds(0.1f);
        }
        navMeshMap.BuildNavMesh();
    }

    public void SetWaterHeightByLevel(int level)
    {
        StartCoroutine(C_WaterUP(level));
    }

}
