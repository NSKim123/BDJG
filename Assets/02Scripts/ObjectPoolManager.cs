using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private List<ObjectData> objectDatum = new List<ObjectData>();

    private Dictionary<PoolType, ObjectData> dataOfType = new Dictionary<PoolType, ObjectData>();
    private Dictionary<PoolType, Stack<GameObject>> pools = new Dictionary<PoolType, Stack<GameObject>>();

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

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

    private void CreatePool(ObjectData data)
    {
        if (pools.ContainsKey(data.type))
        {
            return;
        }

        pools[data.type] = new Stack<GameObject>();

        for (int i = 0; i < data.initialCount; i++)
        {
            GameObject obj = Instantiate(data.prefab);
            obj.SetActive(false);
            pools[data.type].Push(obj);
        }

        dataOfType[data.type] = data;
    }

    public GameObject GetFromPool(PoolType type)
    {
        if (!pools.ContainsKey(type))
        {
            return null;
        }

        GameObject obj;

        if (pools[type].Count > 0)
        {
            obj = pools[type].Pop();
        }
        else
        {
            obj = Instantiate(dataOfType[type].prefab);
            //obj.SetActive(false);
        }

        if (obj != null)
        {
            obj.SetActive(false);
        }
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        PoolType type = obj.GetComponent<KeyForObjectPool>().myPoolType;
        
        if (!pools.ContainsKey(type) || obj.GetType() != dataOfType[type].prefab.GetType())
        {
            return;
        }

        if (pools[type].Count < dataOfType[type].maxCount)
        {
            pools[type].Push(obj);
            obj.SetActive(false);
        }
        // 
        else
        {
            Destroy(obj.gameObject);
        }
    }

    




    [Serializable]
    public class ObjectData
    {
        public PoolType type;
        public GameObject prefab;
        public int initialCount;
        public int maxCount;
    }
}


