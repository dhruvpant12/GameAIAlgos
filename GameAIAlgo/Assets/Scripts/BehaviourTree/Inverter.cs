using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Inverter node flips the nodestate of a node . Makes Success into failure and vice versa.
public class Inverter : Node
{

    //Since sequence is a composite node , it will hold a list that contain all the nodes under it . Will use this list to cycle through all the nodes and evaluate its state.
    protected Node node;



    public Inverter(Node node)
    {
        this.node = node;
    }


    public override NodeState Evaluate()
    {
        
            switch(node.Evaluate())
            {
                case NodeState.RUNNING:
                _nodeState = NodeState.RUNNING;
                    break;

                case NodeState.SUCCESS:   
                _nodeState = NodeState.FAILURE;
                break;

                case NodeState.FAILURE:
                    _nodeState = NodeState.SUCCESS;  
                    return _nodeState;
                    
                default:
                    break;
            }

        return _nodeState;
       
    }
}
