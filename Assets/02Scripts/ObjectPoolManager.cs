using System;
using System.Collections.Generic;
using UnityEngine;


public enum PoolType
{
    None,
    Bullet,
    Mushroom,
    Cactus,
    SpecialMushroom,
    SpecialCactus,
    Effect_Hit,

}

public class ObjectPoolManager : SingletonBase<ObjectPoolManager>
{
    // ������ ������Ʈ���� ������ ����Ʈ
    [SerializeField] private List<ObjectData> objectDatum = new List<ObjectData>();
    // Ÿ�Ժ��� �����͸� �����ϴ� ��ųʸ�
    private Dictionary<PoolType, ObjectData> dataOfType = new Dictionary<PoolType, ObjectData>();
    // Ÿ�Ժ��� Ǯ�� �����ϴ� ��ųʸ�
    private Dictionary<PoolType, Stack<GameObject>> pools = new Dictionary<PoolType, Stack<GameObject>>();


    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    /// <summary>
    /// ���� ���� �� Ǯ�� ���� �����մϴ�.
    /// </summary>
    private void Init()
    {
        if (objectDatum.Count == 0)
        {
            return;
        }

        foreach (var data in objectDatum)
        {
            CreatePool(data);
        }

    }

    /// <summary>
    /// �������� ���� źȯ�� ���͸����� �����մϴ�.
    /// </summary>
    /// <param name="newMaterial">�ٲ��� ���͸���(=�������� ���͸���)</param>
    public void ChangeBulletMaterial(Material newMaterial)
    {
        // ���� �������� ���͸��� ����
        dataOfType[PoolType.Bullet].prefab.GetComponentInChildren<Renderer>().material = newMaterial;

        // Ǯ�� �ִ� źȯ���� ���͸��� ����
        foreach (var item in pools[PoolType.Bullet])
        {
            item.GetComponentInChildren<Renderer>().material = newMaterial;
        }

        // Ǯ���� ������ ��� ���� źȯ���� ���͸��� ����
        GameObject[] bullets = UtilReset.IsElementsExistInMap("Bullet");
        if (bullets != null)
        {
            foreach (var item in bullets)
            {
                item.GetComponentInChildren<Renderer>().material = newMaterial;
            }
        }
        

    }

    /// <summary>
    /// Ǯ�� ����� ������Ʈ�� �̸� �����صӴϴ�.
    /// </summary>
    /// <param name="data">������ ������Ʈ�� ����</param>
    private void CreatePool(ObjectData data)
    {
        if (pools.ContainsKey(data.type))
        {
            return;
        }
       
        Stack<GameObject> pool = new Stack<GameObject>(data.maxCount);
       
        // ��Ȱ��ȭ �� ���ÿ� push
        for (int i = 0; i < data.initialCount; i++)
        {
            GameObject obj = Instantiate(data.prefab);
            obj.SetActive(false);
            pool.Push(obj);
        }

        // �� ������ Ǯ�� ��ųʸ��� �߰�
        pools.Add(data.type, pool);
        // ������ ��ųʸ��� �߰�
        dataOfType.Add(data.type, data);
    }

    /// <summary>
    /// Ǯ���� ������Ʈ�� ���� ����� �� ȣ���մϴ�.
    /// </summary>
    /// <param name="type">Ǯ�� �̸�(Ÿ��)</param>
    /// <returns></returns>
    public GameObject GetFromPool(PoolType type)
    {
        if (!pools.ContainsKey(type))
        {
            return null;
        }

        GameObject obj = null;

        // Ǯ�� ������Ʈ�� �ִٸ� ����
        if (pools[type].Count > 0)
        {
            obj = pools[type].Pop();
            //if (obj == null)
            //{
            //    Debug.Log($"�� if null {type}, {pools[type]}");
            //}

        }
        // Ǯ�� ������Ʈ�� ���ٸ� ���� ����
        else
        {
            obj = Instantiate(dataOfType[type].prefab);
        }

        obj.SetActive(false);

        return obj;
    }

    /// <summary>
    /// ������Ʈ�� �ı��� ��(Ǯ�� ��ȯ�� ��) ȣ���մϴ�.
    /// </summary>
    /// <param name="obj">ȸ���� ������Ʈ</param>
    public void ReturnToPool(GameObject obj)
    {
        PoolType type = obj.GetComponent<KeyForObjectPool>().myPoolType;

        if (!pools.ContainsKey(type))
        {
            return;
        }

        //Debug.Log($"��ȯ {obj.name} ");

        // ���� Ǯ�� �� �ڸ��� ������
        if (pools[type].Count < dataOfType[type].maxCount)
        {
            pools[type].Push(obj);
            obj.SetActive(false);
        }
        // ���� Ǯ�� �� �ڸ��� ������
        else
        {
            Destroy(obj.gameObject);
        }
    }

    /// <summary>
    /// ���� ���� �� ��� ���� ������Ʈ�� ������ Ǯ�� ���¸� �ʱ�ȭ�մϴ�.
    /// </summary>
    public void ResetObjectPools()
    {
        ReturnActivatedObject("Enemy");
        ReturnActivatedObject("Bullet");
        ReturnActivatedObject("HitEffect");
    }

    /// <summary>
    /// Ȱ��ȭ �Ǿ��ִ� ������Ʈ���� Ǯ�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="objects"></param>
    private void ReturnActivatedObject(string tag)
    {
        GameObject[] returnList = UtilReset.IsElementsExistInMap(tag);
        if (returnList != null)
        {
            foreach (var item in returnList)
            {
                ObjectPoolManager.Instance.ReturnToPool(item);
            }
        }
    }

    
    // Ǯ�� �� ������Ʈ�� ����
    [Serializable]
    public class ObjectData
    {
        public PoolType type;  // Ǯ Ÿ��
        public GameObject prefab;  // ���� ������
        public int initialCount;  // �ʱ� ���� ����
        public int maxCount;  // �ִ� ���� ����
    }
}


