using System.Collections.Generic;

// 아이템 이름을 나타내는 열거형입니다.
public enum ItemName
{
    giant,
    timeStop,
    gun,
    wind,
}


// 아이템 이름에 맞는 버프코드를 저장한 딕셔너리를 만들어서 각 아이템에 버프코드를 저장해주려고 했습니다.

// 아래 방식은 BuffIcon을 관리하는 데이터에 접근한 것이기도 하고, 임의로 0번째 1번째에 접근한 것이기도 해서 수정이 필요한데
// 여기에 바로 버프코드를 지정해주거나 따로 또 scriptableObject를 만들거나 등등 해야할 것 같습니다.
// 이 외에는 직접 아이템 스크립트에 버프코드를 하드코딩하는 방법밖엔 생각이 안나서 일단 이렇게 해두었습니다.
public class ItemDataRepository
{
    public Dictionary<ItemName, int> buffInfoOfItem;

    public ItemDataRepository()
    {
        buffInfoOfItem = new Dictionary<ItemName, int>
        {
            //{ItemName.giant, Resources.Load<BuffIconScriptableObject>("ScriptableObject/BuffIcons/BuffIconScriptableObject").m_BuffInfos[0].m_BuffCode},
            //{ItemName.timeStop, Resources.Load<BuffIconScriptableObject>("ScriptableObject/BuffIcons/BuffIconScriptableObject").m_BuffInfos[1].m_BuffCode},
        };
    }
}
