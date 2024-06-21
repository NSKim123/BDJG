using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 버프 시스템을 화면에 표시하는 UI에 대한 컴포넌트입니다.
/// </summary>
public class BuffSystemUI : MonoBehaviour
{
    [Header("# 버프 아이콘 프리팹")]
    public BuffIconUI m_BuffIconUIPrefab;

    /// <summary>
    /// 이 UI가 어느 버프 시스템을 표시할지 나타내는 변수
    /// </summary>
    private BuffSystem _TargetBuffSystem;

    /// <summary>
    /// 현재 표시되고 있는 버프 아이콘 UI들을 담고 있는 리스트
    /// </summary>
    private List<BuffIconUI> _CurrentBuffIconList = new();

    /// <summary>
    /// 버프가 시작될 때 호출될 메서드입니다.
    /// BuffSystem 의 onBuffStarted 이벤트에 연결
    /// </summary>
    /// <param name="startedBuff"> 시작된 버프</param>
    private void OnBuffStarted(Buff startedBuff)
    {
        // 이 버프가 UI에 표시되는 버프가 아니라면 함수 호출 종료
        if (!startedBuff.visibility) return;

        // 버프 아이콘 프리팹을 복사 생성하고 설정합니다.
        BuffIconUI buffIconUI = Instantiate(m_BuffIconUIPrefab);
        buffIconUI.InitIcon(startedBuff);
        buffIconUI.transform.SetParent(this.gameObject.transform);
        (buffIconUI.transform as RectTransform).localScale = Vector3.one;

        // 현재 버프 아이콘 리스트에 추가합니다.
        _CurrentBuffIconList.Add(buffIconUI);
    }

    /// <summary>
    /// 버프가 갱신될 때 호출될 메서드입니다.
    /// BuffSystem 의 onBuffRenew 이벤트에 연결
    /// </summary>
    /// <param name="renewBuff"> 갱신된 버프</param>
    private void OnBuffRenew(Buff renewBuff)
    {
        // 이 버프가 UI에 표시되는 버프가 아니라면 함수 호출 종료
        if (!renewBuff.visibility) return;

        // 이 버프에 해당하는 버프 아이콘 UI를 찾습니다.
        BuffIconUI buffIconUI = _CurrentBuffIconList.Find((buffIconUI) => buffIconUI.CheckIconUIByBuffCode(renewBuff.buffCode));

        // 아이콘 점멸을 중단합니다.
        buffIconUI.StopBlink();        
    }

    /// <summary>
    /// 버프가 업데이트될 때 호출될 메서드입니다.
    /// BuffSystem 의 onBuffUpdated 이벤트에 연결
    /// </summary>
    /// <param name="updatedBuff"> 업데이트된 버프</param>
    private void OnBuffUpdated(Buff updatedBuff)
    {
        // 이 버프가 UI에 표시되는 버프가 아니라면 함수 호출 종료
        if (!updatedBuff.visibility) return;

        // 이 버프에 해당하는 버프 아이콘 UI를 찾습니다.
        BuffIconUI buffIconUI = _CurrentBuffIconList.Find((buffIconUI) => buffIconUI.CheckIconUIByBuffCode(updatedBuff.buffCode));

        // 점멸 조건을 만족한다면
        if(updatedBuff.notMuchLeft)
        {
            // 아이콘 점멸 시작
            //buffIconUI.StartBlink();
        }
    }

    /// <summary>
    /// 버프가 끝났을 때 호출될 메서드입니다.
    /// BuffSystem 의 onBuffFinished 이벤트에 연결 
    /// </summary>
    /// <param name="finishedBuff"> 끝난 버프</param>
    private void OnBuffFinished(Buff finishedBuff)
    {
        // 이 버프가 UI에 표시되는 버프가 아니라면 함수 호출 종료
        if (!finishedBuff.visibility) return;

        // 이 버프에 해당하는 버프 아이콘 UI를 찾습니다.
        BuffIconUI buffIconUI = _CurrentBuffIconList.Find((buffIconUI) => buffIconUI.CheckIconUIByBuffCode(finishedBuff.buffCode));

        // 이 아이콘 UI를 제거합니다.
        RemoveBuffIconUI(buffIconUI);
    }    

    /// <summary>
    /// 버프 리스트에 있는 내용으로 아이콘 UI 리스트를 생성하는 메서드입니다.
    /// </summary>
    private void InitList()
    {  
        // 버프 리스트에 있는 버프들을 차례대로 가져옵니다.
        foreach(Buff buff in _TargetBuffSystem.buffList)
        {
            // 버프 아이콘 실행
            OnBuffStarted(buff);
        }
    }

    /// <summary>
    /// 현재 표시되고 있는 버프 아이콘들을 모두 지우는 메서드입니다.
    /// </summary>
    private void ClearUIList()
    {
        foreach (BuffIconUI buffIconUI in _CurrentBuffIconList)
        {
            RemoveBuffIconUI(buffIconUI);
        }        
    }

    private void RemoveBuffIconUI(BuffIconUI buffIconUI)
    {
        _CurrentBuffIconList.Remove(buffIconUI);
        Destroy(buffIconUI.gameObject);
    }

    /// <summary>
    /// 표시할 버프 시스템에 이벤트를 등록하는 메서드입니다.
    /// </summary>
    private void BindEvents()
    {
        _TargetBuffSystem.onBuffStarted += OnBuffStarted;
        _TargetBuffSystem.onBuffRenew += OnBuffRenew;
        _TargetBuffSystem.onBuffUpdated += OnBuffUpdated;
        _TargetBuffSystem.onBuffFinished += OnBuffFinished;
    }

    /// <summary>
    /// 표시할 버프 시스템에 이벤트를 등록 해제 메서드입니다.
    /// </summary>
    private void UnbindEvents()
    {
        _TargetBuffSystem.onBuffStarted -= OnBuffStarted;
        _TargetBuffSystem.onBuffRenew -= OnBuffRenew;
        _TargetBuffSystem.onBuffUpdated -= OnBuffUpdated;
        _TargetBuffSystem.onBuffFinished -= OnBuffFinished;
    }


    /// <summary>
    /// 표시할 버프 시스템을 재설정합니다.
    /// </summary>
    /// <param name="newTarget"> 새로운 버프 시스템</param>
    public void SetTargetBuffSystem(BuffSystem newTarget)
    {
        // 현재 표시하고 있는 버프 시스템이 있다면
        if(_TargetBuffSystem != null)
        {
            // 이벤트 바인딩 해제
            UnbindEvents();
        }       

        // UI를 모두 지웁니다.
        ClearUIList();

        // 새로 표시할 버프 시스템을 저장합니다.
        _TargetBuffSystem = newTarget;

        // 이벤트 바인딩
        BindEvents();

        // 버프 아이콘들을 다시 생성합니다.
        InitList();
    }
}
