using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 알고리즘 실행하고, 게임 씬에 존재하는 객체들을 연결하는 오브젝트의 컴포넌트의 부모 클래스입니다..
/// </summary>
public class SceneInstanceBase : MonoBehaviour
{
    [Header("# 사용할 플레이어 컨트롤러 프리팹")]
    public PlayerControllerBase m_PlayerControllerPrefab;

    /// <summary>
    /// 현재 존재하는 플레이어 컨트롤러 객체에 대한 프로퍼티입니다.
    /// </summary>
    public PlayerControllerBase playerController { get; protected set; }

    protected virtual void Awake()
    {
        // 프리팹이 등록되지 않았다면 호출 종료합니다.
        if (m_PlayerControllerPrefab == null) return;

        // 플레이어 컨트롤러를 생성하고 컨트롤할 플레이어 캐릭터를 설정합니다.
        InitPlayerController();
    }

    /// <summary>
    /// 플레이어 컨트롤러를 생성하고 컨트롤할 플레이어 캐릭터를 설정하는 메서드입니다.
    /// </summary>
    protected virtual void InitPlayerController()
    {
        // 컨트롤러를 생성합니다.
        playerController = Instantiate(m_PlayerControllerPrefab);

        // 플레이어 캐릭터를 찾습니다.
        PlayerCharacterBase playerCharacterBase = FindObjectOfType<PlayerCharacterBase>();

        // 플레이어 컨트롤러가 조종하는 캐릭터를 설정합니다.
        playerController.StartControlCharacter(playerCharacterBase);
    }
}
