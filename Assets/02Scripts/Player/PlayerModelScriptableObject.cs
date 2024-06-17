using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ���� ����� �÷��̾��� ���� ������ �� �ִ� ��ũ���ͺ� ������Ʈ Ŭ�����Դϴ�.
/// </summary>
[CreateAssetMenu(fileName = "PlayerModelData", menuName = "ScriptableObject/Player/PlayerModelScriptableObject")]
public class PlayerModelScriptableObject : ScriptableObject
{
    /// <summary>
    /// PlayerModelInfo ��ü�� ��Ƶ� ����Ʈ
    /// </summary>
    public List<PlayerModelInfo> m_Models = new List<PlayerModelInfo>();

    /// <summary>
    /// ���޵� ������ ���� �׿� �´� �� �������� ��ȯ�ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="level"> ����</param>
    /// <returns> �Ű������� ���޹��� ������ �´� �� �������� ��ȯ�մϴ�.</returns>
    public GameObject FindModelByLevel(int level)
    {
        return m_Models.Find((PlayerModelInfo modelInfo) => modelInfo.m_Level == level).m_ModelPrefab;
    }
}

/// <summary>
/// ������ ���� � ���� ������� ��Ÿ���� Ŭ�����Դϴ�.
/// </summary>
[Serializable]
public class PlayerModelInfo
{
    /// <summary>
    /// �÷��̾��� ����
    /// </summary>
    public int m_Level;

    /// <summary>
    /// ����� �� ������
    /// </summary>
    public GameObject m_ModelPrefab;
}