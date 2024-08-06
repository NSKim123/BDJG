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
    // 생성할 오브젝트들의 데이터 리스트
    [SerializeField] private List<ObjectData> _objectDatum = new List<ObjectData>();
    // 타입별로 데이터를 저장하는 딕셔너리
    private Dictionary<EPoolType, ObjectData> _dataOfType = new Dictionary<EPoolType, ObjectData>();
    // 타입별로 풀을 저장하는 딕셔너리
    private Dictionary<EPoolType, Stack<GameObject>> _pools = new Dictionary<EPoolType, Stack<GameObject>>();


    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    /// <summary>
    /// 게임 시작 시 풀을 새로 생성합니다.
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
    /// 레벨업에 따라 탄환의 머터리얼을 변경합니다.
    /// </summary>
    /// <param name="newMaterial">바꿔줄 머터리얼(=슬라임의 머터리얼)</param>
    public void ChangeBulletMaterial(Material newMaterial)
    {
        // 원본 프리팹의 머터리얼 변경
        _dataOfType[EPoolType.Bullet].prefab.GetComponentInChildren<Renderer>().material = newMaterial;

        // 풀에 있는 탄환들의 머터리얼 변경
        foreach (var item in _pools[EPoolType.Bullet])
        {
            item.GetComponentInChildren<Renderer>().material = newMaterial;
        }

        // 풀에서 꺼내서 사용 중인 탄환들의 머터리얼 변경
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
    /// 풀을 만들고 오브젝트를 미리 생성해둡니다.
    /// </summary>
    /// <param name="data">생성할 오브젝트의 정보</param>
    private void CreatePool(ObjectData data)
    {
        if (_pools.ContainsKey(data.type))
        {
            return;
        }
       
        Stack<GameObject> pool = new Stack<GameObject>(data.maxCount);
       
        // 비활성화 후 스택에 push
        for (int i = 0; i < data.initialCount; i++)
        {
            GameObject obj = Instantiate(data.prefab);
            obj.SetActive(false);
            pool.Push(obj);
        }

        // 다 생성한 풀을 딕셔너리에 추가
        _pools.Add(data.type, pool);
        // 데이터 딕셔너리에 추가
        _dataOfType.Add(data.type, data);
    }

    /// <summary>
    /// 풀에서 오브젝트를 꺼내 사용할 때 호출합니다.
    /// </summary>
    /// <param name="type">풀의 이름(타입)</param>
    /// <returns></returns>
    public GameObject GetFromPool(EPoolType type)
    {
        if (!_pools.ContainsKey(type))
        {
            return null;
        }

        GameObject obj = null;

        // 풀에 오브젝트가 있다면 꺼냄
        if (_pools[type].Count > 0)
        {
            obj = _pools[type].Pop();

        }
        // 풀에 오브젝트가 없다면 새로 생성
        else
        {
            obj = Instantiate(_dataOfType[type].prefab);
        }

        obj.SetActive(false);

        return obj;
    }

    /// <summary>
    /// 오브젝트를 파괴할 때(풀로 반환할 때) 호출합니다.
    /// </summary>
    /// <param name="obj">회수할 오브젝트</param>
    public void ReturnToPool(GameObject obj)
    {
        EPoolType type = obj.GetComponent<KeyForObjectPool>().myPoolType;

        if (!_pools.ContainsKey(type))
        {
            return;
        }

        // 현재 풀에 들어갈 자리가 있으면
        if (_pools[type].Count < _dataOfType[type].maxCount)
        {
            _pools[type].Push(obj);
            obj.SetActive(false);
        }
        // 현재 풀에 들어갈 자리가 없으면
        else
        {
            Destroy(obj.gameObject);
        }
    }

    /// <summary>
    /// 게임 시작 시 사용 중인 오브젝트가 없도록 풀의 상태를 초기화합니다.
    /// </summary>
    public void ResetObjectPools()
    {
        ReturnActivatedObject("Enemy");
        ReturnActivatedObject("Bullet");
        ReturnActivatedObject("HitEffect");
    }

    /// <summary>
    /// 활성화 되어있는 오브젝트들을 풀로 반환합니다.
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

    
    // 풀에 들어갈 오브젝트의 정보
    [Serializable]
    public class ObjectData
    {
        public EPoolType type;  // 풀 타입
        public GameObject prefab;  // 원본 프리팹
        public int initialCount;  // 초기 생성 개수
        public int maxCount;  // 최대 생성 개수
    }
}


