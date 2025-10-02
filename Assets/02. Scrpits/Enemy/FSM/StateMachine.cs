using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private IState curState;

    public void ChangeState(IState nextState)
    {
        curState?.OnExit(); // ���� State ����
        curState = nextState;
        curState?.OnEnter(); // ���� State ����
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
