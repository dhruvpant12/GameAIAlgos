using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToCoverNode : Node
{
     
    private NavMeshAgent agent; // NavMesh is added to enemy to make it move on the map.
    private EnemyAI ai;

    public GoToCoverNode(  NavMeshAgent agent, EnemyAI ai)
    {
         
        this.agent = agent;
        this.ai = ai;
    }

     

    public override NodeState Evaluate()
    {
        Transform coverSpot = ai.GetBestCover();
        if (coverSpot == null)
            return NodeState.FAILURE;
        ai.SetColor(Color.blue);
        float distance = Vector3.Distance(coverSpot.position, agent.transform.position);


        if(distance> 0.2f)
        {
            agent.isStopped = false;
            agent.SetDestination(coverSpot.position);
           return _nodeState = NodeState.RUNNING;
        }
        else
        {
            agent.isStopped = true;
            return _nodeState = NodeState.SUCCESS;
        }
    }
}
