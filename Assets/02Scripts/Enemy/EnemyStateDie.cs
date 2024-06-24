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
                    enemyAgent.enabled = false;
                    rigid.useGravity = false;
                    animator.Play("die");
                    _currentStep++;
                }
                break;
            case StepInState.Playing:
                {
                    Debug.Log("die");
                    
                    if (fadeCoroutine == null && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
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
        float f = 1;
        while (f > 0)
        {
            f -= 0.3f;
            enemyCharacter.dieRenderer.material = enemyCharacter.changeMat;
            Color ColorAlhpa = enemyCharacter.dieRenderer.material.color;
            ColorAlhpa.a = f;
            enemyCharacter.dieRenderer.material.color = ColorAlhpa;
            yield return new WaitForSeconds(0.1f);
        }
        GameObject.Destroy(enemyCharacter.gameObject);
    }

}
