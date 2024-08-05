using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

public class MapController : MonoBehaviour
{
    // �� ��ü
    [SerializeField] private GameObject _waterGround;
    // �� ��ü
    [SerializeField] private GameObject _map;
    private NavMeshSurface _navMeshMap;

    //�� ���̸� �ܰ躰�� ������ �迭
    private float[] _heightOfWater;

    // �� ���̰� ����� �� ȣ���� �̺�Ʈ
    public event Action<int> OnChangeWave;

    private Coroutine _waterCoroutine;
    private float _waterDownDefaultSecond;
    private float _waterDownOffset;

    // ��� UI
    public GameObject Warning;
    private Animation _warningUIAnim;

    private void Start()
    {
        _navMeshMap = _map.GetComponent<NavMeshSurface>();
        _warningUIAnim = Warning.GetComponent<Animation>();
        _waterDownDefaultSecond = 40f;
        _waterDownOffset = 10f;

        // �� y�� ����
        _heightOfWater = new float[]
        {
            6.4f,
            4.7f,
            3.1f,
            1.5f,
        };
    }

    /// <summary>
    /// �ð��� ���� �� ���̸� �����ϴ� �ڷ�ƾ�Դϴ�.
    /// </summary>
    /// <returns></returns>
    public IEnumerator C_WaterDown()
    {
        int waterIndex = 0;

        _waterGround.transform.position = new Vector3(_waterGround.transform.position.x, _heightOfWater[waterIndex], _waterGround.transform.position.z);
        _navMeshMap.BuildNavMesh();

        // ���� �������� ������ �ܰ���� �ݺ�
        while (waterIndex < _heightOfWater.Length - 1)
        {
            // �Ʒ� �ð����� ���
            yield return new WaitForSeconds(_waterDownDefaultSecond + waterIndex * _waterDownOffset);
            waterIndex++;
            OnChangeWave?.Invoke(waterIndex);

            // ��� UI ǥ��
            _warningUIAnim.Play();
            while (_warningUIAnim.isPlaying)
            {
                yield return null;
            }
            
            // �� ���� ������
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
           
        }
 
    }

    /// <summary>
    /// �ڷ�ƾ�� �ߴ��ϰ� �ٽ� �����ϴ� �޼����Դϴ�.
    /// ���� ���� �� ȣ���մϴ�.
    /// </summary>
    public void ResetMapSetting()
    {
        if (_waterCoroutine != null)
        {
            StopCoroutine(_waterCoroutine);
            _waterCoroutine = null;
        }
        _waterCoroutine = StartCoroutine(C_WaterDown());
    }
}
