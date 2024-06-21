using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemIconScriptableObject", menuName = "ScriptableObject/Item/ItemScriptableObject")]
public class ItemIconScriptableObject : ScriptableObject
{
    public List<ItemIconInfo> m_ItemIconInfos = new();

    public Sprite GetItemIconByBuffCode(int itemCode)
    {   
        return m_ItemIconInfos.Find((itemIconInfo) => itemIconInfo.m_ItemCode == itemCode).m_ItemIcon;
    }
}

[Serializable]
public class ItemIconInfo
{
    public int m_ItemCode;

    public Sprite m_ItemIcon;
}