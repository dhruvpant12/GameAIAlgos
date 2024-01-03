using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Sequence node returns true only if all its children are true.
public class Sequence : Node
{

    //Since sequence is a composite node , it will hold a list that contain all the nodes under it . Will use this list to cycle through all the nodes and evaluate its state.
    protected List<Node> nodes= new List<Node>();



    public Sequence(List<Node> nodes)
    {
        this.nodes = nodes;
    }


    public override NodeState Evaluate()
    {
        bool isAnyChildRunning = false; //In case any child is running , this will be set to true so the sequence doesnt break and the evaluation will move on to the next node.
        foreach(var node in nodes)
        {
            switch(node.Evaluate())
            {
                case NodeState.RUNNING:
                    isAnyChildRunning = true;
                    break;
                case NodeState.SUCCESS:  // if the node is a success , we donot do anything and move on to the next child.
                    break;
                case NodeState.FAILURE:
                    _nodeState = NodeState.FAILURE; //if any node is a failure , the sequence has failed and it will break us out of the loop.
                    return _nodeState;
                    
                default:
                    break;
            }
        }

        //Once we finish the loop, it means either all children were a success or a node is presently running . so we check for that. 
        _nodeState = isAnyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS; 
        return _nodeState;
    }
}
