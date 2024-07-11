using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneInstance : SceneInstanceBase
{
    [Header("# 게임 팁 스크립터블 오브젝트")]
    public GameTipScriptableObject m_GameTipScriptableObject;

    [Header("# 게임 팁 아이콘")]
    public Image m_Image_GameTip;

    [Header("# 게임 팁 설명")]
    public TMP_Text m_TMP_GameTip;

    [Header("# 로딩 바 이미지")]
    public Image m_Image_ProgressBar;

    /// <summary>
    /// 실제 로드 상태가 아닌. 표시용 수치를 나타냅니다.
    /// </summary>
    private float _TargetProgressValue;

    protected override void Awake()
    {
        base.Awake();

        Time.timeScale = 1.0f;
    }

    private void Start()
    {
        StartCoroutine(StartLoadingNextScene());
    }

    private void Update()
    {
        UpdateProgressFill(_TargetProgressValue);
    }

    private IEnumerator StartLoadingNextScene()
    {
        GameTipInfo gameTipInfo = m_GameTipScriptableObject.GetRandomGameTipInfo();

        m_Image_GameTip.sprite = gameTipInfo.m_Icon;
        m_TMP_GameTip.text = gameTipInfo.m_Description;


        m_Image_ProgressBar.fillAmount = 0.0f;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(GameData.Instance.m_NextSceneName);
        asyncOperation.allowSceneActivation = false;

        while (asyncOperation.progress < 0.9f)
        {
            _TargetProgressValue = asyncOperation.progress;

            yield return null;
        }

        _TargetProgressValue = 1.0f;

        yield return new WaitUntil(() => m_Image_ProgressBar.fillAmount > 0.999f);

        yield return new WaitForSeconds(1.0f);

        asyncOperation.allowSceneActivation = true;
    }

    private void UpdateProgressFill(float fill)
    {
        float current = m_Image_ProgressBar.fillAmount;
        current = Mathf.MoveTowards(current, fill, 0.3f * Time.deltaTime);

        m_Image_ProgressBar.fillAmount = current;
    }

}
