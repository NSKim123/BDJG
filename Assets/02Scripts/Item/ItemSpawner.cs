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
        // �� ���� �������� �ִٸ� ���� �����մϴ�.
        GameObject[] removeList = isItemExistInMap();
        if (removeList != null)
        {
            foreach (var item in removeList)
            {
                Destroy(item);
            }
        }

        // �Ŵ�ȭ ���� / ���� ���� ��ġ ����
        if (RandomResult() > 0.5f)
        {
            Instantiate(giantPrefab, transform.position, Quaternion.identity);
        }
        // �������� ����
        else
        {
            Instantiate(timestopPrefab, transform.position, Quaternion.identity);
        }
    }

    // ������ ������ ���� Ȯ�� ���
    // ��ȹ�� ���뿡 ���� Ȯ�� ����
    // ������Ÿ�Կ����� �ݹ� Ȯ�� ����
    private float RandomResult()
    {
        randItem = Random.Range(0.0f, 1.0f);
        return randItem;
    }

    // �������� �� �ȿ� �ִ��� Ȯ���մϴ�.
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
