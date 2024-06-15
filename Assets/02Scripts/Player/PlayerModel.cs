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

    /// <summary>
    /// ���� �𵨿� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public GameObject currentModel => _CurrentModel;

    /// <summary>
    /// ���� ����� �� ȣ��Ǵ� �븮���Դϴ�.
    /// </summary>
    public event System.Action OnModelChanged;

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
        newCharacter.transform.localPosition = Vector3.down * 0.365f;
        newCharacter.transform.localScale = Vector3.one;

        // ���� ���� �����մϴ�.
        _CurrentModel = newCharacter;

        // �� ���� �̺�Ʈ�� ȣ���մϴ�.
        OnModelChanged?.Invoke();
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
        Vector3 newScale = Vector3.MoveTowards(transform.localScale, _TargetScale * Vector3.one, 1.0f * Time.fixedDeltaTime);
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
