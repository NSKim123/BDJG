using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterBase : MonoBehaviour
{
    /// <summary>
	/// 컨트롤러 객체를 나타냅니다.
	/// </summary>
	public PlayerControllerBase playerController { get; private set; }

    /// <summary>
    /// 이 캐릭터의 조종이 시작될 때 호출됩니다.
    /// </summary>
    /// <param name="playerController">조종을 시작하는 플레이어 컨트롤러 객체가 전달됩니다.</param>
    public virtual void OnControlStarted(PlayerControllerBase playerController)
        => this.playerController = playerController;

    /// <summary>
    /// 이 캐릭터의 조종이 끝났을 때 호출됩니다.
    /// </summary>
    public virtual void OnControlFinished()
        => playerController = null;

}
