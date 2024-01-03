using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

 
public class EnemyAI : MonoBehaviour
{
    [SerializeField ]private float startingHealth;
    [SerializeField] private float lowHealthThreshold;
    [SerializeField] private float healthRestoreRate;

    [SerializeField] private float shootingRange;
    [SerializeField] private float chasingRange;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Cover[] availableCovers;

    

    private Material material;
    private Transform bestCoverSpot;
    private NavMeshAgent agent;


    private Node topNode;

    private float _currentHealth;

    public float currentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, startingHealth); }
    }


    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = startingHealth;
        
        ConstructBehaviourTree();
    }

    // Update is called once per frame
    void Update()
    {
        topNode.Evaluate();
        if (topNode.nodeState == NodeState.FAILURE) SetColor(Color.red);

        currentHealth += healthRestoreRate * Time.deltaTime;
    }

    private void OnMouseDown()
    {
        currentHealth -= 20f;
    }
    private void ConstructBehaviourTree()
    {
        IsCoverAvailableNode coverAvailableNode = new IsCoverAvailableNode(availableCovers, this, playerTransform);
        GoToCoverNode goToCoverNode = new GoToCoverNode(agent, this);
        HealthNode healthNode = new HealthNode(this, lowHealthThreshold);
        IsCoveredNode isCoveredNode = new IsCoveredNode(playerTransform, transform);
        ChaseNode chaseNode = new ChaseNode(playerTransform, agent, this);
        RangeNode chasingRangeNode = new RangeNode(chasingRange, playerTransform, transform);
        RangeNode ShootingRangeNode = new RangeNode(shootingRange, playerTransform, transform);
        ShootNode shootNode = new ShootNode(agent, this);

        Sequence chaseSequence = new Sequence(new List<Node> { chasingRangeNode, chaseNode });
        Sequence shootingSequence = new Sequence(new List<Node> { ShootingRangeNode, shootNode });
        Sequence goToCoverSequence = new Sequence(new List<Node> { coverAvailableNode, goToCoverNode });
        Selector findCoverSelector = new Selector(new List<Node> { goToCoverSequence, chaseSequence });
        Selector tryToTakeCoverSelector = new Selector(new List<Node> { isCoveredNode, findCoverSelector });
        Sequence mainCoverSequence = new Sequence(new List<Node> { healthNode, tryToTakeCoverSelector });

        topNode = new Sequence(new List<Node> { mainCoverSequence, shootingSequence, chaseSequence });



    }

 

    

    public void SetBestCover(Transform bestSpot)
    {
        this.bestCoverSpot = bestSpot;
    }

    public Transform GetBestCover()
    {
        return bestCoverSpot;
    }

    public void SetColor(Color color)
    {
        material.color = color;
    }
}
