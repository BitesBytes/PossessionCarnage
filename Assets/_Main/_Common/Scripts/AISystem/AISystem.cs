using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class AISystem : MonoBehaviour
{
    private const string PLAYER_LAYER = "Player";

    private List<Transform> randomPatrolPoints = new List<Transform>(); //Debug se tolgo il SerializeField il foreach allo start funziona
    [SerializeField] private List<Transform> notRandomPatrolPoints;

    private enum State
    {
        IDLE,
        PATROL,
        CHASE,
        ATTACK,
        EXPLOSION
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

    private int searchPlayerLayerMask;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        usedPatrolPointsAmount = 5;
        searchPlayerLayerMask = 1 << LayerMask.NameToLayer(PLAYER_LAYER);
    }

    private void Start()
    {
        EventManager.OnPossessedCharacterChanged += EventManager_OnPossessedCharacterChanged;

        //rigidBody.AddForce(transform.forward * impactForce * Time.deltaTime, ForceMode.Force); TODO

        GameObject patrolNode = GameObject.Find("PatrolPoints");
        Transform[] points = patrolNode.GetComponentsInChildren<Transform>();

        foreach(Transform child in patrolNode.transform)
        {
            randomPatrolPoints.Add(child);
        }
    }

    private void EventManager_OnPossessedCharacterChanged(Character character)
    {
        player = null;

        SwitchBehaviour(State.IDLE);
    }

    private void Update()
    {
        if (navMeshAgent.isActiveAndEnabled)
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
                case State.EXPLOSION:
                    Exploding();
                    break;
            }
        }
    }

    private void SearchPlayer()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if(Physics.SphereCast(ray, searchPlayerRay, out RaycastHit hit, searchPlayerRay, searchPlayerLayerMask))
        {
            player = hit.collider.GetComponent<Player>();
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
        if (navMeshAgent.isActiveAndEnabled)
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
                case State.EXPLOSION:
                    currentState = State.EXPLOSION;
                    break;
            }
        }
    }

    private void Patrolling()
    {
        previousState = State.PATROL;

        AnimatorSystem.IsWalking(character.GetAnimator(), navMeshAgent.velocity != Vector3.zero);

        if (player != null)
        {
            SwitchBehaviour(State.CHASE);
        }

        transform.LookAt(navMeshAgent.nextPosition);
        transform.rotation = Quaternion.Lerp(transform.rotation, usedPatrolPoints[actualPatrolPointIndex].rotation, Time.deltaTime * 0.5f);
        destinationReached = Vector3.Distance(new Vector3(gameObject.transform.position.x, 0f, gameObject.transform.position.z), new Vector3(usedPatrolPoints[actualPatrolPointIndex].position.x, 0f, usedPatrolPoints[actualPatrolPointIndex].position.z)) <= 0.25f;

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

            AnimatorSystem.IsWalking(character.GetAnimator(), navMeshAgent.velocity != Vector3.zero);

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

        if (AnimatorSystem.IsAnimationFinished(character.GetAnimator()))
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

    private void Exploding()
    {
        enabled = false;
        //TODO
    }

    public void DisalbeAI()
    {
        navMeshAgent.isStopped = true;
        destinationReached = true;
        AnimatorSystem.IsWalking(character.GetAnimator(), false);
        SwitchBehaviour(State.EXPLOSION);
    }

    private void OnDestroy()
    {
        EventManager.OnPossessedCharacterChanged -= EventManager_OnPossessedCharacterChanged;
        character.GetHealthSystem().OnHealthAmountChanged -= HealthSystem_OnHealthAmountChanged; //MAYBE BUG
    }

}
