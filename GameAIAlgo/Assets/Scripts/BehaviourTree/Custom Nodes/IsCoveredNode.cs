using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCoveredNode : Node
{
    private Transform target;
    private Transform origin;

    public IsCoveredNode(Transform target, Transform origin)
    {
        this.target = target;
        this.origin = origin;
    }

    public override NodeState Evaluate()
    {
        // Check for raycasthit on player.
        RaycastHit hit;
        if(Physics.Raycast(origin.position,target.position-origin.position,out hit))
        {
            //If raycasthit is not player , it means we hit something else and are hidden.
            if(hit.collider.transform != target)
            {
                return NodeState.SUCCESS;
            }
        }

        return NodeState.FAILURE;
    }
}
