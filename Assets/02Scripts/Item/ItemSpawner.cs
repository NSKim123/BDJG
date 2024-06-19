using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    private float randItem;

    public GameObject giantPrefab;
    public GameObject timestopPrefab;

    // �ӽ�
    public bool isGiantEnd = false;

    private void Start()
    {
        //Invoke("ItemSpawn", 3f);
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

    // ������Ÿ�Կ� ������� ����
    public void ItemSpawn_proto(int level)
    {
        StartCoroutine(C_proto_ItemSpawn());
    }

    private IEnumerator C_proto_ItemSpawn()
    {
        Instantiate(giantPrefab, transform.position, Quaternion.identity);

        while (isGiantEnd)
        {
            yield return null;
            //�Ŵ�ȭ ������ �̺�Ʈ �Ǵ� �÷��׷� �˷���
        }
        Instantiate(timestopPrefab, transform.position, Quaternion.identity);

    }

    // �� �� ���ӽð� ����

    // ������ ������ ���� Ȯ�� ���
    // ��ȹ�� ���뿡 ���� Ȯ�� ����
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
