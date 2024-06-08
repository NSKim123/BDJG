using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlimeModelType
{
    Type1 = 1,
    Type2,
}

public class PlayerModel : MonoBehaviour
{
    private PlayerModelScriptableObject _ModelScriptableObject;

    private SlimeModelType _ModelType = SlimeModelType.Type1;

    private GameObject _CurrentModel;

    public GameObject currentModel => _CurrentModel;

    private void Awake()
    {
        _ModelScriptableObject = Resources.Load<PlayerModelScriptableObject>("ScriptableObject/PlayerModelData/PlayerModelData_Type" + ((int)_ModelType).ToString());        
    }

    private void ChangeModel(GameObject newModel)
    {
        if(_CurrentModel != null) 
            Destroy(_CurrentModel);

        GameObject newCharacter = Instantiate(newModel);
        newCharacter.transform.forward = transform.forward;
        newCharacter.transform.parent = transform;
        newCharacter.transform.localPosition = Vector3.down * 0.421f;
        _CurrentModel = newCharacter;
    }

    public void OnLevelUp(int level)
    {
        GameObject newModel = _ModelScriptableObject.FindModelByLevel(level);

        ChangeModel(newModel);

        transform.localScale = Vector3.one * ((level - 1) * 0.5f + 1);
        _CurrentModel.transform.localScale = Vector3.one;
    }
}
