using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockUnit : MonoBehaviour
{
    //Unit should not be able to detect units that are behind it . 
    [SerializeField] private float FOVangle;
    [SerializeField] private float smoothDamp; // smaller the value , faster the convergence to the cohension vector.

    private List<FlockUnit> cohesionNeighbours = new List<FlockUnit>();
    private Flock assignedFlock;
    private Vector3 currentVelocity;
    private float speed;

    public Transform myTransform { get; set; }

    private void Awake()
    {
        myTransform = transform;
    }

    public void AssignFlock(Flock assignedFlock)
    {
        this.assignedFlock = assignedFlock;
    }

    public void InitialiseSpeed(float speed)
    {
        this.speed = speed;
    }
     public void MoveUnit()
    {
        //Find all neighbours to this fish. Units behind the fish donot get included.
        FindNeighbours();

        CalculateSpeed();

        //Get the average cohesion vector.
        var cohesionVector = CalculateCohesionVector();

        // Turning the fish forwards vector towards the average cohesion vector.
        var moveVector = Vector3.SmoothDamp(myTransform.forward, cohesionVector, ref currentVelocity,smoothDamp);
        moveVector = moveVector.normalized * speed;

        //Rotating and moving the fish.
        myTransform.forward = moveVector;
        myTransform.position += moveVector * Time.deltaTime;

    }

    private void CalculateSpeed()
    {
        if (cohesionNeighbours.Count == 0) return ;

        speed = 0;
        for(int i = 0; i < cohesionNeighbours.Count; i++)
        {
            speed += cohesionNeighbours[i].speed;
        }

        speed /= cohesionNeighbours.Count;
        speed = Mathf.Clamp(speed, assignedFlock.minSpeed, assignedFlock.maxSpeed);
    }

    private void FindNeighbours()
    {
        cohesionNeighbours.Clear();

        var allUnits = assignedFlock.allUnits; // all the fish in the flock object.
        for(int i=0 ;i<allUnits.Length; i++)
        {
            var currentUnit = allUnits[i];
            if(currentUnit != this)
            {
                float currentNeighbourDistanceSqr = Vector3.SqrMagnitude(currentUnit.transform.position - myTransform.position);
                if (currentNeighbourDistanceSqr <= assignedFlock.cohesionDistance * assignedFlock.cohesionDistance) ;
                {
                    cohesionNeighbours.Add(currentUnit);
                }

            
            }
        }
    }

    private Vector3 CalculateCohesionVector()
    {
        var cohesionVector = Vector3.zero;
        // no neibhours found.
        if (cohesionNeighbours.Count == 0) return cohesionVector;

        int neighboursInFOV = 0; // counter to keep how many neighbours in Field of Vision.

        for(int i=0; i < cohesionNeighbours.Count; i++)
        {
            //Checking if neigbour is in FOV of the current fish.
            if(IsInFOV( cohesionNeighbours[i].myTransform.position))
            {
                neighboursInFOV++;

                cohesionVector += cohesionNeighbours[i].myTransform.position;

            }
        }

        //No neighbour in field of view . Return zero vector.
        if(neighboursInFOV==0)
         return cohesionVector;

        //Taking average of the cohesion vector.
        cohesionVector /= neighboursInFOV;

        //Making it local to the fish.
        cohesionVector -= myTransform.position;

        cohesionVector = Vector3.Normalize(cohesionVector);

        return cohesionVector;


    }

    private bool IsInFOV(Vector3 position)
    {
        return Vector3.Angle(myTransform.forward, position - myTransform.position) <= FOVangle;
    }
}
