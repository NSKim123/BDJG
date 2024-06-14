using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerBase : MonoBehaviour
{
    public PlayerCharacterBase controlledCharacter { get; private set; }

    /// <summary>
    /// ������ ������ ĳ���͸� �����մϴ�.
    /// </summary>
    /// <param name="controlCharacter"></param>
    public virtual void StartControlCharacter(PlayerCharacterBase controlCharacter)
    {
        // �̹� ���޵� ĳ���͸� �������̶�� �Լ� ȣ�� ����
        if (controlledCharacter == controlCharacter) return;

        controlledCharacter = controlCharacter;

        // ĳ���� ������ �����մϴ�.
        controlledCharacter.OnControlStarted(this);
    }

    /// <summary>
    /// ������ �����ϴ�.
    /// </summary>
    public virtual void FinishControlCharacter()
    {
        // ���� �������� ĳ���Ͱ� �������� �ʴ´ٸ� �Լ� ȣ�� ����
        if (controlledCharacter == null) return;

        controlledCharacter.OnControlFinished();
        controlledCharacter = null;
    }

}
