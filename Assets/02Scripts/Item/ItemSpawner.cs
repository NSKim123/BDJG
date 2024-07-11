using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    private float rate;
    private float normalRate = 1.0f;
    private float specialRate = 1.0f;

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
        //Debug.Log(_currentItemCount + " 개");
        //Debug.Log(IsItemsFullInMap());
    }


    public void ItemSpawnByPercentage(EnemyType type)
    {
        rate = UnityEngine.Random.Range(0.0f, 1.0f);

        if (IsItemsFullInMap())
        {
            return;
        }

        // 일반 개체
        if ((int)type == 0 || (int)type == 1)
        {
            if (rate >= 0 && rate <= normalRate)
            {
                Vector3 spawnPos = GetRandomPositionOnCircleEdge(itemSpawnAxis.position, itemSpawnRadius);
                GameObject obj = Instantiate(randomItem, spawnPos, Quaternion.identity);
                obj.GetComponent<RandomItem>().onItemCountDecrease += ItemCountDecrease;
            }

        }
        //특수 개체
        else if ((int)type == 2 || (int)type == 3)
        {
            if (rate>=0 || rate <= specialRate)
            {
                Vector3 spawnPos = GetRandomPositionOnCircleEdge(itemSpawnAxis.position, itemSpawnRadius);
                GameObject obj = Instantiate(randomItem, spawnPos, Quaternion.identity);
                obj.GetComponent<RandomItem>().onItemCountDecrease += ItemCountDecrease;

            }
        }

        ItemCountIncrease();

    }

    public void ResetItem()
    {
        // 맵 내에 아이템이 있다면 제거합니다.
        GameObject[] removeList = isItemExistInMap();
        if (removeList != null)
        {
            foreach (var item in removeList)
            {
                Destroy(item);
            }
        }

        _currentItemCount = 0;
    }

    // 아이템이 맵 안에 있는지 확인합니다.
    private GameObject[] isItemExistInMap()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");

        if (items.Length > 0)
        {
            return items;
        }
        else
        {
            return null;
        }
    }

    Vector3 GetRandomPositionOnCircleEdge(Vector3 center, float radius)
    {
        float angle = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad; // 각도를 라디안으로 변환
        float x = center.x + radius * Mathf.Cos(angle);
        float z = center.z + radius * Mathf.Sin(angle);
        return new Vector3(x, center.y, z); // 중심의 y값을 그대로 유지
    }
}
