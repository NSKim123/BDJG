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
    private Dictionary<PoolType, List<GameObject>> poolList = new Dictionary<PoolType, List<GameObject>>();

    protected override void Awake()
    {
        base.Awake();
        //Init();
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
        if (poolList.ContainsKey(data.type))
        {
            return;
        }

        //pools[data.type] = new Stack<GameObject>(data.maxCount);
        poolList[data.type] = new List<GameObject>(data.maxCount);

        for (int i = 0; i < data.initialCount; i++)
        {
            GameObject obj = Instantiate(data.prefab);
            obj.SetActive(false);
            //pools[data.type].Push(obj);
            poolList[data.type].Add(obj);
        }

        dataOfType[data.type] = data;
    }

    public GameObject GetFromPool(PoolType type)
    {
        if (!poolList.ContainsKey(type))
        {
            return null;
        }

        GameObject obj = null;

        foreach (var spawnObj in poolList[type])
        {
            if (!spawnObj.activeInHierarchy)
            {
                return spawnObj;
            }
          
        }

        //if (obj == null)
        //{
        //    Instantiate(dataOfType[type].prefab);
        //}

        return null;


        //if (pools[type].Count > 0)
        //{
        //    obj = pools[type].Pop();
        //    if (obj == null)
        //    {
        //        Debug.Log($"Äç if null {type}, {pools[type]}");
        //    }
        //}
        //else
        //{
        //    obj = Instantiate(dataOfType[type].prefab);
        //    //obj.SetActive(false);
        //}

        //if (obj != null)
        //{
        //    obj.SetActive(false);
        //}
        //return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        PoolType type = obj.GetComponent<KeyForObjectPool>().myPoolType;
        
        if (!poolList.ContainsKey(type))
        {
            return;
        }

        obj.SetActive(false);
        

        //if (pools[type].Count < dataOfType[type].maxCount)
        //{
        //    pools[type].Push(obj);
        //    obj.SetActive(false);
        //}
        //// 
        //else
        //{
        //    Destroy(obj.gameObject);
        //}
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


