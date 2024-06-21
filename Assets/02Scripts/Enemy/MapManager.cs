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

    public event Action OnChangeDestination;


    private void Start()
    {
        // �� y�� 1�ܰ�: 0.5, 2�ܰ�: 3.1, 3�ܰ�: 4.7, 4�ܰ�: 6.1 (��������)
        heightOfWater = new Dictionary<WaveName, float>
        {
            {WaveName.General, 0.5f },
            {WaveName.Trainee, 3.1f },
            {WaveName.Three, 4.7f },
            {WaveName.Four, 6.1f }
        };

        navMeshMap = map.GetComponent<NavMeshSurface>();

    }

    /// <summary>
    /// �� ���̸� �ø��� �ڷ�ƾ �Լ��� �����մϴ�.
    /// 1���������� �� ���� ���� �Լ��� ȣ������ �ʽ��ϴ�.
    /// </summary>
    /// <param name="level"></param>
    public void SetWaterHeightByLevel(int level)
    {
        if (level == 1)
        {
            return;
        }
        StartCoroutine(C_WaterUP((WaveName)level));
    }


    public IEnumerator C_WaterUP(WaveName wave)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Debug.Log("�� �ö��");

        OnChangeDestination?.Invoke();

        while (waterGround.transform.position.y < heightOfWater[wave])
        {

            waterGround.transform.position += new Vector3(0, 0.5f, 0);

            if (waterGround.transform.position.y > heightOfWater[wave])
            {
                waterGround.transform.position =
                    new Vector3(waterGround.transform.position.x, heightOfWater[wave], waterGround.transform.position.z);
            }

            yield return new WaitForSeconds(0.1f);
        }
        navMeshMap.BuildNavMesh();
        Debug.Log("�� ����");

        OnChangeDestination?.Invoke();

    }

    public void ChangeMap(WaveName wave)
    {
        waterGround.transform.position = new Vector3(waterGround.transform.position.x, heightOfWater[wave], waterGround.transform.position.z);

        // ������ Ȯ�� �ʿ�
        navMeshMap.BuildNavMesh();
    }

    public void StartWaterCoroutine(WaveName wave)
    {
        StartCoroutine(C_WaterUP(wave));
    }

    

    // ����� �� ȣ���� ���� �ʱ�ȭ�ϴ� �޼����Դϴ�.
    public void RestartMap(WaveName wave)
    {
        waterGround.transform.position = new Vector3(waterGround.transform.position.x, heightOfWater[WaveName.General], waterGround.transform.position.z);
        navMeshMap.BuildNavMesh();

    }

}