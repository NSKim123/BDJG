using System.Collections.Generic;

// ������ �̸��� ��Ÿ���� �������Դϴ�.
public enum ItemName
{
    giant,
    timeStop,
    gun,
    wind,
}


// ������ �̸��� �´� �����ڵ带 ������ ��ųʸ��� ���� �� �����ۿ� �����ڵ带 �������ַ��� �߽��ϴ�.

// �Ʒ� ����� BuffIcon�� �����ϴ� �����Ϳ� ������ ���̱⵵ �ϰ�, ���Ƿ� 0��° 1��°�� ������ ���̱⵵ �ؼ� ������ �ʿ��ѵ�
// ���⿡ �ٷ� �����ڵ带 �������ְų� ���� �� scriptableObject�� ����ų� ��� �ؾ��� �� �����ϴ�.
// �� �ܿ��� ���� ������ ��ũ��Ʈ�� �����ڵ带 �ϵ��ڵ��ϴ� ����ۿ� ������ �ȳ��� �ϴ� �̷��� �صξ����ϴ�.
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
