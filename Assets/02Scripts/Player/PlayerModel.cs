using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ Ÿ���� ��Ÿ���� ���������Դϴ�.
/// </summary>
public enum SlimeModelType
{
    Type1 = 1,  // �⺻ ������
    Type2,
}

/// <summary>
/// ĳ������ �� �� �������� �����ϴ� ������Ʈ�Դϴ�.
/// </summary>
/// �� ���� �κ�
public partial class PlayerModel : MonoBehaviour
{
    [Header("# źȯ ������")]
    public Bullet m_Bullet;

    [Header("# ���� �� ����Ʈ")]
    public List<LevelUpEffectInfo> m_LevelUpEffects;

    [Header("# ����� ȿ�� �� ����Ʈ")]
    public GameObject m_Effect_MachineGun;

    /// <summary>
    /// �ε��� PlayerModelScriptableObject ��ü
    /// </summary>
    private PlayerModelScriptableObject _ModelScriptableObject;

    /// <summary>
    /// ������ �� Ÿ��
    /// </summary>
    private SlimeModelType _ModelType = SlimeModelType.Type1;

    /// <summary>
    /// ���� ��
    /// </summary>
    private GameObject _CurrentModel;

    private GameObject _InstantiatedEffect;

    /// <summary>
    /// ���� �𵨿� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public GameObject currentModel => _CurrentModel;

    /// <summary>
    /// ���� ����� �� ȣ��Ǵ� �븮���Դϴ�.
    /// </summary>
    public event System.Action onModelChanged;

    private void Awake()
    {
        // �� ��ũ���ͺ� ������Ʈ�� Ÿ�Կ� �°� �ε��մϴ�.
        _ModelScriptableObject = Resources.Load<PlayerModelScriptableObject>("ScriptableObject/PlayerModelData/PlayerModelData_Type" + ((int)_ModelType).ToString());        
    }

    private void FixedUpdate()
    {
        // ũ�⸦ �����մϴ�.
        UpdateScale();
    }

    /// <summary>
    /// ���ο� �𵨷� ��ü�մϴ�.
    /// </summary>
    /// <param name="newModel"> ���ο� �� ������Ʈ</param>
    private void ChangeModel(GameObject newModel)
    {
        // ���� ���� �����մϴ�.
        if(_CurrentModel != null) 
            Destroy(_CurrentModel);

        // ���ο� ���� �����ϰ� transform �� �����մϴ�.
        GameObject newCharacter = Instantiate(newModel);
        newCharacter.transform.forward = transform.forward;
        newCharacter.transform.parent = transform;
        newCharacter.transform.localPosition = Vector3.down * 0.415f;
        newCharacter.transform.localScale = Vector3.one;

        // ���� ���� �����մϴ�.
        _CurrentModel = newCharacter;

        // �� ���� �̺�Ʈ�� ȣ���մϴ�.
        onModelChanged?.Invoke();

        // źȯ�� ���͸��� �ٲߴϴ�.
        //m_Bullet.GetComponentInChildren<Renderer>().material = _CurrentModel.GetComponentInChildren<Renderer>().material;
        ObjectPoolManager.Instance.ChangeBulletMaterial(_CurrentModel.GetComponentInChildren<Renderer>().material);
    }

    private void InstantiateLevelUpEffectByLevel(int level)
    {
        LevelUpEffectInfo effectInfo = m_LevelUpEffects.Find((effectInfo) => effectInfo.m_Level == level);

        if (effectInfo == null || effectInfo.m_Effect == null) return;

        GameObject effect = Instantiate(effectInfo.m_Effect);
        effect.transform.position = _CurrentModel.transform.position;
        effect.transform.SetParent(transform);
    }

    public void OnStartGiant()
    {
        // ���ο� ���� �ҷ��ɴϴ�.
        GameObject newModel = _ModelScriptableObject.FindModelByLevel(4);

        // ���� ��ü�մϴ�.
        ChangeModel(newModel);
    }

    public void OnStartMachineGun()
    {
        _InstantiatedEffect = Instantiate(m_Effect_MachineGun);

        _InstantiatedEffect.transform.position = transform.position;
        //_InstantiatedEffect.transform.SetParent(transform);
    }

    public void OnFinishMachineGun()
    {
        ParticleSystem.MainModule newMain;
        foreach(ParticleSystem particleSystem in _InstantiatedEffect.GetComponentsInChildren<ParticleSystem>())
        {
            newMain = particleSystem.main;
            newMain.loop = false;
        }
    }

    /// <summary>
    /// ���� �� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    /// <param name="level"> �޼� ����</param>
    public void OnLevelUp(int level)
    {
        // ���ο� ���� �ҷ��ɴϴ�.
        GameObject newModel = _ModelScriptableObject.FindModelByLevel(level);

        // ���� ��ü�մϴ�.
        ChangeModel(newModel);

        InstantiateLevelUpEffectByLevel(level);
    }
}







/// <summary>
/// ������ ���� �κ�
/// </summary>
public partial class PlayerModel
{
    [Header("# ������ ����")]
    [Header("�ּ� ũ��")]
    public float m_MinScale = 1.0f;

    [Header("ũ�� ����(���� źȯ ���� ���� ũ�� ������)")]
    public float m_ScaleCoefficient = 0.1f;

    /// <summary>
    /// ��ǥ �������Դϴ�.
    /// </summary>
    private float _TargetScale;

    /// <summary>
    /// ũ�⸦ �����ϴ� �޼����Դϴ�.
    /// </summary>
    private void UpdateScale()
    {
        // ��ǥ ũ��� �ε巴�� ��ȭ��ŵ�ϴ�.
        Vector3 newScale = Vector3.MoveTowards(transform.localScale, _TargetScale * Vector3.one, 50.0f * Time.fixedDeltaTime);
        transform.localScale = newScale;
    }

    /// <summary>
    /// ���� źȯ ���� ���� ũ�⸦ ����ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="bullets"> ���� źȯ ��</param>
    /// <returns> ũ�⸦ ��ȯ�մϴ�.</returns>
    private float CalculateScaleByBullets(int bullets)
    {
        return m_MinScale + 0.1f * bullets;
    }

    /// <summary>
    /// ���� źȯ ���� ���� ��ǥ ũ�⸦ �����ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="bullets"> ���� źȯ ��</param>
    public void UpdateTargetScale(int bullets)
    {
        _TargetScale = CalculateScaleByBullets(bullets);
    }
}

[Serializable]
public class LevelUpEffectInfo
{
    public int m_Level;
    public GameObject m_Effect;
}