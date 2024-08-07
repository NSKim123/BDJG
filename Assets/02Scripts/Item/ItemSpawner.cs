using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    // 일반 개체 아이템 확률
    private float normalRate = 0.2f;
    // 특수 개체 아이템 확률
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
    /// 확률에 따라 아이템을 생성합니다.
    /// </summary>
    /// <param name="type">죽은 적의 타입</param>
    public void SpawnItemByProbability(EEnemyType type)
    {
        // 스폰된 아이템이 이미 최대 개수만큼 있다면 리턴
        if (IsItemsFullInMap())
        {
            return;
        }

        float rate = Random.Range(0.0f, 1.0f);

        // 일반 개체
        if ((int)type == 0 || (int)type == 1)
        {
            if (rate >= 0 && rate <= normalRate)
            {
                InstantiateNewItem();
                IncreaseItemCount();
            }

        }
        //특수 개체
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
    /// 새 아이템을 생성합니다.
    /// </summary>
    private void InstantiateNewItem()
    {
        Vector3 spawnPos = UtilSpawn.GetRandomPositionOnCircle(itemSpawnAxis.position, itemSpawnRadius);
        GameObject obj = Instantiate(randomItem, spawnPos, Quaternion.identity);
        obj.GetComponent<RandomItem>().onItemCountDecrease += DecreaseItemCount;
    }

    /// <summary>
    /// 게임 시작(재시작 시) 맵에 아이템이 있다면 파괴합니다.
    /// </summary>
    public void ResetItems()
    {
        UtilReset.DestroyActivatedItems("Item");
        _currentItemCount = 0;
    }
}