using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// 싱글톤 베이스 스크립트
// DontDestroy는 상속받은 자식 클래스에서 필요할 시 구현
public class SingletonBase<T> : MonoBehaviour
       where T : MonoBehaviour
{
    protected static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)FindAnyObjectByType(typeof(T));

                if (_instance == null)
                {
                    GameObject obj = new GameObject (typeof(T).ToString());
                    _instance = obj.AddComponent<T>();
                }
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
