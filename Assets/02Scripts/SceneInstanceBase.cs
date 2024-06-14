using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInstanceBase : MonoBehaviour
{
    [Header("# �÷��̾� ��Ʈ�ѷ� ������")]
    public PlayerControllerBase m_PlayerControllerPrefab;

    public PlayerControllerBase playerController { get; protected set; }

    protected virtual void Awake()
    {
        if (m_PlayerControllerPrefab == null) return;

        InitPlayerController();
    }

    protected virtual void InitPlayerController()
    {
        playerController = Instantiate(m_PlayerControllerPrefab);

        // �÷��̾� ĳ���͸� ã���ϴ�.
        PlayerCharacterBase playerCharacterBase = FindObjectOfType<PlayerCharacterBase>();

        // �÷��̾� ��Ʈ�ѷ��� �����ϴ� ĳ���͸� �����մϴ�.
        playerController.StartControlCharacter(playerCharacterBase);
    }
}
