using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCoverAvailableNode : Node
{
    private Cover[] availableCover; // array of all the walls in the game.
    private EnemyAI ai;
    private Transform target;

    public IsCoverAvailableNode(Cover[] availableCover, EnemyAI ai, Transform target)
    {
        this.availableCover = availableCover;
        this.ai = ai;
        this.target = target;
    }

    public override NodeState Evaluate()
    {
        Transform bestSpot = FindBestCoverSpot(); //trying to find the best spot in the game.
        ai.SetBestCover(bestSpot); // recieved best hiding spot from all the avalaible spots on all walls.
        return bestSpot != null ? NodeState.SUCCESS : NodeState.FAILURE;  
    }

    private Transform FindBestCoverSpot()
    {
        float minAngle = 90;
        Transform bestspot = null;

        for(int i =0; i < availableCover.Length; i++) // iterating through all the walls.
        {
            Transform bestSpotInCover = FindBestSpotInCover(availableCover[i], ref minAngle); // attepmt to find which side of the wall to hide at.

            if (bestSpotInCover != null) bestspot = bestSpotInCover;
        }

        return bestspot;
    }

    private Transform FindBestSpotInCover(Cover cover, ref float minAngle)
    {
        Transform[] availableSpots = cover.GetCoverSpots(); // ref to the hiding side placeholders on a wall.
        Transform bestSpot = null;
        

        for(int i=0;i<availableSpots.Length;i++) // lets iterate through a wall with its hiding placeholder.
        {
            Vector3 direction = target.position - availableSpots[i].position; // direction of the enemy target to the hiding placeholder side of the wall.
            if (CheckIfSpotIsValid(availableSpots[i]))
            {
                float angle = Vector3.Angle(availableSpots[i].forward, direction); // angle between the forward or the local z axis of the place holder and the direction where the target is facing the placeholder.
                
                // if the angle is leass than minAngle, this will become the new minangle for comparision with other place holders.
                if (angle < minAngle) 
                {
                    minAngle = angle;
                    bestSpot = availableSpots[i];
                }
            }
        }

        return bestSpot;
    }

    //This function check if the enemy target is actually covered from the line of sight of the player. Will use raycasting for this .
    private bool CheckIfSpotIsValid(Transform spot)
    {
        RaycastHit hit;
        Vector3 direction = target.position - spot.position;
         if(Physics.Raycast(spot.position,direction,out hit))
        {
            if(hit.collider.transform != target)
            {
                return true;
            }
        }

        return false;
    }
         
}
