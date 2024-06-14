using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterBase : MonoBehaviour
{
    /// <summary>
	/// ��Ʈ�ѷ� ��ü�� ��Ÿ���ϴ�.
	/// </summary>
	public PlayerControllerBase playerController { get; private set; }

    /// <summary>
    /// �� ĳ������ ������ ���۵� �� ȣ��˴ϴ�.
    /// </summary>
    /// <param name="playerController">������ �����ϴ� �÷��̾� ��Ʈ�ѷ� ��ü�� ���޵˴ϴ�.</param>
    public virtual void OnControlStarted(PlayerControllerBase playerController)
        => this.playerController = playerController;

    /// <summary>
    /// �� ĳ������ ������ ������ �� ȣ��˴ϴ�.
    /// </summary>
    public virtual void OnControlFinished()
        => playerController = null;

}
