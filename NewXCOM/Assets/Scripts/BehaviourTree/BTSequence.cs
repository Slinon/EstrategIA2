using System.Collections;
using System.Collections.Generic;

namespace BehaviourTree
{
    //Sequence es una puerta AND. Si todos sus hijos devuelven true al ser evaluados, ejecutará la acción.
    public class NodeSequence : BTNode
    {
        public NodeSequence() : base() { }
        public NodeSequence(List<BTNode> children) : base(children) { }

        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach (BTNode node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;

                    case NodeState.SUCCESS:
                        continue;

                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;

                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }

            state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }

    }
}
