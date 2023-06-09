using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MeleeAI : Entity
{
    [SerializeField] private float searchPlayerRay = 10f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private MeshCollider meshCollider;

    private int index;
    private float distanceBeetweenPoints;
    private float patrolRange = 1f;
    private float timer;
    private float maxTimer = 3f;
    private float randomPointMaxDistance;
    private Vector3 randomPatrolPosition;
    private bool destinationReached;
    private float navMeshBorderOffset;

    public List<Transform> PatrolPoints;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        randomPointMaxDistance = navMeshAgent.height * 2f;

        destinationReached = true;

        navMeshBorderOffset = 5f;
    }

    private void Start()
    {
        SwitchBehaviour(State.SEARCHING);

        speed = 10f;
        damage = 0f;
        index = 0;
        rigidBody = GetComponent<Rigidbody>();

        Debug.Log(meshCollider.bounds.size);
    }

    private void Update()
    {
        debugMax = AIBehaviourManager.Instance.GetDebugMax();

        if (debugMax != null)
        {
            switch (currentState)
            {
                case State.SEARCHING:
                    SearchForPlayer();
                    break;
                case State.PATROL:
                    Patrolling();
                    break;
                case State.CHASE:
                    Chase();
                    break;
            }
        }

        //if (!playerSeen)
        //{
        //    timer += Time.deltaTime * 1.0f;

        //    if (timer >= maxTimer) { idle = !idle; timer = 0; }

        //    if (!idle)
        //    {
        //        SwitchBehaviour(State.PATROL);
        //    }
        //}

        //if (playerSeen)
        //{
        //    SwitchBehaviour(State.CHASE);
        //}
    }

    private void SearchForPlayer()
    {
        if (!destinationReached && Vector3.Distance(transform.position, randomPatrolPosition) <= 0.05f)
        {
            destinationReached = true;
        }

        if (destinationReached)
        {
            destinationReached = false;
            randomPatrolPosition = new Vector3(Random.Range(navMeshBorderOffset, meshCollider.bounds.size.x - navMeshBorderOffset), 0f, Random.Range(navMeshBorderOffset, meshCollider.bounds.size.z - navMeshBorderOffset));

            Debug.Log(randomPatrolPosition);
        }

        if (Vector3.Distance(AIBehaviourManager.Instance.GetDebugMax().transform.position, transform.position) <= searchPlayerRay)
        {
            //Player found!
            SwitchBehaviour(State.CHASE);
        }

        navMeshAgent.SetDestination(randomPatrolPosition);
    }

    protected override void Patrolling()
    {

    }

    private void Move()
    {
        direction = (PatrolPoints[index].position - this.transform.position).normalized;
        rigidBody.MovePosition(rigidBody.position + direction * speed * Time.deltaTime);
    }

    protected override void Chase()
    {
        navMeshAgent.SetDestination(debugMax.transform.position);

        //direction = (debugMax.transform.position - this.transform.position).normalized;
        //rigidBody.MovePosition(rigidBody.position + direction * speed * Time.deltaTime);
        //transform.LookAt(debugMax.transform.position);

        ////calculate the distance beetween AI and player and if is less/equal to 1.5f which is the minimum distance display the debug.log

        float dist = Vector3.Distance(this.transform.position, debugMax.transform.position);

        if (dist <= attackRange)
        {
            FistsAttack();
        }
    }

    private void FistsAttack()
    {
        Debug.Log("fists attack");
    }

}
