using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateDie : EnemyStateBase
{
    private Coroutine fadeCoroutine;

    public EnemyStateDie(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override bool canExecute() => true;

    public override State MoveNextStep()
    {
        State nextState = State.Die;

        switch (_currentStep)
        {
            case StepInState.None:
                {
                    _currentStep++;
                }
                break;
            case StepInState.Start:
                {
                    _currentStep++;
                }
                break;
            case StepInState.Playing:
                {
                    Debug.Log("die");
                    //애니메이션 멈추기
                    if (fadeCoroutine == null)
                    {
                        _currentStep++;
                    }
                }
                break;
            case StepInState.End:
                {
                    if (fadeCoroutine == null)
                    {
                        fadeCoroutine = enemyCharacter.StartCoroutine(FadeOut());
                    }
                }
                break;
            default:
                break;
        }

        return nextState;
    }

    IEnumerator FadeOut()
    {
        Debug.Log("페이드");
        float f = 1;
        while (f > 0)
        {
            f -= 0.1f;
            enemyCharacter.dieRenderer.material = enemyCharacter.changeMat;
            Color ColorAlhpa = enemyCharacter.dieRenderer.material.color;
            ColorAlhpa.a = f;
            enemyCharacter.dieRenderer.material.color = ColorAlhpa;
            yield return new WaitForSeconds(0.1f);
        }
        GameObject.Destroy(enemyCharacter.gameObject);
    }

}
