using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �ý����� ȭ�鿡 ǥ���ϴ� UI�� ���� ������Ʈ�Դϴ�.
/// </summary>
public class BuffSystemUI : MonoBehaviour
{
    [Header("# ���� ������ ������")]
    public BuffIconUI m_BuffIconUIPrefab;

    /// <summary>
    /// �� UI�� ��� ���� �ý����� ǥ������ ��Ÿ���� ����
    /// </summary>
    private BuffSystem _TargetBuffSystem;

    /// <summary>
    /// ���� ǥ�õǰ� �ִ� ���� ������ UI���� ��� �ִ� ����Ʈ
    /// </summary>
    private List<BuffIconUI> _CurrentBuffIconList = new();

    /// <summary>
    /// ������ ���۵� �� ȣ��� �޼����Դϴ�.
    /// BuffSystem �� onBuffStarted �̺�Ʈ�� ����
    /// </summary>
    /// <param name="startedBuff"> ���۵� ����</param>
    private void OnBuffStarted(Buff startedBuff)
    {
        // �� ������ UI�� ǥ�õǴ� ������ �ƴ϶�� �Լ� ȣ�� ����
        if (!startedBuff.visibility) return;

        // ���� ������ �������� ���� �����ϰ� �����մϴ�.
        BuffIconUI buffIconUI = Instantiate(m_BuffIconUIPrefab);
        buffIconUI.InitIcon(startedBuff);
        buffIconUI.transform.SetParent(this.gameObject.transform);
        (buffIconUI.transform as RectTransform).localScale = Vector3.one;

        // ���� ���� ������ ����Ʈ�� �߰��մϴ�.
        _CurrentBuffIconList.Add(buffIconUI);
    }

    /// <summary>
    /// ������ ���ŵ� �� ȣ��� �޼����Դϴ�.
    /// BuffSystem �� onBuffRenew �̺�Ʈ�� ����
    /// </summary>
    /// <param name="renewBuff"> ���ŵ� ����</param>
    private void OnBuffRenew(Buff renewBuff)
    {
        // �� ������ UI�� ǥ�õǴ� ������ �ƴ϶�� �Լ� ȣ�� ����
        if (!renewBuff.visibility) return;

        // �� ������ �ش��ϴ� ���� ������ UI�� ã���ϴ�.
        BuffIconUI buffIconUI = _CurrentBuffIconList.Find((buffIconUI) => buffIconUI.CheckIconUIByBuffCode(renewBuff.buffCode));

        // ������ ������ �ߴ��մϴ�.
        buffIconUI.StopBlink();        
    }

    /// <summary>
    /// ������ ������Ʈ�� �� ȣ��� �޼����Դϴ�.
    /// BuffSystem �� onBuffUpdated �̺�Ʈ�� ����
    /// </summary>
    /// <param name="updatedBuff"> ������Ʈ�� ����</param>
    private void OnBuffUpdated(Buff updatedBuff)
    {
        // �� ������ UI�� ǥ�õǴ� ������ �ƴ϶�� �Լ� ȣ�� ����
        if (!updatedBuff.visibility) return;

        // �� ������ �ش��ϴ� ���� ������ UI�� ã���ϴ�.
        BuffIconUI buffIconUI = _CurrentBuffIconList.Find((buffIconUI) => buffIconUI.CheckIconUIByBuffCode(updatedBuff.buffCode));

        // ���� ������ �����Ѵٸ�
        if(updatedBuff.notMuchLeft)
        {
            // ������ ���� ����
            //buffIconUI.StartBlink();
        }
    }

    /// <summary>
    /// ������ ������ �� ȣ��� �޼����Դϴ�.
    /// BuffSystem �� onBuffFinished �̺�Ʈ�� ���� 
    /// </summary>
    /// <param name="finishedBuff"> ���� ����</param>
    private void OnBuffFinished(Buff finishedBuff)
    {
        // �� ������ UI�� ǥ�õǴ� ������ �ƴ϶�� �Լ� ȣ�� ����
        if (!finishedBuff.visibility) return;

        // �� ������ �ش��ϴ� ���� ������ UI�� ã���ϴ�.
        BuffIconUI buffIconUI = _CurrentBuffIconList.Find((buffIconUI) => buffIconUI.CheckIconUIByBuffCode(finishedBuff.buffCode));

        // �� ������ UI�� �����մϴ�.
        RemoveBuffIconUI(buffIconUI);
    }    

    /// <summary>
    /// ���� ����Ʈ�� �ִ� �������� ������ UI ����Ʈ�� �����ϴ� �޼����Դϴ�.
    /// </summary>
    private void InitList()
    {  
        // ���� ����Ʈ�� �ִ� �������� ���ʴ�� �����ɴϴ�.
        foreach(Buff buff in _TargetBuffSystem.buffList)
        {
            // ���� ������ ����
            OnBuffStarted(buff);
        }
    }

    /// <summary>
    /// ���� ǥ�õǰ� �ִ� ���� �����ܵ��� ��� ����� �޼����Դϴ�.
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
    /// ǥ���� ���� �ý��ۿ� �̺�Ʈ�� ����ϴ� �޼����Դϴ�.
    /// </summary>
    private void BindEvents()
    {
        _TargetBuffSystem.onBuffStarted += OnBuffStarted;
        _TargetBuffSystem.onBuffRenew += OnBuffRenew;
        _TargetBuffSystem.onBuffUpdated += OnBuffUpdated;
        _TargetBuffSystem.onBuffFinished += OnBuffFinished;
    }

    /// <summary>
    /// ǥ���� ���� �ý��ۿ� �̺�Ʈ�� ��� ���� �޼����Դϴ�.
    /// </summary>
    private void UnbindEvents()
    {
        _TargetBuffSystem.onBuffStarted -= OnBuffStarted;
        _TargetBuffSystem.onBuffRenew -= OnBuffRenew;
        _TargetBuffSystem.onBuffUpdated -= OnBuffUpdated;
        _TargetBuffSystem.onBuffFinished -= OnBuffFinished;
    }


    /// <summary>
    /// ǥ���� ���� �ý����� �缳���մϴ�.
    /// </summary>
    /// <param name="newTarget"> ���ο� ���� �ý���</param>
    public void SetTargetBuffSystem(BuffSystem newTarget)
    {
        // ���� ǥ���ϰ� �ִ� ���� �ý����� �ִٸ�
        if(_TargetBuffSystem != null)
        {
            // �̺�Ʈ ���ε� ����
            UnbindEvents();
        }       

        // UI�� ��� ����ϴ�.
        ClearUIList();

        // ���� ǥ���� ���� �ý����� �����մϴ�.
        _TargetBuffSystem = newTarget;

        // �̺�Ʈ ���ε�
        BindEvents();

        // ���� �����ܵ��� �ٽ� �����մϴ�.
        InitList();
    }
}
