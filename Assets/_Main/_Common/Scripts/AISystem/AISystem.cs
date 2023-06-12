using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class AISystem : MonoBehaviour
{

    private enum State
    {
        SEARCHING,
        CHASE,
        GO_AWAY
    }

    [SerializeField] private GameObject cube;

    private Character character;
    private NavMeshAgent navMeshAgent;
    private Character actualPlayer;

    private State currentState;

    private bool destinationReached;
    private Vector3 randomPatrolPosition;
    private float navMeshBorderOffset;
    private float searchPlayerRay;
    private float attackRange;
    private float distanceToKeepFromPlayer;
    private MeshCollider meshCollider;

    //Debug


    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        SwitchBehaviour(State.SEARCHING);
    }

    private void Start()
    {
        // to enabled
        //EventManager.OnPossessedCharacterChanged += EventManager_OnPossessedCharacterChanged;
    }

    private void EventManager_OnPossessedCharacterChanged(Character character)
    {
        actualPlayer = character;

        SwitchBehaviour(State.SEARCHING);
    }

    private void Update()
    {
        if (cube != null) //to replace in to actual player
        {
            switch (currentState)
            {
                case State.SEARCHING:
                    SearchForPlayer();
                    break;
                case State.CHASE:
                    Chase();
                    break;
                case State.GO_AWAY:
                    GoAway();
                    break;
            }
        }
    }

    public void Init(Character character)
    {
        this.character = character;

        navMeshBorderOffset = character.GetCharacterType().NavMeshBorderOffset;
        searchPlayerRay = character.GetCharacterType().SearchPlayerRay;
        attackRange = character.GetCharacterType().AttackRange;
        distanceToKeepFromPlayer = character.GetCharacterType().DistanceToKeepFromPlayer;
        meshCollider = character.GetCharacterType().MeshCollider;

        navMeshAgent.speed = character.GetCharacterType().CharacterSpeed;
    }

    private void SwitchBehaviour(State behaviourState)
    {
        switch (behaviourState)
        {
            case State.SEARCHING:
                destinationReached = false;
                currentState = State.SEARCHING;
                break;
            case State.CHASE:
                destinationReached = false;
                navMeshAgent.isStopped = false;
                currentState = State.CHASE;
                break;
            case State.GO_AWAY:
                navMeshAgent.isStopped = true;
                currentState = State.GO_AWAY;
                break;
        }
    }

    private void SearchForPlayer()
    {
        destinationReached = navMeshAgent.velocity == Vector3.zero;

        if (destinationReached)
        {
            randomPatrolPosition = new Vector3(Random.Range(navMeshBorderOffset, meshCollider.bounds.size.x - navMeshBorderOffset), 0f, Random.Range(navMeshBorderOffset, meshCollider.bounds.size.z - navMeshBorderOffset));
        }

        if (Vector3.Distance(cube.transform.position, transform.position) <= searchPlayerRay) // cube to replace actual player
        {
            SwitchBehaviour(State.CHASE);
        }

        navMeshAgent.SetDestination(randomPatrolPosition);
    }

    private void Chase()
    {
        float distanceFromPlayer = Vector3.Distance(this.transform.position, cube.transform.position); //cube to replace actualplayer

        destinationReached = distanceFromPlayer <= attackRange;

        if (distanceToKeepFromPlayer != 0f && distanceFromPlayer <= distanceToKeepFromPlayer)
        {
            SwitchBehaviour(State.GO_AWAY);
        }

        if (destinationReached)
        {
            navMeshAgent.isStopped = true;
            character.GetAttackSystem().PerformAttack(AttackType.LIGHT);
        }
        else
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(cube.transform.position); // replace with actual player
        }
    }

    private void GoAway()
    {
        if(navMeshAgent.isStopped)
        {
            navMeshAgent.isStopped = false;
        }

        Vector3 driveAway = (this.transform.position - cube.transform.position).normalized;

        navMeshAgent.SetDestination(driveAway);

        float chaseDist = Vector3.Distance(this.transform.position, cube.transform.position); // actual player will replace cube

        if(chaseDist >= distanceToKeepFromPlayer)
        {
            SwitchBehaviour(State.CHASE);
        }
    }

    private void OnDestroy()
    {
        EventManager.OnPossessedCharacterChanged -= EventManager_OnPossessedCharacterChanged;
    }

}
