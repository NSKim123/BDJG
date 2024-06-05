using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 게이지를 나타내는 클래스입니다.
/// </summary>
public class Gauge
{
    /// <summary>
    /// 현재 값을 나타냅니다.
    /// </summary>
    private float _CurrentValue;
    
    /// <summary>
    /// 이 게이지의 최대값을 나타내는 자동 구현 프로퍼티입니다.
    /// </summary>
    public float max { get; set; }

    /// <summary>
    /// 이 게이지의 최소값을 나타내는 자동 구현 프로퍼티입니다.
    /// </summary>
    public float min { get; set; }

    /// <summary>
    /// 현재 값에 대한 프로퍼티입니다.
    /// </summary>
    public float currentValue
    {
        get { return _CurrentValue; }
        set
        {
            if(value < min)
                value = min;
            else if(value > max) 
                value = max;

            _CurrentValue = value;
        }
    }

    /// <summary>
    /// 게이지가 얼마나 차있는지에 대한 비율을 나타내는 읽기 전용 프로퍼티입니다.
    /// </summary>
    public float ratio
    {
        get
        {
            if (max <= min)
                return 0.0f;
            else
                return _CurrentValue / (max - min);
        }
    }

    /// <summary>
    /// 생성자입니다.
    /// </summary>
    /// <param name="max"> 설정할 최대값</param>
    /// <param name="current"> 설정할 현재 값</param>
    /// <param name="min"> 설정할 최소값</param>
    public Gauge(float max, float current = 0.0f, float min = 0.0f)
    {
        this.max = max;
        this.min = min;
        this.currentValue = current;
    }
}
