using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerBase : MonoBehaviour
{
    public PlayerCharacterBase controlledCharacter { get; private set; }

    /// <summary>
    /// 조종을 시작할 캐릭터를 설정합니다.
    /// </summary>
    /// <param name="controlCharacter"></param>
    public virtual void StartControlCharacter(PlayerCharacterBase controlCharacter)
    {
        // 이미 전달된 캐릭터를 조종중이라면 함수 호출 종료
        if (controlledCharacter == controlCharacter) return;

        controlledCharacter = controlCharacter;

        // 캐릭터 조종을 시작합니다.
        controlledCharacter.OnControlStarted(this);
    }

    /// <summary>
    /// 조종을 끝냅니다.
    /// </summary>
    public virtual void FinishControlCharacter()
    {
        // 만약 조종중인 캐릭터가 존재하지 않는다면 함수 호출 종료
        if (controlledCharacter == null) return;

        controlledCharacter.OnControlFinished();
        controlledCharacter = null;
    }

}
