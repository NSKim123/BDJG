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
            Instantiate(giantPrefab, transform.position, giantPrefab.transform.rotation);
        }
        // �������� ����
        else
        {
            Instantiate(timestopPrefab, transform.position, timestopPrefab.transform.rotation);
        }
    }

    /// <summary>
    /// ������Ÿ�Կ� ������� ������ �����ϴ� �޼����Դϴ�.
    /// �Ķ���� �޾Ƽ� ���� �Ǵ� �� 2������ �� �����մϴ�.
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
        Debug.Log("������");
        Instantiate(giantPrefab, transform.position, giantPrefab.transform.rotation);

        //�Ŵ�ȭ ������ �̺�Ʈ�� �˷���
        while (!isGiantEnd)
        {
            yield return null;
        }
        Debug.Log("�Ŵ�ȭ����");
        Instantiate(timestopPrefab, transform.position, timestopPrefab.transform.rotation);

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
