using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    // �Ϲ� ��ü ������ Ȯ��
    private float normalRate = 0.2f;
    // Ư�� ��ü ������ Ȯ��
    private float specialRate = 0.5f;

    private float itemSpawnRadius = 8.0f;
    [SerializeField] private Transform itemSpawnAxis;
    [SerializeField] private GameObject randomItem;
    private int _currentItemCount = 0;
    private int _maxItemCount = 4;


    private void DecreaseItemCount()
    {
        _currentItemCount--;
    }

    private void IncreaseItemCount()
    {
        _currentItemCount++;
    }

    private bool IsItemsFullInMap()
    {
        return _currentItemCount >= _maxItemCount;
    }


    /// <summary>
    /// Ȯ���� ���� �������� �����մϴ�.
    /// </summary>
    /// <param name="type">���� ���� Ÿ��</param>
    public void SpawnItemByProbability(EEnemyType type)
    {
        // ������ �������� �̹� �ִ� ������ŭ �ִٸ� ����
        if (IsItemsFullInMap())
        {
            return;
        }

        float rate = Random.Range(0.0f, 1.0f);

        // �Ϲ� ��ü
        if ((int)type == 0 || (int)type == 1)
        {
            if (rate >= 0 && rate <= normalRate)
            {
                InstantiateNewItem();
                IncreaseItemCount();
            }

        }
        //Ư�� ��ü
        else if ((int)type == 2 || (int)type == 3)
        {
            if (rate >= 0 && rate <= specialRate)
            {
                InstantiateNewItem();
                IncreaseItemCount();
            }
        }
    }

    /// <summary>
    /// �� �������� �����մϴ�.
    /// </summary>
    private void InstantiateNewItem()
    {
        Vector3 spawnPos = UtilSpawn.GetRandomPositionOnCircle(itemSpawnAxis.position, itemSpawnRadius);
        GameObject obj = Instantiate(randomItem, spawnPos, Quaternion.identity);
        obj.GetComponent<RandomItem>().onItemCountDecrease += DecreaseItemCount;
    }

    /// <summary>
    /// ���� ����(����� ��) �ʿ� �������� �ִٸ� �ı��մϴ�.
    /// </summary>
    public void ResetItems()
    {
        UtilReset.DestroyActivatedItems("Item");
        _currentItemCount = 0;
    }
}