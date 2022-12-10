using System.Collections.Generic;
using BehaviourTree;

public class AITree : BTree
{

    protected override BTNode SetUpTree()
    {
        BTNode root = new NodeSelector(new List<BTNode>(

            //new NodeSequence(new List<BTNode>
            //(
                //new Check
                //new Task
            //)


            ));

        return root;
    }
}
