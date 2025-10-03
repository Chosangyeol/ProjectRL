using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private IState curState;

    public void ChangeState(IState nextState)
    {
        Debug.Log(curState + " 종료");
        curState?.OnExit(); // 기존 State 종료
        curState = nextState;
        Debug.Log(nextState + " 시작");
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
