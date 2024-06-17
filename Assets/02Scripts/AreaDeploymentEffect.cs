using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AreaDeploymentEffect : MonoBehaviour
{
    private float _Time;

    private bool _IsStartDisplayingCharacter;

    public Material m_AreaDeploymentEffectMaterial;

    public List<RectTransform> m_Characters;


    private void Update()
    {
        if(_Time <= 1.5f)
        {
            m_AreaDeploymentEffectMaterial.SetFloat("_Boundary", _Time);
            _Time += Time.deltaTime;
        }
        else if(!_IsStartDisplayingCharacter)
        {
            StartCoroutine(DisplayCharacters());
            _IsStartDisplayingCharacter = true;
        }
    }

   

    private IEnumerator DisplayCharacters()
    {
        foreach(RectTransform character in m_Characters)
        {
            character.gameObject.SetActive(true);
            character.localScale = Vector3.one * 1.5f;
            // 크키 감소 코루틴 시작
            StartCoroutine(AdjustCharacterScale(character));

            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(0.3f);

        Vector2 sliceDirection = (new Vector2(1.0f, 2.0f)).normalized;

        float initSpeed = 5.0f;

        while(initSpeed > 0.1f)
        {
            m_Characters[0].anchoredPosition += sliceDirection * initSpeed;
            m_Characters[1].anchoredPosition += sliceDirection * initSpeed;

            m_Characters[2].anchoredPosition -= sliceDirection * initSpeed;
            m_Characters[3].anchoredPosition -= sliceDirection * initSpeed;

            initSpeed = Mathf.Lerp(initSpeed, 0.0f, 0.05f);

            yield return null;
        }

        Time.timeScale = 1.0f;

        Destroy(gameObject, 2.0f);
    }

    private IEnumerator AdjustCharacterScale(RectTransform character)
    {
        while(true)
        {
            character.localScale -= Vector3.one * Time.deltaTime * 10.0f;

            if(character.localScale.x < 0.5f)
            {
                character.localScale = Vector3.one * 0.5f;
                break;
            }
            else
            {
                yield return null;
            }
        }

        while (true)
        {
            character.localScale += Vector3.one * Time.deltaTime * 10.0f;

            if (character.localScale.x >= 1.0f)
            {
                character.localScale = Vector3.one;
                break;
            }
            else
            {
                yield return null;
            }
        }
    }

    private void OnDestroy()
    {
        m_AreaDeploymentEffectMaterial.SetFloat("_Boundary", 0.0f);
    }
}
