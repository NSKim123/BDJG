using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    
    private float randItem;

    public GameObject giantPrefab;
    public GameObject timestopPrefab;

    private void Start()
    {
        Invoke("ItemSpawn", 3f);
    }

    public void ItemSpawn()
    {
        // 맵 내에 아이템이 있다면 먼저 제거합니다.
        GameObject[] removeList = isItemExistInMap();
        if (removeList != null)
        {
            foreach (var item in removeList)
            {
                Destroy(item);
            }
        }

        // 거대화 스폰 / 추후 랜덤 위치 적용
        if (RandomResult() > 0.5f)
        {
            Instantiate(giantPrefab, transform.position, Quaternion.identity);
        }
        // 영역전개 스폰
        else
        {
            Instantiate(timestopPrefab, transform.position, Quaternion.identity);
        }
    }

    // 아이템 생성을 위한 확률 계산
    // 기획서 내용에 따라 확률 수정
    // 프로토타입에서는 반반 확률 적용
    private float RandomResult()
    {
        randItem = Random.Range(0.0f, 1.0f);
        return randItem;
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
}
