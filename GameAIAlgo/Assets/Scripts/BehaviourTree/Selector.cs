using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Selector node returns true if one of its children is a success.
public class Selector : Node
{

    //Since sequence is a composite node , it will hold a list that contain all the nodes under it . Will use this list to cycle through all the nodes and evaluate its state.
    protected List<Node> nodes= new List<Node>();



    public Selector(List<Node> nodes)
    {
        this.nodes = nodes;
    }


    public override NodeState Evaluate()
    {
         
        foreach(var node in nodes)
        {
            switch(node.Evaluate())
            {
                case NodeState.RUNNING:
                    _nodeState = NodeState.RUNNING;
                    return _nodeState; 
 
                case NodeState.SUCCESS:
                    _nodeState = NodeState.SUCCESS;
                    return _nodeState;
                     
                case NodeState.FAILURE:
                    break;               
                    
                default:
                    break;
            }
        }

        //if we complete the loop , it means none of the children were a success . so we will return failure.
        _nodeState = NodeState.FAILURE;
 
        return _nodeState;
    }
}
