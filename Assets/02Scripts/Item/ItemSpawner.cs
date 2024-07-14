using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    private float rate;
    private float normalRate = 0.2f;
    private float specialRate = 0.5f;

    private float itemSpawnRadius = 8.0f;
    [SerializeField] private Transform itemSpawnAxis;
    [SerializeField] private GameObject randomItem;
    private int _currentItemCount = 0;
    private int _maxItemCount = 4;

 
    private void ItemCountDecrease()
    {
        _currentItemCount--;
    }

    private void ItemCountIncrease()
    {
        _currentItemCount++;
    }

    private bool IsItemsFullInMap()
    {
        return _currentItemCount >= _maxItemCount;
    }

    private void Update()
    {
        if (Time.timeScale == 0.0f)
        {
            return;
        }
    }


    public void ItemSpawnByPercentage(EnemyType type)
    {
        rate = UnityEngine.Random.Range(0.0f, 1.0f);

        // 스폰된 아이템이 이미 최대 개수만큼 있다면 리턴
        if (IsItemsFullInMap())
        {
            return;
        }
        
        // 일반 개체
        if ((int)type == 0 || (int)type == 1)
        {
            if (rate >= 0 && rate <= normalRate)
            {
                Vector3 spawnPos = UtilSpawn.GetRandomPositionOnCircleEdge(itemSpawnAxis.position, itemSpawnRadius);
                GameObject obj = Instantiate(randomItem, spawnPos, Quaternion.identity);
                obj.GetComponent<RandomItem>().onItemCountDecrease += ItemCountDecrease;
                ItemCountIncrease();
            }

        }
        //특수 개체
        else if ((int)type == 2 || (int)type == 3)
        {
            if (rate>=0 && rate <= specialRate)
            {
                Vector3 spawnPos = UtilSpawn.GetRandomPositionOnCircleEdge(itemSpawnAxis.position, itemSpawnRadius);
                GameObject obj = Instantiate(randomItem, spawnPos, Quaternion.identity);
                obj.GetComponent<RandomItem>().onItemCountDecrease += ItemCountDecrease;
                ItemCountIncrease();
            }
        }
    }

    public void ResetItems()
    {
        UtilReset.DestroyActivatedItems("Item");
        _currentItemCount = 0;
    }
}