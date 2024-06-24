using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    private float randItem;

    public GameObject giantPrefab;
    public GameObject timestopPrefab;

    // 임시
    public bool isGiantEnd = false;

    public void ToggleGiantValue()
    {
        if (!isGiantEnd)
        {
            isGiantEnd = true;
        }
    }

    private void Start()
    {
        //Invoke("ItemSpawn", 3f);
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
            Instantiate(giantPrefab, transform.position, giantPrefab.transform.rotation);
        }
        // 영역전개 스폰
        else
        {
            Instantiate(timestopPrefab, transform.position, timestopPrefab.transform.rotation);
        }
    }

    /// <summary>
    /// 프로토타입용 순서대로 아이템 스폰하는 메서드입니다.
    /// 파라미터 받아서 레벨 판단 후 2레벨일 때 스폰합니다.
    /// </summary>
    /// <param name="level"></param>
    public void ItemSpawn_proto(int level)
    {
        if (level == 2)
        {
            StartCoroutine(C_proto_ItemSpawn());
        }

    }

    private IEnumerator C_proto_ItemSpawn()
    {
        yield return new WaitForSecondsRealtime(2f);
        Debug.Log("아이템");
        Instantiate(giantPrefab, transform.position, giantPrefab.transform.rotation);

        //거대화 끝나고 이벤트로 알려줌
        while (!isGiantEnd)
        {
            yield return null;
        }
        Debug.Log("거대화끝남");
        Instantiate(timestopPrefab, transform.position, timestopPrefab.transform.rotation);

    }

    // 맵 내 지속시간 설정

    // 아이템 생성을 위한 확률 계산
    // 기획서 내용에 따라 확률 수정
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
