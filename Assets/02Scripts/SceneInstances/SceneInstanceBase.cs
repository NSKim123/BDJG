using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �˰��� �����ϰ�, ���� ���� �����ϴ� ��ü���� �����ϴ� ������Ʈ�� ������Ʈ�� �θ� Ŭ�����Դϴ�..
/// </summary>
public class SceneInstanceBase : MonoBehaviour
{
    [Header("# ����� �÷��̾� ��Ʈ�ѷ� ������")]
    public PlayerControllerBase m_PlayerControllerPrefab;

    /// <summary>
    /// ���� �����ϴ� �÷��̾� ��Ʈ�ѷ� ��ü�� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public PlayerControllerBase playerController { get; protected set; }

    protected virtual void Awake()
    {
        // �������� ��ϵ��� �ʾҴٸ� ȣ�� �����մϴ�.
        if (m_PlayerControllerPrefab == null) return;

        // �÷��̾� ��Ʈ�ѷ��� �����ϰ� ��Ʈ���� �÷��̾� ĳ���͸� �����մϴ�.
        InitPlayerController();
    }

    /// <summary>
    /// �÷��̾� ��Ʈ�ѷ��� �����ϰ� ��Ʈ���� �÷��̾� ĳ���͸� �����ϴ� �޼����Դϴ�.
    /// </summary>
    protected virtual void InitPlayerController()
    {
        // ��Ʈ�ѷ��� �����մϴ�.
        playerController = Instantiate(m_PlayerControllerPrefab);

        // �÷��̾� ĳ���͸� ã���ϴ�.
        PlayerCharacterBase playerCharacterBase = FindObjectOfType<PlayerCharacterBase>();

        // �÷��̾� ��Ʈ�ѷ��� �����ϴ� ĳ���͸� �����մϴ�.
        playerController.StartControlCharacter(playerCharacterBase);
    }
}
