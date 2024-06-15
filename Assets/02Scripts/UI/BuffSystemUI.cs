using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystemUI : MonoBehaviour
{

    [Header("# 버프 아이콘 프리팹")]
    public BuffIconUI m_BuffIconUIPrefab;

    private BuffSystem _TargetBuffSystem;

    private List<BuffIconUI> _CurrentBuffIconList = new();



    private void OnBuffStarted(Buff startedBuff)
    {
        if (!startedBuff.visibility) return;

        BuffIconUI buffIconUI = Instantiate(m_BuffIconUIPrefab);
        buffIconUI.InitIcon(startedBuff);
        buffIconUI.transform.SetParent(this.gameObject.transform);

        (buffIconUI.transform as RectTransform).localScale = Vector3.one;

        _CurrentBuffIconList.Add(buffIconUI);
    }

    private void OnBuffRenew(Buff renewBuff)
    {
        BuffIconUI buffIconUI = _CurrentBuffIconList.Find((buffIconUI) => buffIconUI.CheckIconUIByBuffCode(renewBuff.buffCode));

        if (buffIconUI != null)
        {
            buffIconUI.StopBlink();
        }
    }

    private void OnBuffNotMuchLeft(Buff notMuchLeftBuff)
    {
        BuffIconUI buffIconUI = _CurrentBuffIconList.Find((buffIconUI) => buffIconUI.CheckIconUIByBuffCode(notMuchLeftBuff.buffCode));

        if (buffIconUI != null)
        {
            buffIconUI.StartBlink();
        }
    }

    private void OnBuffFinished(Buff finishedBuff)
    {
        BuffIconUI buffIconUI = _CurrentBuffIconList.Find((buffIconUI) => buffIconUI.CheckIconUIByBuffCode(finishedBuff.buffCode));

        if(buffIconUI != null)
        {
            Destroy(buffIconUI.gameObject);
        }
    }    

    private void InitList()
    {
        int count = 0;
        foreach(Buff buff in _TargetBuffSystem.buffList)
        {
            OnBuffStarted(buff);
            ++count;

            if (count == 5)
                break;
        }
    }

    private void ClearUIList()
    {
        foreach (BuffIconUI buffIconUI in _CurrentBuffIconList)
        {
            Destroy(buffIconUI.gameObject);
        }
    }

    public void SetTargetBuffSystem(BuffSystem newTarget)
    {
        if(_TargetBuffSystem != null)
        {
            _TargetBuffSystem.onBuffStarted -= OnBuffStarted;
            _TargetBuffSystem.onBuffRenew -= OnBuffRenew;
            _TargetBuffSystem.onBuffNotMuchLeft -= OnBuffNotMuchLeft;
            _TargetBuffSystem.onBuffFinished -= OnBuffFinished;
        }       

        ClearUIList();

        _TargetBuffSystem = newTarget;

        _TargetBuffSystem.onBuffStarted += OnBuffStarted;
        _TargetBuffSystem.onBuffRenew += OnBuffRenew;
        _TargetBuffSystem.onBuffNotMuchLeft += OnBuffNotMuchLeft;
        _TargetBuffSystem.onBuffFinished += OnBuffFinished;

        InitList();
    }
}
