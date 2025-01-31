using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateDie : EnemyStateBase
{
    private Coroutine fadeCoroutine;

    public EnemyStateDie(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override bool CanExecute() => true;

    public override EState MoveNextStep()
    {
        EState nextState = EState.Die;

        switch (_currentStep)
        {
            case EStepInState.None:
                {
                    _currentStep++;
                }
                break;
            case EStepInState.Start:
                {
                    enemyAgent.enabled = false;
                    rigid.useGravity = false;
                    animator.Play("die");
                    _currentStep++;
                }
                break;
            case EStepInState.Playing:
                {
                    if (fadeCoroutine == null && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
                    {
                        _currentStep++;
                    }
                }
                break;
            case EStepInState.End:
                {
                    fadeCoroutine = enemyCharacter.StartCoroutine(FadeOut());
                }
                break;
            default:
                break;
        }

        return nextState;
    }

    private IEnumerator FadeOut()
    {
        float f = 1;
        while (f > 0)
        {
            f -= 0.3f;
            enemyCharacter.dieRenderer.material = enemyCharacter.changeMat;
            Color colorAlhpa = enemyCharacter.dieRenderer.material.color;
            colorAlhpa.a = f;
            enemyCharacter.dieRenderer.material.color = colorAlhpa;
            yield return new WaitForSeconds(0.1f);
        }
        
        ObjectPoolManager.Instance.ReturnToPool(enemyCharacter.gameObject);
    }

}
