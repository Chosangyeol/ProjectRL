using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private IState curState;

    public void ChangeState(IState nextState)
    {
        curState?.OnExit(); // 기존 State 종료
        curState = nextState;
        curState?.OnEnter(); // 다음 State 시작
    }

    public void Tick()
    {
        curState?.Tick();
    }

    public void FixedTick()
    {
        curState?.FixedTick();
    }
}
