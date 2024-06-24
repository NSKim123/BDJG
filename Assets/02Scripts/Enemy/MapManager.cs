using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject waterGround;

    private Dictionary<WaveName, float> heightOfWater;
    [SerializeField] private GameObject map;
    private NavMeshSurface navMeshMap;

    public event Action OnChangeDestination;

    private Coroutine waterCoroutine;

    public GameObject warning;

    private Animation warninganim;

    private void Start()
    {
        warninganim = warning.GetComponent<Animation>();

        // �� y�� 1�ܰ�: 0.5, 2�ܰ�: 3.1, 3�ܰ�: 4.7, 4�ܰ�: 6.1 (��������)
        heightOfWater = new Dictionary<WaveName, float>
        {
            {WaveName.General, 1.5f },
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
        //StartCoroutine(C_WaterUP((WaveName)level));
    }



    public IEnumerator C_WaterUP()
    {
        WaveName wave = WaveName.General;
        waterGround.transform.position = new Vector3(waterGround.transform.position.x, heightOfWater[wave], waterGround.transform.position.z);
        navMeshMap.BuildNavMesh();

        while ((int)wave < Enum.GetValues(typeof(WaveName)).Length)
        {
            yield return new WaitForSeconds(45.0f);

            warninganim.Play();

            while (warninganim.isPlaying)
            {
                yield return null;
            }

            wave++;

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
           // Debug.Log("�� ����");

            OnChangeDestination?.Invoke();
        }
 
    }

    public void ChangeMap(WaveName wave)
    {
        waterGround.transform.position = new Vector3(waterGround.transform.position.x, heightOfWater[wave], waterGround.transform.position.z);

        // ������ Ȯ�� �ʿ�
        navMeshMap.BuildNavMesh();
    }

    public void StartMapSetting()
    {
        if (waterCoroutine != null)
        {
            StopCoroutine(waterCoroutine);
            waterCoroutine = null;
        }
        waterCoroutine = StartCoroutine(C_WaterUP());
    }



    // ����� �� ȣ���� ���� �ʱ�ȭ�ϴ� �޼����Դϴ�.
    public void RestartMap(WaveName wave)
    {
        waterGround.transform.position = new Vector3(waterGround.transform.position.x, heightOfWater[wave], waterGround.transform.position.z);
        navMeshMap.BuildNavMesh();

    }

}
