using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

public class MapController : MonoBehaviour
{
    // 물 객체
    [SerializeField] private GameObject _waterGround;
    // 맵 객체
    [SerializeField] private GameObject _map;
    private NavMeshSurface _navMeshMap;

    //물 높이를 단계별로 저장한 배열
    private float[] _heightOfWater;

    // 물이 높아질 때 호출할 이벤트
    // 적의 이동방향을 중앙으로 변경
    public event Action OnChangeDestination;
    public event Action<int> OnChangeWave;

    private Coroutine _waterCoroutine;
    private float _waterDownDefaultSecond;
    private float _waterDownOffset;

    // 경고 UI
    public GameObject warning;
    private Animation _warningUIAnim;

    Wave currentWave;

    private void Start()
    {
        _navMeshMap = _map.GetComponent<NavMeshSurface>();
        _warningUIAnim = warning.GetComponent<Animation>();
        _waterDownDefaultSecond = 40f;
        _waterDownOffset = 10f;

        // 물 y축 높이 1단계: 1.5, 2단계: 3.1, 3단계: 4.7, 4단계: 6.4 (변동가능)
        _heightOfWater = new float[]
        {
            6.4f,
            4.7f,
            3.1f,
            1.5f,
        };
    }

    /// <summary>
    /// 시간에 따라 물 높이를 조절하는 코루틴입니다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator C_WaterUP()
    {
        int waterIndex = 0;

        _waterGround.transform.position = new Vector3(_waterGround.transform.position.x, _heightOfWater[waterIndex], _waterGround.transform.position.z);
        _navMeshMap.BuildNavMesh();

        while (waterIndex < _heightOfWater.Length - 1)
        {
            
            // time for water rising
            yield return new WaitForSeconds(_waterDownDefaultSecond + waterIndex * _waterDownOffset);

            waterIndex++;

            OnChangeWave?.Invoke(waterIndex);

            // Show warning UI
            _warningUIAnim.Play();

            while (_warningUIAnim.isPlaying)
            {
                yield return null;
            }
            
            

            // 적의 이동방향 변경 (중심으로)
            //OnChangeDestination?.Invoke();

            while (_waterGround.transform.position.y > _heightOfWater[waterIndex])
            {
                _waterGround.transform.position -= new Vector3(0, 0.1f, 0);

                if (_waterGround.transform.position.y < _heightOfWater[waterIndex])
                {
                    _waterGround.transform.position =
                        new Vector3(_waterGround.transform.position.x, _heightOfWater[waterIndex], _waterGround.transform.position.z);
                }

                yield return new WaitForSeconds(0.1f);
            }
            _navMeshMap.BuildNavMesh();


            // 적의 이동방향 변경 (플레이어쪽으로)
            //OnChangeDestination?.Invoke();
        }
 
    }

    /// <summary>
    /// 코루틴을 중단하고 다시 시작하는 메서드입니다.
    /// 게임 시작 시 호출합니다.
    /// </summary>
    public void StartMapSetting()
    {
        if (_waterCoroutine != null)
        {
            StopCoroutine(_waterCoroutine);
            _waterCoroutine = null;
        }
        _waterCoroutine = StartCoroutine(C_WaterUP());
    }
}
