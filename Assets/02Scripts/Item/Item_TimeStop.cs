using UnityEngine;

// 영역전개 아이템에 컴포넌트로 붙을 클래스
public class Item_TimeStop : ItemOfPlayer
{
    [SerializeField] private int _buffCode;

    // 아이템 데이터 클래스에서 timestop에 부여한 버프코드를 설정해줍니다.
    // 주석대로 호출하시면 됩니다.
    protected override void Start()
    {
        base.Start();
        //_buffCode = ItemDataRepository.buffInfoOfItem[ItemName.timeStop];
    }

    public override int BuffCode { get => _buffCode; set => _buffCode = value; }
}
