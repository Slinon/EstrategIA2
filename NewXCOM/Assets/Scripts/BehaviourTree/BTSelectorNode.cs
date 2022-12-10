using System.Collections;
using System.Collections.Generic;

namespace BehaviourTree
{
    //Selector es una puerta OR. Si todos alg�n hijo devuelve true al ser evaluado, ejecutar� la acci�n.
    public class NodeSelector : BTNode
    {
       public NodeSelector() : base () { }
       public NodeSelector(List<BTNode> children) : base (children) { }

        public override NodeState Evaluate()
        {
            foreach (BTNode node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;

                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;

                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }

            state = NodeState.FAILURE;
            return state;
        }

    }
}