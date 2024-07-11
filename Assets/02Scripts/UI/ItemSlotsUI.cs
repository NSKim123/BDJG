using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemSlotsUI : MonoBehaviour
{
    private static ItemIconScriptableObject _ItemIconScriptableObject;

    [Header("# 아이템 사용 버튼")]
    public Button m_Button;

    [Header("# 아이템 이미지 1")]
    public Image m_ItemImage1;

    [Header("# 아이템 이미지 2")]
    public Image m_ItemImage2;

    private bool _Interactable = true;

    private void Awake()
    {
        if (_ItemIconScriptableObject == null)
            _ItemIconScriptableObject = Resources.Load<ItemIconScriptableObject>("ScriptableObject/ItemIcons/ItemIconScriptableObject");

        BindClickEvent(PlayClickSound);
    }

    private void PlayClickSound()
    {
        SoundManager.Instance.PlaySound(_Interactable ? Constants.SOUNDNAME_CLICK_ABLEBUTTON : Constants.SOUNDNAME_CLICK_DISABLEBUTTON, SoundType.Effect);
    }

    public void BindClickEvent(UnityAction addFunction)
    {
        m_Button.onClick.AddListener(addFunction);
    }

    public void OnItemSlotChanged(List<int> itemSlots)
    {
        // 스크립터블 오브젝트에서 아이템 아이콘 찾아서 이미지 설정
        Sprite sprite1 = null;
        Sprite sprite2 = null;

        if (itemSlots.Count == 1 )
        {
            sprite1 = _ItemIconScriptableObject.GetItemIconByBuffCode(itemSlots[0]);
        }
        else if (itemSlots.Count == 2 )
        {
            sprite1 = _ItemIconScriptableObject.GetItemIconByBuffCode(itemSlots[0]);
            sprite2 = _ItemIconScriptableObject.GetItemIconByBuffCode(itemSlots[1]);
        }        
                
        m_ItemImage1.sprite = sprite1;
        m_ItemImage1.color = m_ItemImage1.sprite != null ? Color.white : Color.clear;

        m_ItemImage2.sprite = sprite2;
        m_ItemImage2.color = m_ItemImage2.sprite != null ? Color.white : Color.clear;
    }

    public void OnToggleChanged(bool toggleSwitch)
    {
        _Interactable = toggleSwitch;
    }
}
