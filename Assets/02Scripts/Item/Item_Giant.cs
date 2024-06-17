using UnityEngine;

// 거대화 아이템에 컴포넌트 붙을 클래스
public class Item_Giant : ItemOfPlayer
{
    [SerializeField] private int _buffCode;

    // 아이템 데이터 클래스에서 giant에 부여한 버프코드를 설정해줍니다.
    protected override void Start()
    {
        base.Start();
        _buffCode = ItemDataRepository.buffInfoOfItem[ItemName.giant];
    }

    public override int BuffCode { get => _buffCode; set => _buffCode = value; }
}