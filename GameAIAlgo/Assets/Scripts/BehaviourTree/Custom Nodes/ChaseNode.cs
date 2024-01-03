using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseNode : Node
{
    private Transform target;
    private NavMeshAgent agent; // NavMesh is added to enemy to make it move on the map.
    private EnemyAI ai;

    public ChaseNode(Transform target, NavMeshAgent agent, EnemyAI ai)
    {
        this.target = target;
        this.agent = agent;
        this.ai = ai;
    }

     

    public override NodeState Evaluate()
    {
        ai.SetColor(Color.yellow);
        float distance = Vector3.Distance(target.position, agent.transform.position);


        if(distance> 0.2f)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
           return _nodeState = NodeState.RUNNING;
        }
        else
        {
            agent.isStopped = true;
            return _nodeState = NodeState.SUCCESS;
        }
    }
}
