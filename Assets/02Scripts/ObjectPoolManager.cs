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
    private Dictionary<PoolType, KeyForObjectPool> _sampleDict = new Dictionary<PoolType, KeyForObjectPool>();


    private Dictionary<PoolType, List<GameObject>> poolList = new Dictionary<PoolType, List<GameObject>>();

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

    public void ChangeBulletMaterial(Material newMaterial)
    {
        dataOfType[PoolType.Bullet].prefab.GetComponentInChildren<Renderer>().material = newMaterial;

        foreach (var item in pools[PoolType.Bullet])
        {
            item.GetComponentInChildren<Renderer>().material = newMaterial;
        }

        GameObject[] bullets = IsElementsExistInMap("Bullet");
        if (bullets != null)
        {
            foreach (var item in bullets)
            {
                item.GetComponentInChildren<Renderer>().material = newMaterial;
            }
        }
        

    }

    private void CreatePool(ObjectData data)
    {
        if (poolList.ContainsKey(data.type))
        {
            return;
        }

       
        Stack<GameObject> pool = new Stack<GameObject>(data.maxCount);
        //poolList[data.type] = new List<GameObject>(data.maxCount);

        for (int i = 0; i < data.initialCount; i++)
        {

            GameObject obj = Instantiate(data.prefab);
            obj.SetActive(false);
            pool.Push(obj);
            //poolList[data.type].Add(obj);
        }

        pools.Add(data.type, pool);
        dataOfType.Add(data.type, data);
        //dataOfType[data.type] = data;
    }

    public GameObject GetFromPool(PoolType type)
    {


        if (!pools.ContainsKey(type))
        {
            return null;
        }

        GameObject obj = null;


        if (pools[type].Count > 0)
        {
            obj = pools[type].Pop();
            if (obj == null)
            {
                Debug.Log($"Äç if null {type}, {pools[type]}");
            }

        }
        else
        {
            obj = Instantiate(dataOfType[type].prefab);
        }

        obj.SetActive(false);

        if (obj != null)
        {
        }

        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {

        PoolType type = obj.GetComponent<KeyForObjectPool>().myPoolType;

        if (!pools.ContainsKey(type))
        {
            return;
        }


        //Debug.Log($"¹ÝÈ¯ {obj.name} ");


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

    public void ResetObjectPools()
    {
        ReturnRemainedObject(IsElementsExistInMap("Enemy"));
        ReturnRemainedObject(IsElementsExistInMap("Bullet"));
        ReturnRemainedObject(IsElementsExistInMap("HitEffect"));
    }

    private void ReturnRemainedObject(GameObject[] objects)
    {
        if (objects != null)
        {
            foreach (var item in objects)
            {
                ObjectPoolManager.Instance.ReturnToPool(item);
            }
        }
    }

    private GameObject[] IsElementsExistInMap(string tag)
    {
        GameObject[] elements = GameObject.FindGameObjectsWithTag(tag);



        if (elements.Length > 0)
        {
            return elements;
        }
        else
        {
            return null;
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


