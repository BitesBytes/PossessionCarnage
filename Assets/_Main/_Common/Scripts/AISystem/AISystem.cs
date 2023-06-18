using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody), typeof(AnimatorSystem))]
public class AISystem : MonoBehaviour
{
    private const string PLAYER_LAYER = "Player";

    [SerializeField] private List<Transform> randomPatrolPoints;
    [SerializeField] private List<Transform> notRandomPatrolPoints;

    private enum State
    {
        IDLE,
        PATROL,
        CHASE,
        ATTACK
    }

    private Character character;
    private NavMeshAgent navMeshAgent;
    private Player player;
    private Rigidbody rigidBody;

    private State currentState;
    private State previousState;

    private Transform[] usedPatrolPoints;
    private int usedPatrolPointsAmount;
    private int actualPatrolPointIndex;

    private bool destinationReached;
    private Vector3 randomPatrolPosition;
    private float searchPlayerRay;
    private float attackRange;
    private float distanceToKeepFromPlayer;

    private float stunTimer;
    private float impactForce;

    private float idleTimer;

    private bool attackPerformed;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        usedPatrolPointsAmount = 5;
    }

    private void Start()
    {
        EventManager.OnPossessedCharacterChanged += EventManager_OnPossessedCharacterChanged;

        //rigidBody.AddForce(transform.forward * impactForce * Time.deltaTime, ForceMode.Force); TODO
    }

    private void EventManager_OnPossessedCharacterChanged(Character character)
    {
        player = null;

        SwitchBehaviour(State.IDLE);
    }

    private void Update()
    {
        if (player == null)
        {
            SearchPlayer();
        }

        switch (currentState)
        {
            case State.IDLE:
                Idle();
                break;
            case State.PATROL:
                Patrolling();
                break;
            case State.CHASE:
                Chasing();
                break;
            case State.ATTACK:
                Attacking();
                break;
        }
    }

    private void SearchPlayer()
    {
        Physics.SphereCast(transform.position, searchPlayerRay, transform.forward, out RaycastHit raycastHit);

        if (raycastHit.collider != null)
        {
            player = raycastHit.collider.GetComponent<Player>();
        }
    }

    public void Init(Character character)
    {
        this.character = character;

        searchPlayerRay = character.GetCharacterType().SearchPlayerRay;
        attackRange = character.GetCharacterType().AttackRange;
        distanceToKeepFromPlayer = character.GetCharacterType().DistanceToKeepFromPlayer;
        impactForce = character.GetCharacterType().ImpactForce;

        navMeshAgent.speed = character.GetCharacterType().CharacterSpeed;

        character.GetHealthSystem().OnHealthAmountChanged += HealthSystem_OnHealthAmountChanged;

        SelectPatrolPoints();

        SwitchBehaviour(State.IDLE);
    }

    private void HealthSystem_OnHealthAmountChanged(object sender, HealthSystem.OnHealthAmountChangedEventArgs e)
    {
        if (e.amount < 0f)
        {

        }
    }

    private void SelectPatrolPoints()
    {
        usedPatrolPoints = new Transform[usedPatrolPointsAmount];

        if (randomPatrolPoints.Count > 0)
        {
            for (int i = 0; i < usedPatrolPointsAmount; i++)
            {
                int chosenRandomPatrolPointIndex = UnityEngine.Random.Range(0, randomPatrolPoints.Count);
                usedPatrolPoints[i] = randomPatrolPoints[chosenRandomPatrolPointIndex];

                randomPatrolPoints.RemoveAt(chosenRandomPatrolPointIndex);
            }
        }
        else if (notRandomPatrolPoints.Count > 0)
        {
            for (int i = 0; i < notRandomPatrolPoints.Count; i++)
            {
                usedPatrolPoints[i] = notRandomPatrolPoints[i];
            }
        }
        else
        {
            Debug.LogError("No patrol poin found!");
        }
    }

    private void SwitchBehaviour(State behaviourState)
    {
        switch (behaviourState)
        {
            case State.IDLE:
                navMeshAgent.isStopped = true;
                idleTimer = character.GetCharacterType().IdleTimer;
                currentState = State.IDLE;
                break;
            case State.PATROL:
                navMeshAgent.isStopped = false;
                if (player == null)
                {
                    actualPatrolPointIndex = 0;
                    navMeshAgent.SetDestination(usedPatrolPoints[actualPatrolPointIndex].position);
                }
                currentState = State.PATROL;
                break;
            case State.CHASE:
                navMeshAgent.isStopped = false;
                currentState = State.CHASE;
                break;
            case State.ATTACK:
                navMeshAgent.isStopped = true;
                attackPerformed = false;
                currentState = State.ATTACK;
                break;
        }
    }

    private void Patrolling()
    {
        previousState = State.PATROL;

        AnimatorSystem.IsWalking(navMeshAgent.velocity != Vector3.zero);

        if (player != null)
        {
            SwitchBehaviour(State.CHASE);
        }

        transform.LookAt(navMeshAgent.nextPosition);
        transform.rotation = Quaternion.Lerp(transform.rotation, usedPatrolPoints[actualPatrolPointIndex].rotation, Time.deltaTime * 0.5f);
        destinationReached = Vector3.Distance(gameObject.transform.position, usedPatrolPoints[actualPatrolPointIndex].position) <= 0.1f;

        if (destinationReached)
        {
            actualPatrolPointIndex++;

            if (actualPatrolPointIndex >= usedPatrolPoints.Length)
            {
                actualPatrolPointIndex = 0;
            }

            navMeshAgent.SetDestination(usedPatrolPoints[actualPatrolPointIndex].position);
        }
    }

    private void Chasing()
    {
        if (player != null)
        {
            previousState = State.CHASE;

            AnimatorSystem.IsWalking(navMeshAgent.velocity != Vector3.zero);

            navMeshAgent.SetDestination(player.transform.position);

            float distanceFromPlayer = Vector3.Distance(this.transform.position, player.transform.position);

            destinationReached = distanceFromPlayer <= attackRange;

            //if (distanceToKeepFromPlayer != 0f && distanceFromPlayer <= distanceToKeepFromPlayer)
            //{
            //    SwitchBehaviour(State.GO_AWAY);
            //}

            if (destinationReached)
            {
                SwitchBehaviour(State.ATTACK);
            }
        }
    }

    private void Attacking()
    {
        if (!attackPerformed)
        {
            attackPerformed = true;
            character.GetAttackSystem().PerformAttack((AttackType)UnityEngine.Random.Range(0, (int)AttackType.LAST));
        }

        if (AnimatorSystem.IsAnimationFinished())
        {
            SwitchBehaviour(State.IDLE);
        }
    }

    private void Idle()
    {
        previousState = State.IDLE;

        idleTimer -= Time.deltaTime;
        if (idleTimer <= 0f)
        {
            SwitchBehaviour(State.PATROL);
        }
    }

    private void OnDestroy()
    {
        EventManager.OnPossessedCharacterChanged -= EventManager_OnPossessedCharacterChanged;
        character.GetHealthSystem().OnHealthAmountChanged -= HealthSystem_OnHealthAmountChanged; //MAYBE BUG
    }

}
