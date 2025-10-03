using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private IState curState;

    public void ChangeState(IState nextState)
    {
        Debug.Log(curState + " ����");
        curState?.OnExit(); // ���� State ����
        curState = nextState;
        Debug.Log(nextState + " ����");
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
