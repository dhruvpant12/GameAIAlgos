using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Flock : MonoBehaviour
{
    [Header("Sqawn Variables")]
    //Flock is a container that will contain multiple fish in a group.
    [SerializeField] private FlockUnit flockUnitPrefab; // model of fish
    [SerializeField] private int flockSize; // number of fishes
    [SerializeField] private Vector3 spawnBound; // fishes will remain inside this bound .

    [Header("Speed Variable")]
    [Range(0, 10)]
    [SerializeField] private float _minSpeed;
    [Range(0, 10)]
    [SerializeField] private float _maxSpeed;

    public float minSpeed { get { return _minSpeed; } }
    public float maxSpeed { get { return _maxSpeed; } }


    [Header("Detection Distances")]
    [Range(0,10)]
    [SerializeField] private float _cohesionDistance;
    public float cohesionDistance { get { return _cohesionDistance; } }


    public FlockUnit[] allUnits { set; get; } // all the instantiated fish will be stored in this array.

    // Start is called before the first frame update
    void Start()
    {
        GenerateUnits();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < allUnits.Length; i++)
        {
            allUnits[i].MoveUnit();
        }
    }
    private void GenerateUnits()
    {
        //generating fish around the center of the flock gameobject.
        allUnits = new FlockUnit[flockSize];

        for(int i=0; i< flockSize;i++)
        {
            // getting local position of fish.
            var randomVector = UnityEngine.Random.insideUnitSphere;
            randomVector = new Vector3(randomVector.x * spawnBound.x, randomVector.y * spawnBound.y, randomVector.z * spawnBound.z);

            //setting fish position relative to the center of the flock
            var spawnPosition = transform.position + randomVector;
            var rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
            allUnits[i] = Instantiate(flockUnitPrefab, spawnPosition, rotation);

            //assigning the instantiated fish to this flock.
            allUnits[i].AssignFlock(this);
            allUnits[i].InitialiseSpeed(UnityEngine.Random.Range(minSpeed, maxSpeed));
        }
    }

   
}
