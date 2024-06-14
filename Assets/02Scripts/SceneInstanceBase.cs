using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInstanceBase : MonoBehaviour
{
    [Header("# 플레이어 컨트롤러 프리팹")]
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

        // 플레이어 캐릭터를 찾습니다.
        PlayerCharacterBase playerCharacterBase = FindObjectOfType<PlayerCharacterBase>();

        // 플레이어 컨트롤러가 조종하는 캐릭터를 설정합니다.
        playerController.StartControlCharacter(playerCharacterBase);
    }
}
