using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class KeyForObjectPool : MonoBehaviour
{
    public PoolType myPoolType = PoolType.None;

    public KeyForObjectPool Clone()
    {
        GameObject go = Instantiate(gameObject);
        if (!go.TryGetComponent(out KeyForObjectPool po))
            po = go.AddComponent<KeyForObjectPool>();
        go.SetActive(false);

        return po;
    }

    /// <summary> 게임오브젝트 활성화 </summary>
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    /// <summary> 게임오브젝트 비활성화 </summary>
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
