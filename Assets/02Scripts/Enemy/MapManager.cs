using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    // �� ��ü
    [SerializeField] private GameObject _waterGround;
    // �� ��ü
    [SerializeField] private GameObject _map;
    private NavMeshSurface _navMeshMap;

    //�� ���̸� �ܰ躰�� ������ �迭
    private float[] _heightOfWater;

    // ���� ������ �� ȣ���� �̺�Ʈ
    // ���� �̵������� �߾����� ����
    public event Action OnChangeDestination;

    private Coroutine _waterCoroutine;
    private WaitForSeconds _waitSecForWaterUp;

    // ��� UI
    public GameObject warning;
    private Animation _warningAnim;


    private void Start()
    {
        _navMeshMap = _map.GetComponent<NavMeshSurface>();
        _warningAnim = warning.GetComponent<Animation>();
        _waitSecForWaterUp = new WaitForSeconds(45.0f);

        // �� y�� ���� 1�ܰ�: 1.5, 2�ܰ�: 3.1, 3�ܰ�: 4.7, 4�ܰ�: 6.4 (��������)
        _heightOfWater = new float[]
        {
            1.5f,
            3.1f,
            4.7f,
            6.4f
        };
    }

    /// <summary>
    /// �ð��� ���� �� ���̸� �����ϴ� �ڷ�ƾ�Դϴ�.
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
            yield return _waitSecForWaterUp;

            // Show warning UI
            _warningAnim.Play();

            while (_warningAnim.isPlaying)
            {
                yield return null;
            }

            waterIndex++;

            // ���� �̵����� ���� (�߽�����)
            OnChangeDestination?.Invoke();

            while (_waterGround.transform.position.y < _heightOfWater[waterIndex])
            {
                _waterGround.transform.position += new Vector3(0, 0.3f, 0);

                if (_waterGround.transform.position.y > _heightOfWater[waterIndex])
                {
                    _waterGround.transform.position =
                        new Vector3(_waterGround.transform.position.x, _heightOfWater[waterIndex], _waterGround.transform.position.z);
                }

                yield return new WaitForSeconds(0.1f);
            }
            _navMeshMap.BuildNavMesh();
            // Debug.Log("�� ����");

            // ���� �̵����� ���� (�÷��̾�������)
            OnChangeDestination?.Invoke();
        }
 
    }

    /// <summary>
    /// �ڷ�ƾ�� �ߴ��ϰ� �ٽ� �����ϴ� �޼����Դϴ�.
    /// ���� ���� �� ȣ���մϴ�.
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
