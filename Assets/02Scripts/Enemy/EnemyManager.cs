using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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


    // ������ ���� ���� -> ���̺� ����
    // ���� ���� �� ����


}
