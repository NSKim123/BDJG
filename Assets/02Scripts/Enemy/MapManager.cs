using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject waterGround;

    private Dictionary<WaveName, float> heightOfWater;
    [SerializeField] private GameObject map;
    [SerializeField] private NavMeshSurface navMeshMap;

    private void Start()
    {
        // 물 y값 1단계: 0.5, 2단계: 3.1, 3단계: 4.7, 4단계: 6.1 (변동가능)
        heightOfWater = new Dictionary<WaveName, float>
        {
            {WaveName.General, 0.5f },
            {WaveName.Trainee, 3.1f },
            {WaveName.Three, 4.7f },
            {WaveName.Four, 6.1f }
        };

        navMeshMap = map.GetComponent<NavMeshSurface>();

    }

    public void SetWaterHeightByLevel(int level)
    {
        StartCoroutine(C_WaterUP((WaveName)level));
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

    

    // 재시작 시 호출할 맵을 초기화하는 메서드입니다.
    public void RestartMap(WaveName wave)
    {
        if (wave != WaveName.General)
        {
            waterGround.transform.position = new Vector3(waterGround.transform.position.x, heightOfWater[WaveName.General], waterGround.transform.position.z);
        }
        navMeshMap.BuildNavMesh();

    }

}
