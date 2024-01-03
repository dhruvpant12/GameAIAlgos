using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthNode : Node
{

    private EnemyAI ai; // Get reference to enemy to get its health.
    private float threshold; // threshold to compare with enemy current health.

    public HealthNode(EnemyAI ai, float threshold)
    {
        this.ai = ai;
        this.threshold = threshold;
    }

    public override NodeState Evaluate()
    {
        //This node will never return a running state.
        return ai.currentHealth <= threshold ? NodeState.SUCCESS : NodeState.FAILURE;
     }
}
