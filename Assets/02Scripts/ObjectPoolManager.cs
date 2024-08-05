using System;
using System.Collections.Generic;
using UnityEngine;


public enum EPoolType
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
    [SerializeField] private List<ObjectData> _objectDatum = new List<ObjectData>();
    // Ÿ�Ժ��� �����͸� �����ϴ� ��ųʸ�
    private Dictionary<EPoolType, ObjectData> _dataOfType = new Dictionary<EPoolType, ObjectData>();
    // Ÿ�Ժ��� Ǯ�� �����ϴ� ��ųʸ�
    private Dictionary<EPoolType, Stack<GameObject>> _pools = new Dictionary<EPoolType, Stack<GameObject>>();


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
        if (_objectDatum.Count == 0)
        {
            return;
        }

        foreach (var data in _objectDatum)
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
        _dataOfType[EPoolType.Bullet].prefab.GetComponentInChildren<Renderer>().material = newMaterial;

        // Ǯ�� �ִ� źȯ���� ���͸��� ����
        foreach (var item in _pools[EPoolType.Bullet])
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
        if (_pools.ContainsKey(data.type))
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
        _pools.Add(data.type, pool);
        // ������ ��ųʸ��� �߰�
        _dataOfType.Add(data.type, data);
    }

    /// <summary>
    /// Ǯ���� ������Ʈ�� ���� ����� �� ȣ���մϴ�.
    /// </summary>
    /// <param name="type">Ǯ�� �̸�(Ÿ��)</param>
    /// <returns></returns>
    public GameObject GetFromPool(EPoolType type)
    {
        if (!_pools.ContainsKey(type))
        {
            return null;
        }

        GameObject obj = null;

        // Ǯ�� ������Ʈ�� �ִٸ� ����
        if (_pools[type].Count > 0)
        {
            obj = _pools[type].Pop();

        }
        // Ǯ�� ������Ʈ�� ���ٸ� ���� ����
        else
        {
            obj = Instantiate(_dataOfType[type].prefab);
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
        EPoolType type = obj.GetComponent<KeyForObjectPool>().myPoolType;

        if (!_pools.ContainsKey(type))
        {
            return;
        }

        // ���� Ǯ�� �� �ڸ��� ������
        if (_pools[type].Count < _dataOfType[type].maxCount)
        {
            _pools[type].Push(obj);
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
        public EPoolType type;  // Ǯ Ÿ��
        public GameObject prefab;  // ���� ������
        public int initialCount;  // �ʱ� ���� ����
        public int maxCount;  // �ִ� ���� ����
    }
}


