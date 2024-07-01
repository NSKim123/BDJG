using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IState<T>
    where T : System.Enum
{
    public enum StepInState
    {
        None,
        Start,
        Playing,
        End
    }
    StepInState current { get; }
    bool canExecute { get; }
    T MoveNextStep();
    void Reset();
}
