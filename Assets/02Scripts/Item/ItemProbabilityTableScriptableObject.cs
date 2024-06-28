using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Giant,
    AreaDeployment,
}

[CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "ScriptableObject/Item/ItemProbabilityTableScriptableObject")]
public class ItemProbabilityTableScriptableObject : ProbabilityTableScriptableObject<ItemType>
{
    
}
