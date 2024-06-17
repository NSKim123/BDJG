using UnityEngine;

// �������� �����ۿ� ������Ʈ�� ���� Ŭ����
public class Item_TimeStop : ItemOfPlayer
{
    [SerializeField] private int _buffCode;

    // ������ ������ Ŭ�������� giant�� �ο��� �����ڵ带 �������ݴϴ�.
    protected override void Start()
    {
        base.Start();
        _buffCode = ItemDataRepository.buffInfoOfItem[ItemName.timeStop];
    }

    public override int BuffCode { get => _buffCode; set => _buffCode = value; }
}
