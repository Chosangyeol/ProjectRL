using UnityEngine;


// 모든노드는 Success, Running, Fail 반환

public enum NodeState{ Success, Fail , Running };

public abstract class BNode
{
    public abstract NodeState Evaluate();  // 실행하고 // 상태반환
}


// 시퀀스 노드 

public class SequenceNode : BNode
{
    private BNode[] children;
    public SequenceNode(BNode[] node)
    {
        children =node;
    }
    public override NodeState Evaluate()
    {
        foreach (var child in children)
        {
            if(child.Evaluate() != NodeState.Success)
            {
                return NodeState.Fail;
            }
        }
        return NodeState.Success;
    }
}

public class SelectNode : BNode
{
    private BNode[] children;
    public SelectNode(BNode[] node)
    {
        children = node;
    }
    public override NodeState Evaluate()
    {
        foreach (var child in children)
        {
            if (child.Evaluate() == NodeState.Success)
            {
                return NodeState.Success;
            }
        }
        return NodeState.Fail;
    }
}
