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
  

    public void ItemSpawnByPercentage(EnemyType type)
    {
        rate = UnityEngine.Random.Range(0.0f, 1.0f);

        // �Ϲ� ��ü
        if ((int)type == 0 || (int)type == 1)
        {
            if (rate >= 0 && rate <= normalRate)
            {
                Vector3 spawnPos = GetRandomPositionOnCircleEdge(itemSpawnAxis.position, itemSpawnRadius);
                Instantiate(randomItem, spawnPos, Quaternion.identity);
            }

        }
        //Ư�� ��ü
        else if ((int)type == 2 || (int)type == 3)
        {
            if (rate>=0 || rate <= specialRate)
            {
                Vector3 spawnPos = GetRandomPositionOnCircleEdge(itemSpawnAxis.position, itemSpawnRadius);
                Instantiate(randomItem, spawnPos, Quaternion.identity);
            }
        }

    }

    public void ResetItem()
    {
        // �� ���� �������� �ִٸ� �����մϴ�.
        GameObject[] removeList = isItemExistInMap();
        if (removeList != null)
        {
            foreach (var item in removeList)
            {
                Destroy(item);
            }
        }
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

    Vector3 GetRandomPositionOnCircleEdge(Vector3 center, float radius)
    {
        float angle = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad; // ������ �������� ��ȯ
        float x = center.x + radius * Mathf.Cos(angle);
        float z = center.z + radius * Mathf.Sin(angle);
        return new Vector3(x, center.y, z); // �߽��� y���� �״�� ����
    }
}
